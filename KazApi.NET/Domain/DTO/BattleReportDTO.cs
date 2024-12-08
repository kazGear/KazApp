namespace KazApi.Domain.DTO
{
    public class BattleReportDTO
    {
        public int BattleId { get; set; }
        public DateTime BattleEndDate { get; set; }
        public string BattleEndDateStr { get; set; }
        public TimeSpan BattleEndTime { get; set; }
        public string BattleEndTimeStr { get; set; }
        public int Serial { get; set; }
        public string MonsterId { get; set; }
        public string MonsterName { get; set; }
        public bool IsWin { get; set; }
    }
}
