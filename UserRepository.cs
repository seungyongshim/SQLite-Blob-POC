using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;

namespace SQLite_Blob_POC
{
    public class UserRepository
    {
        public UserRepository(Func<SqliteConnection> dbConnect)
        {
            DbConnect = dbConnect;
        }

        private Func<SqliteConnection> DbConnect { get; }

        public void Delete(long key)
        {
            using (var c = DbConnect())
            {
                c.Open();
                c.Execute($@"DELETE FROM USERS WHERE USER_ID = {key}");
            }
        }

        public void Delete(User userInformation)
        {
            using (var c = DbConnect())
            {
                c.Open();
                c.Execute($@"DELETE FROM USERS WHERE USER_ID = {userInformation.USER_ID}");
            }
        }

        public IEnumerable<User> FindAll()
        {
            using (var c = DbConnect())
            {
                c.Open();
                return c.Query<User>("SELECT * FROM USERS");
            }
        }

        public void InitializeDatabase()
        {
            using (var c = DbConnect())
            {
                c.Open();

                var tableCommand = "CREATE TABLE IF NOT "
                                 + "EXISTS USERS "
                                 + "("
                                 + "USER_ID INTEGER PRIMARY KEY, "
                                 + "PASSWORD TEXT, "
                                 + "USER_NAME TEXT, "
                                 + "USER_GROUP TEXT, "
                                 + "BLOB BLOB "
                                 + ")";

                SqliteCommand createTable = new SqliteCommand(tableCommand, c);

                createTable.ExecuteReader();
            }
        }

        public void Insert(User user)
        {
            using (var c = DbConnect())
            {
                c.Open();
                c.Execute(@"INSERT INTO USERS (PASSWORD, USER_NAME, USER_GROUP, BLOB)" +
                                  "VALUES (:PASSWORD, :USER_NAME, :USER_GROUP, :BLOB)", user);
            }
        }
    }
}