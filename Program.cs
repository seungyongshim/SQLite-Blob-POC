using Microsoft.Data.Sqlite;
using System;

namespace SQLite_Blob_POC
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<SqliteConnection> conn = () => new SqliteConnection($"Data Source=Application.db;Cache=Shared");

            var users = new UserRepository(conn);

            users.InitializeDatabase();

            users.Insert(new UserInformation
            {
                BLOB = new byte[] {0x00, 0x00, 0x00, 0x01},
                USER_NAME = "Hong",
                PASSWORD = "GilDong",
                USER_GROUP = "Hero"
            });

            foreach (var user in users.FindAll())
            {
                Console.WriteLine(user.ToString());
            }
        }
    }
}
