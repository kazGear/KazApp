﻿
using KazApi.Common._Log;
using KazApi.Domain._GameSystem;
using KazApi.DTO;

namespace KazApi.Domain._Monster._Skill
{
    /// <summary>
    /// 割合ダメージクラス
    /// </summary>
    public class RateDamageSkill : ISkill
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public RateDamageSkill(SkillDTO dto, string effectFilePath)
                        : base(dto, effectFilePath) { }

        public override void Use(IEnumerable<IMonster> monsters, IMonster me)
        {
            IMonster enemy = BattleSystem.SelectOneEnemy(monsters);

            // 現HPの割合ダメージ
            double damage = enemy.Hp * (base.Attack / 100.0);

            _Log.Logging(new BattleMetaData(
                enemy.MonsterId,
                enemy.Hp,
                (int)damage,
                base.SkillId,
                $"{enemy.MonsterName}は{(int)damage}のダメージを受けた。")
                );

            enemy.AcceptDamage((int)damage);
        }
    }
}