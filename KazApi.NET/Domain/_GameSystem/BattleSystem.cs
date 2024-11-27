using CSLib.Lib;
using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.Domain._Monster;
using KazApi.Domain._Monster._Skill;
using KazApi.Domain._Monster._State;
using KazApi.DTO;

namespace KazApi.Domain._GameSystem
{
    /// <summary>
    /// 戦闘システムクラス
    /// </summary>
    public class BattleSystem
    {        
        private static readonly ILog<BattleMetaData> LOG = new BattleLogger();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        private BattleSystem()
        {
            
        }
        
        /// <summary>
        /// 現状のHPを確認
        /// </summary>
        public static void CurrentHp(IEnumerable<IMonster> monsters)
        {
            foreach (IMonster m in monsters)
            {
                LOG.Logging(new BattleMetaData(
                      $"[ {m.MonsterName} ] HP: {(m.Hp <= 0 ? 0 : m.Hp)} / {m.MaxHp}"
                    + $" {(m.Hp <= 0 ? "（戦闘不能）" : "")}"
                ));
            }
        }

        /// <summary>
        /// 敵を選択する
        /// </summary>
        public static IMonster SelectOneEnemy(IEnumerable<IMonster> monsters)
        {
            IEnumerable<IMonster> enemies = monsters.Where(e => e.Hp > 0);

            int enemyIndex = URandom.RandomInt(0, enemies.Count());
            return enemies.ElementAt(enemyIndex);
        }
        /// <summary>
        /// 弱点属性によるダメージの算出
        /// </summary>
        public static int WeeknessDamage(IMonster enemy, ISkill skill, int damage)
        {
            if (skill.ElementType == CElement.NONE.VALUE) return damage;
            if (enemy.Week == CElement.NONE.VALUE) return damage;

            int week = enemy.Week;
            bool isWeekness = week == skill.ElementType;

            if (isWeekness)
            {
                damage = (int)(damage * CSysRate.WEEK_DAMAGE.VALUE);
                LOG.Logging(new BattleMetaData("弱点ダメージ！"));
            }
            return damage;
        }
        /// <summary>
        /// クリティカルによるダメージ
        /// </summary>
        public static int CriticalDamage(ISkill skill, int damage)
        {
            double randomVal = URandom.RandomDouble(0.0, 1.0);
            bool isCritical = randomVal <= skill.Critical;

            if (isCritical)
            {
                damage = (int)(damage * CSysRate.CRITICAL_DAMAGE.VALUE);
                LOG.Logging(new BattleMetaData("クリティカルヒット！"));
            }
            return damage;
        }
        /// <summary>
        /// モンスタをランダムに選出する
        /// </summary>
        public static IEnumerable<IMonster> MonsterSelector(IEnumerable<IMonster> monsters, int needAmount)
        {
            if (monsters.Count() < needAmount) throw new Exception("モンスターの選択数が多すぎます。");

            IList<IMonster> result = [];
            IList<int> usedIndex = [];

            // 必要数のモンスタを用意
            for (int i = 0; i < needAmount; i++)
            {
                int index = URandom.RandomInt(0, monsters.Count());

                // 同じモンスターは選べない
                while (usedIndex.Contains(index)) 
                    index = URandom.RandomInt(0, monsters.Count());
                usedIndex.Add(index);

                IMonster monster = monsters.ElementAt(index);
                result.Add(monster);
            }
            return result;
        }

        /// <summary>
        /// モンスターをランダムに選出する
        /// </summary>
        public static IEnumerable<T> MonsterSelector<T>(IEnumerable<T> monsters, int needAmount)
        {
            if (monsters.Count() < needAmount) throw new Exception("モンスターの選択数が多すぎます。");
            if (monsters.Count() < 2) throw new Exception("バトルは２体以上必要です。");

            IList<T> result = [];
            IList<int> usedMonsterId = [];

            // 必要数のモンスタを用意
            for (int i = 0; i < needAmount; i++)
            {
                int monsterId = URandom.RandomInt(0, monsters.Count());

                // 同じモンスターは選べない
                while (usedMonsterId.Contains(monsterId))
                    // ランダムに選出
                    monsterId = URandom.RandomInt(0, monsters.Count());

                usedMonsterId.Add(monsterId);

                T monster = monsters.ElementAt(monsterId);
                result.Add(monster);
            }
            return result;
        }

        /// <summary>
        /// 行動順を決定する
        /// </summary>
        public static IEnumerable<IMonster> ActionOrder(IEnumerable<IMonster> monsters)
        {
            // スピードを乱数調整した上で順番決め
            IList<IMonster> result =
                monsters.Where(e => e.Hp > 0)
                        .OrderByDescending(
                            e => URandom.RandomChangeInt(e.Speed, 0.5))
                        .ToList();

            return result;
        }

        /// <summary>
        /// 状態異常解除
        /// </summary>
        public static void DisabledStatus(IMonster me)
        {
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
            LOG.Logging(new BattleMetaData());
        }

        /// <summary>
        /// 賭け金レートを算出
        /// </summary>
        /// <param name="monsters"></param>
        public static void CalcBetRate(IEnumerable<MonsterDTO> monsters)
        {
            int monsterCount = monsters.Count() - 1; // モンスター数が多いほど倍率UP
            double maxScore = monsters.Max(e => e.BetScore);

            foreach (MonsterDTO monster in monsters)
            {
                if (maxScore == monster.BetScore)
                {
                    monster.BetRate = double.Parse(
                        (maxScore / monster.BetScore * monsterCount * 1.15).ToString("F2")
                        );
                }
                else
                {
                    monster.BetRate = double.Parse(
                        (maxScore / monster.BetScore * monsterCount).ToString("F2")
                        );
                }
            }
        }

        /// <summary>
        /// モンスター毎の戦績、勝率の加工
        /// </summary>
        /// <param name="report"></param>
        /// <returns></returns>
        public static IEnumerable<MonsterReportDTO> ResultsOfMonster(IEnumerable<MonsterReportDTO> report)
        {
            IEnumerable<MonsterReportDTO> result 
                = report.Select(e => new MonsterReportDTO
                {
                    MonsterId = e.MonsterId,
                    MonsterName = e.MonsterName,
                    BattleCount = e.BattleCount,
                    Wins = e.Wins,
                    WinRate = (e.Wins / (double)e.BattleCount * 100).ToString("N2") + "%"
                });
            return result;
        }

        /// <summary>
        /// バトル毎のレポート加工
        /// </summary>
        /// <param name="report"></param>
        public static IEnumerable<BattleReportDTO> ResultsOfBattle(IEnumerable<BattleReportDTO> report)
        {
            IEnumerable<BattleReportDTO> result 
                = report.Select(e => new BattleReportDTO
                {
                    BattleId = e.BattleId,
                    BattleEndDateStr = e.BattleEndDate.ToString().Substring(0, 10),
                    BattleEndTimeStr = e.BattleEndTime.ToString().Substring(0, 8),
                    Serial = e.Serial,
                    MonsterId = e.MonsterId,
                    MonsterName = e.MonsterName,
                    IsWin = e.IsWin
                });
            return result;
        }

    }
}
