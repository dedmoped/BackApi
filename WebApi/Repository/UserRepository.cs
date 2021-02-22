using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Utils;

namespace WebApi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string connectionstring;
        private readonly ILogger _logger;
        public UserRepository(IConfiguration configuration,ILogger<UserRepository> _logger)
        {
            connectionstring = configuration["Database:ConnectionString"];
            this._logger = _logger;
        }
        public void Create(User user)
        {
            try
            {
                _logger.LogInformation("Start Create User");
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "Insert into Users(Login,Email,Password) values(@Login,@Email,@Password);";
                    insertSQL.Parameters.AddWithValue("@Login", user.Login);
                    insertSQL.Parameters.AddWithValue("@Email", user.Email);
                    insertSQL.Parameters.AddWithValue("@Password", user.Password);
                    insertSQL.ExecuteNonQuery();

                }
                _logger.LogInformation("Create User Completed");
            }
           catch(Exception ex)
            {
                _logger.LogError(ex.Message, "Create User fail");
            }
        }

        public IEnumerable<User> FindUser(string login)
        {
            try
            {
                _logger.LogInformation("Start FindUser");
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    connection.Open();

                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "Select Users.Id,login,email,password,Roles.role  from Users inner join UserRole on Users.id=UserRole.User_id inner join Roles on UserRole.Role_id=Roles.Id  where login = @login or email = @login;";
                    insertSQL.Parameters.AddWithValue("@login", login);
                    using (var rdr = insertSQL.ExecuteReader())
                    {
                        List<User> users = new List<User>();
                        while (rdr.Read())
                        {
                            users.Add(new User()
                            {
                                Id = rdr.GetInt32(0),
                                Login = rdr.GetString(1),
                                Email = rdr.GetString(2),
                                Password = rdr.GetString(3),
                                Role = rdr.GetString(4)
                            });
                        }
                        _logger.LogInformation("FindUser Completed");
                        return users;
                    }

                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, "FindUser Fail");
                return new List<User>();
            }
        }

        public IEnumerable<UsersStatistic> GetStatistic()
        {
            _logger.LogInformation("Start GetStatistic");
            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "SELECT Users.id,Users.Email, (select count(*) from UserTask where userId=Users.Id) FROM UserTask inner join Users on UserTask.userId = Users.Id group by Users.Id";
                    insertSQL.ExecuteNonQuery();

                    using (var rdr = insertSQL.ExecuteReader())
                    {

                        List<UsersStatistic> users = new List<UsersStatistic>();
                        while (rdr.Read())
                        {
                            users.Add(new UsersStatistic() { UserId = rdr.GetInt32(0), UserEmail = rdr.GetString(1), CountTask = rdr.GetInt32(2) });
                        }
                        _logger.LogInformation("GetStatistic completed");
                        return users;
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, "FindUser Fail");
                return new List<UsersStatistic>();
            }
        }
    }
}
