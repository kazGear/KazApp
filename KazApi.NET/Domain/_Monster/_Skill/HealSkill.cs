using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.DTO;
using KazApi.Lib;

namespace KazApi.Domain._Monster._Skill
{
    /// <summary>
    /// 回復スキルクラス
    /// </summary>
    public class HealSkill : ISkill, IPositiveSkill
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HealSkill(SkillDTO dto, string effectFilePath)
                  : base(dto, effectFilePath) { }

        public override void Use(IEnumerable<IMonster> monsters, IMonster me)
        {
            int healPoint = URandom.RandomChangeInt(base.Attack, CSysRate.MAGIC_SKILL_DAMAGE.VALUE);

            // MaxHp以上に回復はできない
            int healAble = me.MaxHp - me.Hp;
            healPoint = healAble < healPoint ? healAble : healPoint;

            _Log.Logging(new BattleMetaData(
                me.MonsterId,
                me.Hp,
                healPoint * -1,
                base.SkillId,
                  $"{me.MonsterName}は{healPoint}ポイント回復した！"
                ));

            // マイナス数値の減算でHP加算
            me.AcceptDamage(healPoint * -1);
        }
    }
}
