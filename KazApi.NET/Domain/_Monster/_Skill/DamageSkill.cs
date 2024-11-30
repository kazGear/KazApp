using CSLib.Lib;
using KazApi.Common._Log;
using KazApi.Domain._Const;
using KazApi.Domain._GameSystem;
using KazApi.Domain.DTO;

namespace KazApi.Domain._Monster._Skill
{
    /// <summary>
    /// ダメージ付与クラス
    /// </summary>
    public class DamageSkill : ISkill
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DamageSkill(SkillDTO dto, string effectFilePath)
                    : base(dto, effectFilePath) { }

        public override void Use(IEnumerable<IMonster> monsters, IMonster me)
        {
            int target = OneOrAll();

            if (target == CTarget.ENEMY_RANDOM.VALUE) // 単体攻撃
            {
                IMonster enemy = BattleSystem.SelectOneEnemy(monsters);
                AttackEnemy(enemy, me);
            }
            else if (target == CTarget.ENEMY_ALL.VALUE) // 全体攻撃
            {
                // 全体攻撃は威力弱め
                PowerDown();
                monsters = monsters.Where(e => e.Hp > 0);
                foreach (IMonster enemy in monsters) AttackEnemy(enemy, me);
                InitPower();
            }
            else
            {
                throw new Exception("不適切なターゲットタイプです。");
            }
        }
        /// <summary>
        /// 敵を攻撃
        /// </summary>
        private void AttackEnemy(IMonster enemy, IMonster me)
        {
            // ダメージ量が多少揺れる
            int damage = URandom.RandomChangeInt(Attack + me.Attack, CSysRate.PHYSICAL_SKILL_DAMAGE.VALUE);

            // 弱点等のダメージ欲正
            damage = BattleSystem.WeeknessDamage(enemy, this, damage);
            damage = BattleSystem.CriticalDamage(this, damage);

            _Log.Logging(new BattleMetaData(
                enemy.MonsterId,
                enemy.Hp,
                damage,
                SkillId,
                $"{enemy.MonsterName}は{damage}のダメージを受けた。")
                );

            enemy.AcceptDamage(damage);
        }
        /// <summary>
        /// 単体攻撃か全体攻撃かを選択する
        /// </summary>
        private int OneOrAll()
        {
            if (TargetType == CTarget.ENEMY_RANDOM_OR_ALL.VALUE)
            {
                return URandom.RandomBool() ? CTarget.ENEMY_RANDOM.VALUE
                                            : CTarget.ENEMY_ALL.VALUE;
            }
            return TargetType;

        }

    }
}
