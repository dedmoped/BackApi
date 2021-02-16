using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class SqliteHelper
    {
        internal static List<User> GetData()
        {
            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=db.sqlite;Version=3;"))
                {
                    connection.Open();

                    using (var cmd = new SQLiteCommand(@"SELECT * FROM Users", connection))

                    {
                        using (var rdr = cmd.ExecuteReader())
                        {
                            List<User> users = new List<User>();
                            while (rdr.Read())
                            {
                                users.Add(new User()
                                {
                                    Login = rdr.GetString(1),
                                    Email = rdr.GetString(2),
                                    Password = rdr.GetString(3),
                                    Role = rdr.GetString(4)
                                }) ;
                            }
                            return users;
                        }
                    }

                }
            }
            catch
            {

            }
            return new List<User>();
        }

        internal static List<Data> GetTasks()
        {

            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=db.sqlite;Version=3;"))
                {
                    connection.Open();

                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "Select * from UserTask";
                    using (var rdr = insertSQL.ExecuteReader())
                    {
                        List<Data> users = new List<Data>();
                        while (rdr.Read())
                        {
                            users.Add(new Data()
                            {
                                Id=rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                Description = rdr.GetString(2),
                                lastGetDataTime= rdr.GetString(3),
                                sourceApi=rdr.GetString(5),
                                CronTime=rdr.GetString(4),
                                userId=rdr.GetInt32(6)
                            });
                        }
                        return users;
                    }

                }
            }
            catch
            {

            }
            return new List<Data>();
        }

        internal static List<User> FindUser(string login,string password)
        {
            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=db.sqlite;Version=3;"))
                {
                    connection.Open();

                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "Select * from Users where login = @login or email = @login and password =@password";
                    insertSQL.Parameters.AddWithValue("@login", login);
                    insertSQL.Parameters.AddWithValue("@password", password);
                   
                        using (var rdr = insertSQL.ExecuteReader())
                        {
                            List<User> users = new List<User>();
                            while (rdr.Read())
                            {
                                users.Add(new User()
                                {
                                    Login = rdr.GetString(1),
                                    Email = rdr.GetString(2),
                                    Password = rdr.GetString(3),
                                    Role = rdr.GetString(4)
                                });
                            }
                            return users;
                        }

                }
            }
            catch
            {

            }
            return new List<User>();
        }

        internal static void AddUser(User user)
        {
            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=db.sqlite;Version=3;"))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "Insert into Users(Login,Email,Password,Role) values(@Login,@Email,@Password,@Role)";
                    insertSQL.Parameters.AddWithValue("@Login",user.Login);
                    insertSQL.Parameters.AddWithValue("@Email", user.Email);
                    insertSQL.Parameters.AddWithValue("@Password",user.Password);
                    insertSQL.Parameters.AddWithValue("@Role",user.Role);
                    insertSQL.ExecuteNonQuery();

                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        internal static void SetUserdata(Data data)
        {
            try
            {
                using (var connection = new SQLiteConnection(@"Data Source=db.sqlite;Version=3;"))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "Insert into UserTask(Name,Description,LastGetDataTime,CronTime,sourceApi,userId) values(@Name,@Description,@LastGetDataTime,@CronTime,@sourceApi,@userId)";
                    insertSQL.Parameters.AddWithValue("@Name", data.Name);
                    insertSQL.Parameters.AddWithValue("@Description", data.Description);
                    insertSQL.Parameters.AddWithValue("@LastGetDataTime", data.lastGetDataTime);
                    insertSQL.Parameters.AddWithValue("@CronTime", data.CronTime);
                    insertSQL.Parameters.AddWithValue("@sourceApi", data.sourceApi);
                    insertSQL.Parameters.AddWithValue("@userId", data.userId);
                    insertSQL.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
