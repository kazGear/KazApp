namespace KazApi.DTO
{
    public class MonsterReportDTO
    {
        public int MonsterId { get; set; }
        public string MonsterName { get; set; }
        public int BattleCount { get; set; }
        public int Wins { get; set; }
        public string WinRate { get; set; }
    }
}
