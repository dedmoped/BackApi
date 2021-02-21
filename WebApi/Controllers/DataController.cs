using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebApi.Models;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using Newtonsoft.Json;
using System.Data;
using WebApi.HelpClass;
using WebApi.Apis;
using Quartz;
using WebApi.Jobs;
using System.Threading.Tasks;
using System;
using System.Diagnostics;
using Quartz.Impl.Matchers;
using System.Security.Claims;
using System.Linq;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private string UserId => User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
        private string Email => User.Claims.Single(c => c.Type == ClaimTypes.Email).Value;
        [HttpGet]
        public IEnumerable<User> GetData()
        {

            List<User> list = SqliteHelper.GetData();

            return list;
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        [Route("task")]
        public IActionResult GetTasks()
        {
            List<Data> tasks = SqliteHelper.GetTasks(UserId);

            return Ok(tasks);
        }

        [HttpPost]
        [Authorize(Roles ="user")]
        [Route("update")]
        public IActionResult UpdateTasks([FromBody] Data data)
        {
           SqliteHelper.UpdateTask(data);
            data.startTime = DateTime.Now.ToString();
           JobScheduler.UpdateJob(data,Email);
            return Ok();
        }

        [HttpDelete]
        [Route("task/{id}")]
          public void delete(string id)
        {
            SqliteHelper.deleteTask(id);
            JobScheduler.Deletejob(id);
        }

        [HttpGet]
        [Authorize(Roles="admin")]
        [Route("statistic")]
        public IActionResult UsersStatisctic()
        {
            return Ok(SqliteHelper.GetUsersStatistic());
        }

        [HttpPost]
        [Authorize(Roles="user")]
        [Route("job")]
        public async Task<IActionResult> StartJob([FromForm] string data)
        {
            try
            {
               // HearthstoneApi.getData("cards");
                Data formdata = JsonConvert.DeserializeObject<Data>(data);
                if (formdata.startTime == "")
                {
                    formdata.startTime = DateTime.Now.ToString();
                    formdata.lastGetDataTime = DateTime.Now.ToString();
                }
                string jobid = SqliteHelper.Userdata(formdata,UserId);
               // DateTime date = DateTime.Parse(formdata.startTime);
                JobScheduler.AddTaskTriggerForJob(formdata,jobid,Email);
                return Ok();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }
        
    }
}
