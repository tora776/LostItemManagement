namespace LostItemManagement.Models
{
    public class Lost
    {
        public int lostId { get; set; }
        public int userId { get; set; }
        public int lostFlag { get; set; }
        public DateTime? lostDate { get; set; }
        public DateTime? foundDate { get; set; }
        public string? lostItem {  get; set; }
        public string? lostPlace { get; set; }
        public string? lostDetailedPlace { get; set; }
        public DateTime? registrateDate { get; set; }
        public DateTime? updateDate { get; set; }

    }
}
