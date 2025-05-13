using Npgsql;


namespace LostItemManagement.Models
{
    public class LostRepository
    {
        private readonly DatabaseContext _dbContext;
        public LostRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Lost> SelectLostRepository(string lostItem, string lostPlace, string lostDetailedPlace)
        {
            var items = new List<Lost>();
            using var conn = _dbContext.CreateConnection();
            
            try
                {
                    conn.Open();
                    // SELECT文のクエリを作成
                    var query = "SELECT lost_id, user_id, lost_flag, lost_date, found_date, lost_item, lost_place, lost_detailed_place FROM losts WHERE user_id = @USER_ID";
                    using var cmd = new NpgsqlCommand(query, (NpgsqlConnection?)conn);
                    // ユーザーIDをパラメータとして追加
                    cmd.Parameters.Add("@USER_ID", NpgsqlTypes.NpgsqlDbType.Integer).Value = 1; // Assuming user_id is 1 for this example

                    // lostItemがNull・空文字の場合は、条件式を含めない。
                    if (!string.IsNullOrEmpty(lostItem))
                        {
                            query += " AND lost_item = @LOST_ITEM";
                            cmd.Parameters.Add("@LOST_ITEM", NpgsqlTypes.NpgsqlDbType.Varchar).Value = lostItem;
                        }
                    // lostPlaceがNull・空文字の場合は、条件式を含めない。
                    if (!string.IsNullOrEmpty(lostPlace))
                        {
                            query += " AND lost_place = @LOST_PLACE";
                            cmd.Parameters.Add("@LOST_PLACE", NpgsqlTypes.NpgsqlDbType.Varchar).Value = lostPlace;
                        }
                    // lostDetailedPlaceがNull・空文字の場合は、条件式を含めない。
                    if (!string.IsNullOrEmpty(lostDetailedPlace))
                        {
                            query += " AND lost_detailed_place = @LOST_DETAILED_PLACE";
                            cmd.Parameters.Add("@LOST_DETAILED_PLACE", NpgsqlTypes.NpgsqlDbType.Varchar).Value = lostDetailedPlace;
                        }

                    // 更新した検索条件をコマンドに再設定
                    cmd.CommandText = query;

                    // SQL文を実行
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
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error retrieving lost items: " + ex.Message);
                    }
                    finally
                    {
                        conn.Close();
                    }
                return items;
        }

        public void InsertLostRepository(Lost lost, int LostId)
        {
            using var conn = _dbContext.CreateConnection();
            try
            {
                conn.Open();

                var query = "INSERT INTO losts (lost_id, user_id, lost_flag, lost_date, found_date, lost_item, lost_place, lost_detailed_place, registrate_date) " +
                            "VALUES (:lostId, :userId, :lostFlag, :lostDate, :foundDate, :lostItem, :lostPlace, :lostDetailedPlace, CURRENT_TIMESTAMP)";
                using var cmd = new NpgsqlCommand(query, (NpgsqlConnection?)conn);
                cmd.Parameters.AddWithValue("lostId", LostId);
                cmd.Parameters.AddWithValue("userId", lost.userId);
                cmd.Parameters.AddWithValue("lostFlag", lost.lostFlag);
                cmd.Parameters.AddWithValue("lostDate", lost.lostDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("foundDate", lost.foundDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("lostItem", lost.lostItem ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("lostPlace", lost.lostPlace ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("lostDetailedPlace", lost.lostDetailedPlace ?? (object)DBNull.Value);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error inserting lost item: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }

        public void UpdateLostRepository(Lost lost)
        {
            using var conn = _dbContext.CreateConnection();
            try
            {
                conn.Open();
                var query = "UPDATE losts SET lost_item = @lostItem, lost_place = @lostPlace, lost_detailed_place = @lostDetailedPlace, update_date = CURRENT_TIMESTAMP WHERE lost_id = @lostId";
                using var cmd = new NpgsqlCommand(query, (NpgsqlConnection?)conn);
                cmd.Parameters.AddWithValue("lostItem", lost.lostItem ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("lostPlace", lost.lostPlace ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("lostDetailedPlace", lost.lostDetailedPlace ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("lostId", lost.lostId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating lost item: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        public void DeleteLostRepository(int lostId)
        {
            using var conn = _dbContext.CreateConnection();
            try
            {
                conn.Open();
                var query = "DELETE FROM losts WHERE lost_id = @lostId";
                using var cmd = new NpgsqlCommand(query, (NpgsqlConnection?)conn);
                cmd.Parameters.AddWithValue("lostId", lostId);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error deleting lost item: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        ///  InsertLostItemメソッドで使用するlost_idの最大値を取得するメソッド
        /// </summary>
        /// <returns>lostIdの最大値</returns>
        public int GetMaxLostId()
        {
            // 最大のlost_idを初期化
            int maxLostId = 0;
            using var conn = _dbContext.CreateConnection();

            try
            {
                conn.Open();
                // SELECT文のクエリを作成
                var query = "SELECT Max(lost_id) FROM losts";
                using var cmd = new NpgsqlCommand(query, (NpgsqlConnection?)conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        maxLostId = reader.GetInt32(0);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving lost items: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return maxLostId;

        }
        public List<Lost> SelectLostByIds(List<int> lostIds)
        {
            var query = "SELECT * FROM Losts WHERE lost_id = ANY(@LostIds)";
            using var conn = _dbContext.CreateConnection();
            var items = new List<Lost>();
            using var cmd = new NpgsqlCommand(query, (NpgsqlConnection?)conn);

            cmd.Parameters.AddWithValue("LostIds", NpgsqlTypes.NpgsqlDbType.Array | NpgsqlTypes.NpgsqlDbType.Integer, lostIds.ToArray());
            
            try
            {
                conn.Open();
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
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving lost items: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return items;
        }
    }
}

