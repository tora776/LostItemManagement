using Npgsql;
using System.Data;

namespace LostItemManagement.Models
{
    public class DatabaseContext
    {
        private readonly string _connectionString;
        public DatabaseContext(IConfiguration configuration) {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            
        }

        public IDbConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }

        public void connectDB(string _connectionString)
        {
            using (var conn = new NpgsqlConnection(_connectionString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("データベース接続成功！");
                }
                catch
                {
                    Console.WriteLine("データベース接続失敗");
                }
            }
        }

        public DataTable SqlExecute(string query, NpgsqlConnection conn)
        {
            NpgsqlCommand sql = new NpgsqlCommand(query, conn);
            NpgsqlDataReader reader = sql.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            return dt;
        }

    }
}
