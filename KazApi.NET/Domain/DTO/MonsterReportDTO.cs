namespace KazApi.Domain.DTO
{
    public class MonsterReportDTO
    {
        public string MonsterId { get; set; }
        public string MonsterName { get; set; }
        public int BattleCount { get; set; }
        public int Wins { get; set; }
        public string WinRate { get; set; }
    }
}
