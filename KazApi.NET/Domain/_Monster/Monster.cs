using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.Domain._Monster._Skill;
using KazApi.Domain._Monster._State;
using KazApi.DTO;
using KazApi.Lib;

namespace KazApi.Domain._Monster
{
    /// <summary>
    /// モンスタークラウ
    /// </summary>
    public class Monster : IMonster
    {
        private static readonly int TARGET_NONE = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Monster(MonsterDTO dto, IEnumerable<ISkill> skills, IEnumerable<IState> status)
                : base(dto, skills, status) { }


        public override ISkill SelectSkill()
        {
            IList<ISkill> skills = new List<ISkill>(_skills);

            // ランダムにスキル選択
            int randomSkillIndex = URandom.RandomInt(0, skills.Count());
            ISkill skill = skills[randomSkillIndex];
            return skill;
        }
        
        protected override void AttackMove(IEnumerable<IMonster> monsters, ISkill skill)
        {
            string attackMessage = $"{base.MonsterName}は {skill.SkillName} を放った！";

            // 無害な攻撃のメッセージ
            if (skill.SkillType == ((int)CSkillType.NOT_MOVE))
                attackMessage = $"{base.MonsterName}は {skill.SkillName} ...";

            base._Log.Logging(new BattleMetaData(TARGET_NONE, attackMessage + "\n"));
            
            skill.Use(monsters, this);
        }

        protected override void PositiveMove(IEnumerable<IMonster> monsters, ISkill skill)
        {
            string attackMessage = $"{base.MonsterName}は {skill.SkillName} を放った！";
            base._Log.Logging(new BattleMetaData(base.MonsterId, attackMessage + "\n"));
            skill.Use([this], this);
        }
    }
}
