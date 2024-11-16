
namespace KazApi.DTO
{
    public class UserDTO
    {
        public string LoginId { get; set; }
        public string LoginPass { get; set; }
        public int FailedLoginCnt { get; set; }
        public bool IsLoginDisabled { get; set; }
        public string UserName { get; set; }
        public int Cash { get; set; }
        public int Wins { get; set; }
        public int WinsGetCash { get; set; }
        public int Losses { get; set; }
        public int BankruptcyCnt { get; set; }
        public int LossesLostCash { get; set; }
    }
}
