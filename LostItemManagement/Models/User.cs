﻿namespace LostItemManagement.Models
{
    public class User
    {
        public int userId { get; set; }
        public string? userName { get; set; }
        public string? password {  get; set; }
        public DateTime? registrateDate { get; set; }
        public DateTime? updateDate { get; set; }
    }
}
