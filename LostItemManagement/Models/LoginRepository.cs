using Npgsql;
using System;
using System.Collections.Generic;

namespace LostItemManagement.Models
{
    public class LoginRepository
    {
        private readonly DatabaseContext _dbContext;

        public LoginRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// ユーザーIDとパスワードでユーザーを取得
        /// </summary>
        public User? GetUser(string userName, string password)
        {
            using var conn = _dbContext.CreateConnection();
            try
            {
                conn.Open();
                var query = "SELECT user_id, user_name, user_password FROM users WHERE user_name = @userName AND user_password = @password";
                using var cmd = new NpgsqlCommand(query, (NpgsqlConnection?)conn);
                cmd.Parameters.AddWithValue("userName", userName);
                cmd.Parameters.AddWithValue("password", password);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new User
                    {
                        userId = reader.GetInt32(0),
                        userName = reader.IsDBNull(1) ? null : reader.GetString(1),
                        password = reader.IsDBNull(2) ? null : reader.GetString(2)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving user: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// トークンをDBに保存
        /// </summary>
        public void SaveLoginToken(int userId, string token, DateTime loginDate, DateTime expireDate)
        {
            using var conn = _dbContext.CreateConnection();
            try
            {
                conn.Open();
                var query = "INSERT INTO login (user_id, token, login_date, expire_date) VALUES (@userId, @token, @loginDate, @expireDate)";
                using var cmd = new NpgsqlCommand(query, (NpgsqlConnection?)conn);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("token", token);
                cmd.Parameters.AddWithValue("loginDate", loginDate);
                cmd.Parameters.AddWithValue("expireDate", expireDate);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving login token: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        /// <summary>
        /// トークンでログイン情報を取得
        /// </summary>
        public Login? GetLoginByToken(string token)
        {
            using var conn = _dbContext.CreateConnection();
            try
            {
                conn.Open();
                var query = "SELECT login_id, user_id, token, login_date, expire_date FROM login WHERE token = @token";
                using var cmd = new NpgsqlCommand(query, (NpgsqlConnection?)conn);
                cmd.Parameters.AddWithValue("token", token);

                using var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return new Login
                    {
                        loginId = reader.GetInt32(0),
                        userId = reader.GetInt32(1),
                        token = reader.IsDBNull(2) ? null : reader.GetString(2),
                        loginDate = reader.IsDBNull(3) ? null : reader.GetDateTime(3),
                        expireDate = reader.IsDBNull(4) ? null : reader.GetDateTime(4)
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving login by token: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        /// <summary>
        /// トークンの有効期限を更新（必要に応じて）
        /// </summary>
        public void UpdateTokenExpireDate(string token, DateTime newExpireDate)
        {
            using var conn = _dbContext.CreateConnection();
            try
            {
                conn.Open();
                var query = "UPDATE login SET expire_date = @expireDate WHERE token = @token";
                using var cmd = new NpgsqlCommand(query, (NpgsqlConnection?)conn);
                cmd.Parameters.AddWithValue("expireDate", newExpireDate);
                cmd.Parameters.AddWithValue("token", token);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error updating token expire date: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }
    }
}