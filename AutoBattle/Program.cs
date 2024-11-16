using KazApi.Common._Const;
using KazApi.Domain._Monster;
using KazApi.Repository;
using KazApi.Service;
using KazApi.Domain._Monster._State;
using KazApi.Lib;
using KazApi.DTO;
using System.Text;

Console.WriteLine("Auto battle start...");

IDatabase _posgre = new PostgreSQL();
BattleService _service = new BattleService();


int battleTimes = 3;
for (int i = 0; i < battleTimes; i++)
{
    try
    {
        Console.OutputEncoding = Encoding.UTF8;

        /**
         * モンスタ－用意
         */

        // モンスターデータ等の読込み
        IEnumerable<MonsterDTO> monstersFromDB = _service.SelectMonsters();
        IEnumerable<SkillDTO> skillsFromDB = _service.SelectSkills();
        IEnumerable<MonsterSkillDTO> monsterSkillFromDB = _service.SelectMonsterSkills();

        // モンスターDTO構築
        IEnumerable<MonsterDTO> monstersDTO =
            _service.MappingToMonsterDTO(monstersFromDB, skillsFromDB, monsterSkillFromDB);

        // 参加モンスター（モンスター数はランダム）
        IEnumerable<MonsterDTO> battleMonstersDTO
            = _service.MonsterSelector(monstersDTO, URandom.RandomInt(2, 7));

        // 戦闘用モンスターを構築
        IEnumerable<IMonster> battleMonsters = _service.CreateBattleMonsters(battleMonstersDTO);

        // TODO 未実装 チーム決め
        ((List<IMonster>)battleMonsters).ForEach(e => e.DefineTeam(CTeam.A.VALUE));

        if (battleMonsters.Where(e => e.Team == (CTeam.UNKNOWN.VALUE)).Count() > 0)
        {
            throw new Exception("チーム決めが完了していません。");
        }

        IEnumerable<IMonster> alives = []; // 生き残りモンスター

        /**
         * 戦闘不能が1人以下になるまで戦う
         */

        do
        {
            // 行動順決め
            IEnumerable<IMonster> orderedMonsters = _service.ActionOrder(battleMonsters);

            // モンスタ達のーの行動
            foreach (IMonster me in orderedMonsters)
            {
                // 行動不可
                if (me.Hp <= 0) continue;

                // 状態異常の効果
                me.StateImpact();

                // モンスターの行動
                IList<IMonster> otherMonsters = orderedMonsters.Where(e => e.MonsterId != me.MonsterId).ToList();
                if (me.IsMoveAble()) me.Move(otherMonsters);

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

                // HP 現状確認
                // Console.Writeline("================================================================");
                foreach (IMonster monster in battleMonsters)
                {
                    int hp = monster.Hp > 0 ? monster.Hp : 0;
                    // Console.Writeline($"name: {monster.MonsterName}, HP: {hp}");
                }

                // 勝敗判定
                alives = battleMonsters.Where(e => e.Hp > 0);
                IMonster? alive = alives.Count() == 1 ? alives.Single() : null;

                //if (alives.Count() == 1)
                //    Console.Writeline($"Winner: {alive!.MonsterName} !!");
                //else if (alives.Count() == 0)
                //    Console.Writeline($"All loser ...");

            }
        } while (alives.Count() > 1);

        /**
         * 戦績の記録
         */
        IList<MonsterDTO> records = [];
        foreach (IMonster monster in battleMonsters)
        {
            records.Add(new MonsterDTO(monster));
        }
        DateTime endDate = DateTime.Now;
        TimeSpan endTime = new TimeSpan(endDate.Ticks);

        _service.InsertBattleResult(records, endDate, endTime);


        // 間隔を空け再選（2分ごと、最終回は待たない）
        if (i < 2)
        {
            await Task.Delay(120000);
            // Console.Writeline("再選待ち...(2分)");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("batch [AutoBattle] が異常終了しました。");
        Console.WriteLine(e);
    }

}
Console.WriteLine($"Auto battle finish. （{battleTimes}戦）");