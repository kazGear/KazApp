using KazApi.Domain.DTO;

namespace KazApi.Domain._Monster._Skill
{
    /// <summary>
    /// 行動しないスキル
    /// </summary>
    public class NoMoveSkill : ISkill
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NoMoveSkill(SkillDTO dto, string effectFilePath)
                    : base(dto, effectFilePath) { }

        public override void Use(IEnumerable<IMonster> monsters, IMonster me)
        {
            // 何もしない
        }
    }
}
