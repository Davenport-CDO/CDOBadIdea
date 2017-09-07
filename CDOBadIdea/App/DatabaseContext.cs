using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDOBadIdea.App
{
    public class DatabaseContext
    {
        private SqliteConnection _connection;

        public DatabaseContext()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            Initialize();
        }

        private void Initialize()
        {
            _connection.Execute("CREATE TABLE User (username TEXT PRIMARY KEY, password TEXT)");
            _connection.Execute("CREATE TABLE SSN (name TEXT PRIMARY KEY, ssn TEXT)");

            CreateUser("dave", "12345");
            CreateUser("admin", "password");
        }

        private void CreateUser(string username, string password)
        {
            //Use parameterized queries - data is sent separately from the query itself, so no chance of injection
            _connection.Execute("INSERT INTO User (username, password) VALUES (@username, @password)", new { username, password });
        }

        private void AddSSN(string name, string ssn)
        {
            _connection.Execute("INSERT INTO SSN (name, ssn) VALUES (@name, @ssn)", new { name, ssn });
        }

        public bool ValidLogin(string username, string password)
        {
            //No parameterized query - data is embedded directly into the query string, very easy to inject into
            return _connection.ExecuteScalar<bool>($"SELECT EXISTS(SELECT 1 FROM User WHERE username='{username}' AND password='{password}' LIMIT 1)");
        }
    }
}
