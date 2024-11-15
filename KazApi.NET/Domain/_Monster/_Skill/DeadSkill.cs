﻿
using KazApi.Common._Log;
using KazApi.Domain._GameSystem;
using KazApi.DTO;
using KazApi.Lib;

namespace KazApi.Domain._Monster._Skill
{
    /// <summary>
    /// 即死スキルクラス
    /// </summary>
    public class DeadSkill : ISkill
    {
        private static readonly int DEAD_DAMAGE = 999;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DeadSkill(SkillDTO dto, string effectFilePath)
                  : base(dto, effectFilePath) {  }

        public override void Use(IEnumerable<IMonster> monsters, IMonster me)
        {
            IMonster enemy = BattleSystem.SelectOneEnemy(monsters);
            
            // 即死確立
            double rate = 1.0 - ((double)enemy.Hp / (double)enemy.MaxHp);
            rate = rate < 0.05 ? 0.05 : rate; // 最低でも５％は効く
            
            double randomVal = URandom.RandomDouble(0.0, 1.0);
            int deadDamage = 9999;
            
            if (randomVal <= rate)
            {
                enemy.AcceptDamage(DEAD_DAMAGE);
                base._Log.Logging(new BattleMetaData(
                    enemy.MonsterId,
                    enemy.Hp,
                    deadDamage,
                    base.SkillId,
                    $"{enemy.MonsterName}は戦闘不能になった。"));
            }
            else
            {
                base._Log.Logging(new BattleMetaData(
                    enemy.MonsterId,
                    base.SkillId
                    ,$"{enemy.MonsterName}には効かなかった。"));
            }
        }
    }
}
