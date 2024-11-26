using CSLib.Lib;
using KazApi.Common._Const;
using KazApi.Common._Log;
using KazApi.Domain.monster._Skill;
using KazApi.Domain.monster._State;
using KazApi.DTO;

namespace KazApi.Domain.monster
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
            string attackMessage = $"{MonsterName}は {skill.SkillName} を放った！";

            // 無害な攻撃のメッセージ
            if (skill.SkillType == CSkillType.NOT_MOVE.VALUE)
                attackMessage = $"{MonsterName}は {skill.SkillName} ...";

            _Log.Logging(new BattleMetaData(TARGET_NONE, attackMessage + "\n"));

            skill.Use(monsters, this);
        }

        protected override void PositiveMove(IEnumerable<IMonster> monsters, ISkill skill)
        {
            string attackMessage = $"{MonsterName}は {skill.SkillName} を放った！";
            _Log.Logging(new BattleMetaData(MonsterId, attackMessage + "\n"));
            skill.Use([this], this);
        }
    }
}
