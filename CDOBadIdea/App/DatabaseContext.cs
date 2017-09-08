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
            _connection.Execute("CREATE TABLE SSN (name TEXT PRIMARY KEY, number TEXT)");
            _connection.Execute("CREATE TABLE Blog (id INTEGER PRIMARY KEY AUTOINCREMENT, title TEXT, author TEXT, date TEXT, content TEXT)");

            CreateUser("dave", "12345");
            CreateUser("admin", "password");
            CreateUser("jmazouri", "12281995");

            AddSSN("John Mazouri", "156-43-5555");
            AddSSN("Matthew Woodhead", "224-15-5555");
            AddSSN("Gavin Stewart", "863-75-5555");
            AddSSN("Mark McKinnon", "769-90-5555");
            AddSSN("Pete Anderson", "534-47-5555");

            AddBlogPost("Downtime", "admin", "9/1/2017", "Hello,<br>We will be having downtime on September 5th 2017 to implement our new online banking system. All services will be unavailable during this time. We apologize for the inconvenience.");
            AddBlogPost("Welcome", "John Mazouri", "9/7/2017", "Welcome to <a href='/about'>CDO Credit Union</a>. We provide a safe, secure location to deposit your savings, get loans, and do all your banking. Our new online banking system is up and running - please feel free to log in and contact support if you have any issues.");
            AddBlogPost("Happy Birthday!", "admin", "12/28/2016", "Happy birthday to our president, John Mazouri! He was born on December 28, 1995, and has been doing great since. May he have many more years as our great president! <a href='/about'>All of our users</a> thank you! <iframe width='560' height='315' src='https://www.youtube.com/embed/xxOviBI-8fc' frameborder='0' allowfullscreen></iframe>");

        }

        public IEnumerable<string> GetUsers()
        {
            return _connection.Query<string>("SELECT username FROM User");
        }

        public void AddBlogPost(string title, string author, string date, string content)
        {
            _connection.Execute("INSERT INTO Blog (title, author, date, content) VALUES (@title, @author, @date, @content)", new { title, author, date, content });
        }

        public void DeleteBlogPost(int id)
        {
            _connection.Execute("DELETE FROM Blog WHERE @id = id", new { id });
        }

        public IEnumerable<BlogPost> GetBlogPosts()
        {
            return _connection.Query<BlogPost>("SELECT * FROM Blog ORDER BY id DESC");
        }

        private void CreateUser(string username, string password)
        {
            //Use parameterized queries - data is sent separately from the query itself, so no chance of injection
            _connection.Execute("INSERT INTO User (username, password) VALUES (@username, @password)", new { username, password });
        }

        public void AddSSN(string name, string number)
        {
            _connection.Execute("INSERT INTO SSN (name, number) VALUES (@name, @number)", new { name, number });
        }

        public bool ValidLogin(string username, string password)
        {
            //No parameterized query - data is embedded directly into the query string, very easy to inject into
            return _connection.ExecuteScalar<bool>($"SELECT EXISTS(SELECT 1 FROM User WHERE username='{username}' AND password='{password}' LIMIT 1)");
        }

        public bool UserExists(string username)
        {
            return _connection.ExecuteScalar<bool>($"SELECT EXISTS(SELECT 1 FROM User WHERE username='{username}' LIMIT 1)");
        }

        public IEnumerable<SSN> GetSSNs()
        {
            return _connection.Query<SSN>("SELECT * FROM SSN");
        }
    }
}
