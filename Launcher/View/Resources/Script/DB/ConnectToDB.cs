using Npgsql;
using System.Windows;

namespace Launcher.View.Resources.Script.DB
{
    internal class ConnectToDB
    {
        private const string ip = "localhost";
        private const string port = "5432";
        private const string database = "readmer_launcher";
        private const string userId = "postgres";
        private const string password = "cr2032";

        private const string connection = $"Server={ip};Port={port};Database={database}; User id = {userId};Password={password};";

        public static void ConnectToAuthorization()
        {
            string sql = "SELECT * FROM public.\"user\"";

            using (var conn = new NpgsqlConnection(connection))
            {
                conn.Open();
                using (var command = new NpgsqlCommand(sql, conn))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string login = reader.GetString(1);
                            string password = reader.GetString(2);
                        }
                    }
                }
            }
        }
    }
}
