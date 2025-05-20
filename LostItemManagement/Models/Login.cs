namespace LostItemManagement.Models
{
    public class Login
    {
        public int loginId { get; set; }
        public int userId { get; set; }
        public string? token { get; set; }
        public DateTime? loginDate { get; set; }
        public DateTime? expireDate { get; set; }
    }
}
