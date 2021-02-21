using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class SqliteHelper
    {
        static string connectionstring = @"Data Source=db.sqlite;Version=3;";
        internal static List<User> GetData()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
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

        internal static List<Data> GetTasks(string userId)
        {

            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
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
                                userId=rdr.GetInt32(6),
                                apiParams=rdr.GetString(7)
                                
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

        internal static List<UsersStatistic> GetUsersStatistic()
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "SELECT Users.id,Users.Email, (select count(*) from UserTask where userId=Users.Id) FROM UserTask inner join Users on UserTask.userId = Users.Id";
                    insertSQL.ExecuteNonQuery();

                    using (var rdr = insertSQL.ExecuteReader())
                    {
                       
                        List<UsersStatistic> users = new List<UsersStatistic>();
                        while (rdr.Read())
                        {
                            users.Add(new UsersStatistic() { userId = rdr.GetInt32(0), userEmail = rdr.GetString(1), countTask = rdr.GetInt32(2) });    
                        }
                        return users;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal static void deleteTask(string id)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "delete from UserTask  where id=@id";
                    insertSQL.Parameters.AddWithValue("@id",id);
                    insertSQL.ExecuteNonQuery();

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal static List<User> FindUser(string login,string password)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
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
                                    Id=rdr.GetInt32(0),
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
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "Insert into Users(Login,Email,Password,Role) values(@Login,@Email,@Password,@Role)";
                    insertSQL.Parameters.AddWithValue("@Login",user.Login);
                    insertSQL.Parameters.AddWithValue("@Email", user.Email);
                    insertSQL.Parameters.AddWithValue("@Password",user.Password);
                    insertSQL.Parameters.AddWithValue("@Role","user");
                    insertSQL.ExecuteNonQuery();

                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        internal static string Userdata(Data data,string userId)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "Insert into UserTask(Name,Description,LastGetDataTime,CronTime,sourceApi,userId,apiParams) values(@Name,@Description,@LastGetDataTime,@CronTime,@sourceApi,@userId,@apiParams);select last_insert_rowid()";
                    insertSQL.Parameters.AddWithValue("@Name", data.Name);
                    insertSQL.Parameters.AddWithValue("@Description", data.Description);
                    insertSQL.Parameters.AddWithValue("@LastGetDataTime", data.lastGetDataTime);
                    insertSQL.Parameters.AddWithValue("@CronTime", data.CronTime);
                    insertSQL.Parameters.AddWithValue("@sourceApi", data.sourceApi);
                    insertSQL.Parameters.AddWithValue("@userId", userId);
                    insertSQL.Parameters.AddWithValue("@apiParams", data.apiParams);

                    using (var rdr = insertSQL.ExecuteReader())
                    {
                        string id="";
                        while (rdr.Read())
                        {

                            id = rdr.GetInt32(0).ToString();

                        }
                        return id;
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return "Error";
        }

        internal static void setLastTime(string taskid)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = $"update UserTask  set LastGetDataTime=@time where id=@taskid";
                    insertSQL.Parameters.AddWithValue("@time", DateTime.Now.ToString());
                    insertSQL.Parameters.AddWithValue("@taskid", taskid);
                    insertSQL.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        internal static void UpdateTask(Data data)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "update  UserTask SET id = @id,Name = @Name,Description =@Description,CronTime =@CronTime,sourceApi = @sourceApi,apiParams = @apiParams WHERE id =@id";
                    insertSQL.Parameters.AddWithValue("@id", data.Id);
                    insertSQL.Parameters.AddWithValue("@Name", data.Name);
                    insertSQL.Parameters.AddWithValue("@Description", data.Description);
                    insertSQL.Parameters.AddWithValue("@CronTime", data.CronTime);
                    insertSQL.Parameters.AddWithValue("@sourceApi", data.sourceApi);
                    insertSQL.Parameters.AddWithValue("@apiParams", data.apiParams);
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
