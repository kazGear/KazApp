using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.Domain._GameSystem;
using KazApi.DTO;
using KazApi.Lib;

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

            if (target == ((int)CTarget.ENEMY_RANDOM)) // 単体攻撃
            {
                IMonster enemy = BattleSystem.SelectOneEnemy(monsters);
                AttackEnemy(enemy, me);
            }
            else if (target == ((int)CTarget.ENEMY_ALL)) // 全体攻撃
            {
                // 全体攻撃は威力弱め
                base.PowerDown();
                monsters = monsters.Where(e => e.Hp > 0);
                foreach (IMonster enemy in monsters) AttackEnemy(enemy, me);
                base.InitPower();
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
            int damage = URandom.RandomChangeInt(base.Attack + me.Attack, CSysRate.PHYSICAL_SKILL_DAMAGE);
            
            // 弱点等のダメージ欲正
            damage = BattleSystem.WeeknessDamage(enemy, this, damage);
            damage = BattleSystem.CriticalDamage(this, damage);

            _Log.Logging(new BattleMetaData(
                enemy.MonsterId,
                enemy.Hp,
                damage,
                base.SkillId,
                $"{enemy.MonsterName}は{damage}のダメージを受けた。")
                );

            enemy.AcceptDamage(damage);
        }
        /// <summary>
        /// 単体攻撃か全体攻撃かを選択する
        /// </summary>
        private int OneOrAll()
        {
            if (base.TargetType == ((int)CTarget.ENEMY_RANDOM_OR_ALL))
            {
                return URandom.RandomBool() ? ((int)CTarget.ENEMY_RANDOM)
                                            : ((int)CTarget.ENEMY_ALL);
            }
            return base.TargetType;

        }

    }
}
