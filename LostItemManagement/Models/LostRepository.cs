namespace LostItemManagement.Models
{
    public class LostRepository
    {
        private readonly DatabaseContext _dbContext;
        public LostRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;

        }

        public List<Lost> GetLostItem()
        {
            var items = new List<Lost>();

            using (var conn = _dbContext.CreateConnection())
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT lost_id, user_id, lost_flag, lost_date, found_date, lost_item, lost_place, lost_detailed_place FROM losts";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            items.Add(new Lost
                            {
                                lostId = reader.GetInt32(0),
                                userId = reader.GetInt32(1),
                                lostFlag = reader.GetInt32(2),
                                lostDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                                foundDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                                lostItem = reader.IsDBNull(5) ? null : reader.GetString(5),
                                lostPlace = reader.IsDBNull(6) ? null : reader.GetString(6),
                                lostDetailedPlace = reader.IsDBNull(7) ? null : reader.GetString(7)
                            });
                        }
                    }
                    return items;
                }
            }
        }
    }
}

