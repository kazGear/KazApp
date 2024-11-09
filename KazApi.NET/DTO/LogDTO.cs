using System.Text.Json.Serialization;
using KazApi.Common._Log;

namespace KazApi.DTO
{
    public class LogDTO
    {
        [JsonPropertyName("Log")]
        public IEnumerable<BattleMetaData> Memory { get; set; }
    }
}
