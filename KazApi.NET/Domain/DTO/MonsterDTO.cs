using KazApi.Domain._Monster;
using System.Text.Json.Serialization;

namespace KazApi.Domain.DTO
{
    /// <summary>
    /// モンスターパラメータクラス
    /// </summary>
    public class MonsterDTO
    {
        [JsonPropertyName("MonsterId")]
        public int MonsterId { get; set; }
        [JsonPropertyName("MonsterName")]
        public string MonsterName { get; set; }
        [JsonPropertyName("MonsterType")]
        public int MonsterType { get; set; }
        [JsonPropertyName("Hp")]
        public int Hp { get; set; }
        [JsonPropertyName("MaxHp")]
        public int MaxHp { get; set; } = 0;
        [JsonPropertyName("Attack")]
        public int Attack { get; set; }
        [JsonPropertyName("Speed")]
        public int Speed { get; set; }
        [JsonPropertyName("Week")]
        public int Week { get; set; }
        [JsonPropertyName("Skills")]
        public IEnumerable<SkillDTO> Skills { get; set; } = [];
        [JsonPropertyName("Status")]
        public IEnumerable<StateDTO> Status { get; set; } = [];

        public IEnumerable<string> StatusName { get; set; } = [];
        [JsonPropertyName("Winner")]

        public double BetScore { get; set; }

        public double BetRate { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MonsterDTO() { }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MonsterDTO(IMonster model)
        {
            MonsterId = model.MonsterId;
            MonsterName = model.MonsterName;
            MonsterType = model.MonsterType;
            Hp = model.Hp;
            if (MaxHp == 0) MaxHp = model.MaxHp;
            Attack = model.Attack;
            Speed = model.Speed;
            Week = model.Week;
        }
    }
}
