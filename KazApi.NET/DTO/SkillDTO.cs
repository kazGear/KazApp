using System.Text.Json.Serialization;
using KazApi.Domain.monster._Skill;

namespace KazApi.DTO
{
    /// <summary>
    /// スキルパラメータクラス
    /// </summary>
    public class SkillDTO
    {
        [JsonPropertyName("SkillId")]
        public int SkillId { get; set; }
        [JsonPropertyName("SkillName")]
        public string SkillName { get; set; }
        [JsonPropertyName("SkillType")]
        public int SkillType { get; set; }
        [JsonPropertyName("ElementType")]
        public int ElementType { get; set; }
        [JsonPropertyName("StateType")]
        public int StateType { get; set; }
        [JsonPropertyName("TargetType")]
        public int TargetType { get; set; }
        [JsonPropertyName("Attack")]
        public int Attack { get; set; }
        [JsonPropertyName("Weight")]
        public int Weight { get; set; }
        [JsonPropertyName("Critical")]
        public double Critical { get; set; }
        [JsonPropertyName("Remarks")]
        public string? Remarks { get; set; } = string.Empty;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SkillDTO() { }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SkillDTO(ISkill model)
        {
            SkillId = model.SkillId;
            SkillName = model.SkillName;
            SkillType = model.SkillType;
            ElementType = model.ElementType;
            StateType = model.StateType;
            TargetType = model.TargetType;
            Attack = model.Attack;
            Weight = model.Weight;
            Critical = model.Critical;
            Remarks = "";
        }

        /// <summary>
        /// DTOへ変換
        /// </summary>
        public static IEnumerable<SkillDTO> ModelToDTO(IEnumerable<ISkill> models)
        {
            IList<SkillDTO> result = [];

            foreach (ISkill model in models)
                result.Add(new SkillDTO(model));

            return result;
        }
    }
}
