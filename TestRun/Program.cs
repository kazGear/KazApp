﻿using CSLib.Lib;
using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.Domain._Factory;
using KazApi.Domain._GameSystem;
using KazApi.Domain._Monster;
using KazApi.Domain._Monster._Skill;
using KazApi.Domain._Monster._State;
using KazApi.DTO;
using KazApi.Repository;
using KazApi.Service;
using Microsoft.EntityFrameworkCore.Storage.Json;
using System.Net;
using System.Runtime.InteropServices;

/*
 print util
 */
void Print(object o) => Console.WriteLine(o);
void PrintAll<T>(IEnumerable<T> os)
{
    foreach (T o in os) Console.WriteLine(o!.ToString());
}

/************************************************************
 * 
 * 汎用テストエリア
 * 
 ************************************************************/

bool doTest = true; // テスト稼働可否
if (!doTest) return;
Print(">>> test info.");

UTimeMeasure.Start();


Print(OSPlatform.Windows);
Print(OSPlatform.OSX);
Print(OSPlatform.Linux);

Print(RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "windows" : "?");
Print(RuntimeInformation.IsOSPlatform(OSPlatform.OSX) ? "OSX" : "?");
Print(RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ? "Linux" : "?");

Print(UEnvironment.IsRuntime());

Print(UTimeMeasure.Stop());
Print(">>> test end...");

/************************************************************
 * **********************************************************
 * バトル・テストエリア
 * **********************************************************
 ************************************************************/

// オブジェクト構築
IDatabase posgre = new PostgreSQL();
BattleService service = new BattleService();

UTimeMeasure.Start(); // SQL測定

// DB読込み
IEnumerable<MonsterDTO> monstersFromDB =
    posgre.Select<MonsterDTO>(SQL.MonsterSQL.SelectMonsters());
IEnumerable<SkillDTO> skillFromDB =
    posgre.Select<SkillDTO>(SQL.SkillSQL.SelectSkill());
IEnumerable<MonsterSkillDTO> monsterSkillFromDB =
    posgre.Select<MonsterSkillDTO>(SQL.MonsterSkillSQL.SelectMonsterSkill());
IEnumerable<CodeDTO> stateCodeFromDB =
    posgre.Select<CodeDTO>(SQL.CodeSQL.SelectCode())
          .Where(e => e.Category == ((int)CCodeType.STATE));

Print(UTimeMeasure.Stop()); // SQL測定

// スキルオブジェクト生成
SkillFactory skillFactory = new SkillFactory(stateCodeFromDB);
IEnumerable<ISkill> skillModels = skillFactory.Create(skillFromDB);

// モンスターオブジェクト生成
MonsterFactory monsterFactory = new MonsterFactory();
IEnumerable<IMonster> monsterModels = 
    monsterFactory.CreateModel(monstersFromDB, skillModels, monsterSkillFromDB);


// 参加モンスター（ランダム）
IEnumerable<IMonster> battleMonsters = 
    BattleSystem.MonsterSelector(monsterModels, 4);

// テスト用モンスター用意
IList<IMonster> testMonsters = new List<IMonster>();
testMonsters.Add(monsterModels.Where(m => m.MonsterId == ((int)CMonster.マイコニド)).Single());
testMonsters.Add(monsterModels.Where(m => m.MonsterId == ((int)CMonster.グリーンスライム)).Single());
testMonsters.Add(monsterModels.Where(m => m.MonsterId == ((int)CMonster.プリースト)).Single());
testMonsters.Add(monsterModels.Where(m => m.MonsterId == ((int)CMonster.デーモン)).Single());
battleMonsters = testMonsters;
//goblin.SetSkill(new List<ISkill>()
//{
//    skillModels.Where(s => s.Id == ((int)CSkill.打撃)).Single(),
//    skillModels.Where(s => s.Id == ((int)CSkill.ポイズン)).Single()
//});


// TODO 未実装 チーム決め
((List<IMonster>)battleMonsters).ForEach(e => e.DefineTeam((int)CTeam.A));

ILog<BattleMetaData> log = new BattleLogger();


bool onMoreBattle = true;
bool enableBattle = false; // バトル稼働可否
string nextInfo = "\n▼ ▽ ▼ ▽ press enter ... ▼ ▽ ▼ ▽s";

try
{
    if (battleMonsters.Where(e => e.Team == ((int)CTeam.UNKNOWN)).Count() > 0)
    {
        throw new Exception("チーム決めが完了していません。");
    }

    while (onMoreBattle && enableBattle)
    {
        // 行動順決め
        IEnumerable<IMonster> orderedMonsters = service.ActionOrder(battleMonsters);

        log.Logging(new BattleMetaData(">>> 行動順"));
        PrintAll(log.DumpMemory());

        ((List<IMonster>)orderedMonsters).ForEach(e => log.Logging(new BattleMetaData(e.MonsterName)));
        PrintAll(log.DumpMemory());

        // モンスターの行動
        foreach (IMonster me in orderedMonsters)
        {
            if (me.Hp <= 0)
            {
                continue;
            }

            // 順番決め
            service.WhoseTurn(me);
            PrintAll(log.DumpMemory());

            // 状態異常の効果
            me.StateImpact();
            PrintAll(log.DumpMemory());

            // モンスターの行動
            IList<IMonster> otherMonsters = orderedMonsters.Where(e => e.MonsterId != me.MonsterId).ToList();
            if (me.IsMoveAble())
                me.Move(otherMonsters);
            PrintAll(log.DumpMemory());

            // 状態異常解除
            IEnumerable<IState> currentStatus = me.CurrentStatus();
            ISet<IState> changedStatus = new HashSet<IState>();
            foreach (IState state in currentStatus)
            {
                if (!state.IsDisable())
                    changedStatus.Add(state);
                else
                    state.DisabledLogging(me);
            }
            me.UpdateStatus(changedStatus);
            PrintAll(log.DumpMemory());

            Print(nextInfo);
            Console.ReadLine();
        }

        // HP 現状確認
        BattleSystem.CurrentHp(battleMonsters);
        PrintAll(log.DumpMemory());

        // 勝敗判定
        service.BattleResult(battleMonsters);       
        PrintAll(log.DumpMemory());

        //if (existWinner || allLoser) break; 

        // バトル継続可否
        Print($"\nもう一度戦いますか？ [ yes: press enter] [ no: any input ]");
        onMoreBattle = Console.ReadLine() == string.Empty ? true : false;

        if (!onMoreBattle)
        {
            Print("\nバトルを終了します。");
            Print(nextInfo);
            Console.ReadLine();
            break;
        }
    }

    bool needAllLog = false;
    if (log.DumpAll().Count > 0 && needAllLog)
    {
        Print(">>> 全ログ出力");
        PrintAll(log.DumpAll());
    }
} 
catch (Exception e)
{
    Print("無効試合！");
    Print(e.Message);
    Print(e.InnerException!);
}