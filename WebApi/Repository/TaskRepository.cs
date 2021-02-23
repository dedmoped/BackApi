using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;


namespace WebApi.Repository
{
    public class TaskRepository : AbstractRepository<TaskRepository>,ITaskRepository
    {
 
        public TaskRepository(IConfiguration configuration,ILogger<TaskRepository> logger):base(configuration,logger)
        {

        }
        public string Create(Data data,string userId)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    logger.LogInformation("Create Task");
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "Insert into UserTask(Name,Description,LastGetDataTime,CronTime,sourceApi,userId,apiParams) values(@Name,@Description,@LastGetDataTime,@CronTime,@sourceApi,@userId,@apiParams);select last_insert_rowid()";
                    insertSQL.Parameters.AddWithValue("@Name", data.Name);
                    insertSQL.Parameters.AddWithValue("@Description", data.Description);
                    insertSQL.Parameters.AddWithValue("@LastGetDataTime", data.LastGetDataTime);
                    insertSQL.Parameters.AddWithValue("@CronTime", data.CronTime);
                    insertSQL.Parameters.AddWithValue("@sourceApi", data.SourceApi);
                    insertSQL.Parameters.AddWithValue("@userId", userId);
                    insertSQL.Parameters.AddWithValue("@apiParams", data.ApiParams);

                    using (var rdr = insertSQL.ExecuteReader())
                    {
                        string id = "";
                        while (rdr.Read())
                        {

                            id = rdr.GetInt32(0).ToString();

                        }
                        logger.LogInformation("Task created successful");
                        return id;
                    }

                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message,"Task create Error");
                return null;
            }
        }

        public void Delete(string id)
        {
            using (var connection = new SQLiteConnection(connectionstring))
            {
                connection.Open();
                SQLiteCommand insertSQL = new SQLiteCommand(connection);
                insertSQL.CommandText = "delete from UserTask  where id=@id";
                insertSQL.Parameters.AddWithValue("@id", id);
                insertSQL.ExecuteNonQuery();

            }
        }

        public IEnumerable<Data> GetTask(string userId)
        {
            try
            {
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    logger.LogInformation("GetTask Start");
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "Select Id,Name,Description,LastGetDataTime,SourceApi,CronTime,UserId,ApiParams from UserTask where userId=@id";
                    insertSQL.Parameters.AddWithValue("@id", userId);
                    using (var rdr = insertSQL.ExecuteReader())
                    {
                        List<Data> users = new List<Data>();
                        while (rdr.Read())
                        {
                            users.Add(new Data()
                            {
                                Id = rdr.GetInt32(0),
                                Name = rdr.GetString(1),
                                Description = rdr.GetString(2),
                                LastGetDataTime = rdr.GetString(3),
                                SourceApi = rdr.GetString(5),
                                CronTime = rdr.GetString(4),
                                UserId = rdr.GetInt32(6),
                                ApiParams = rdr.GetString(7)
                            });
                        }

                        logger.LogInformation("GetTask completed");
                        return users;
                    }

                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message,"GetTask faild");
                return  new List<Data>();
            }
        }

        public void Update(Data data)
        {
            try
            {
                logger.LogInformation("UpdateTask Start");
                using (var connection = new SQLiteConnection(connectionstring))
                {
                   
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = "update  UserTask SET id = @id,Name = @Name,Description =@Description,CronTime =@CronTime,sourceApi = @sourceApi,apiParams = @apiParams WHERE id =@id";
                    insertSQL.Parameters.AddWithValue("@id", data.Id);
                    insertSQL.Parameters.AddWithValue("@Name", data.Name);
                    insertSQL.Parameters.AddWithValue("@Description", data.Description);
                    insertSQL.Parameters.AddWithValue("@CronTime", data.CronTime);
                    insertSQL.Parameters.AddWithValue("@sourceApi", data.SourceApi);
                    insertSQL.Parameters.AddWithValue("@apiParams", data.ApiParams);
                    insertSQL.ExecuteNonQuery();
                    

                }
                logger.LogInformation("UpdateTask completed");
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, "Update faild");
            }
        }

        public void UpdateDate(string taskid)
        {
            try
            {
                logger.LogInformation("UpdateDate Start");
                using (var connection = new SQLiteConnection(connectionstring))
                {
                    connection.Open();
                    SQLiteCommand insertSQL = new SQLiteCommand(connection);
                    insertSQL.CommandText = $"update UserTask  set LastGetDataTime=@time where id=@taskid";
                    insertSQL.Parameters.AddWithValue("@time", DateTime.Now.ToString());
                    insertSQL.Parameters.AddWithValue("@taskid", taskid);
                    insertSQL.ExecuteNonQuery();
                }
                logger.LogInformation("UpdateDate completed");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message, "UpdateDate faild");
            }
        }
    }
}
