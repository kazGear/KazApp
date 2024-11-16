using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.Domain._Monster;
using KazApi.Domain._Monster._Skill;
using KazApi.Lib;

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
    }
}
