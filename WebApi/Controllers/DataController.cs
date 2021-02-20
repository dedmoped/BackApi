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

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        IScheduler _scheduler;
        public DataController(IScheduler scheduler)
        {
            this._scheduler = scheduler;
        }

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
            List<Data> tasks = SqliteHelper.GetTasks();

            return Ok(tasks);
        }
        [HttpPost]
        [Route("adduser")]
        public void addUser([FromBody] User user)
        {
            SqliteHelper.AddUser(user);
        }

        [HttpPost]
        [Route("task")]
        public IActionResult addTask([FromForm] string data)
        {
            Data formdata = JsonConvert.DeserializeObject<Data>(data);
           // SqliteHelper.SetUserdata(formdata);
            return Ok();
        }
        [HttpGet]
        [Route("covid/{country}")]
        public void covid(string country)
        { 
            CoronaApi.getData(country); 
        }

        [HttpDelete]
        [Route("task/{id}")]
          public void delete(string id)
        {
            SqliteHelper.deleteTask(id);
            _scheduler.UnscheduleJob(new TriggerKey(id));
        }

        [HttpPost]
        [Route("job")]
        public async Task<IActionResult> StartJob([FromForm] string data)
        {
            try
            {
                Data formdata = JsonConvert.DeserializeObject<Data>(data);
                string jobid = SqliteHelper.Userdata(formdata);
                DateTime date = DateTime.Parse(formdata.startTime);
                if (!_scheduler.IsStarted)
                {
                    await _scheduler.Start();
                }

                TriggerBuilder trigger = TriggerBuilder.Create().ForJob(new JobKey("SimpleJob")).WithIdentity(jobid).StartAt(date).WithSimpleSchedule(x=>x.WithIntervalInMinutes(Convert.ToInt32(formdata.CronTime)).RepeatForever());
                ITrigger trigger1 = trigger.UsingJobData("info",jobid).Build();
                var job=_scheduler.GetJobDetail(new JobKey("SimpleJob")).Result;
              
                if (job == null) {
                    job = JobBuilder.Create<SimpleJob>().WithIdentity("SimpleJob").StoreDurably().Build();
                    await _scheduler.ScheduleJob(job,trigger1);
                    await _scheduler.AddJob(job, true);
                }
                else
                {
                    await _scheduler.ScheduleJob(trigger1);
                }
                return Ok();
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return StatusCode(500);
            }
        }
        [HttpGet]
        [Route("Actors")]
        public void Actors()
        {
            var client = new RestClient("https://imdb8.p.rapidapi.com/actors/get-all-news?nconst=nm0001667");
           // var client = new RestClient("https://imdb8.p.rapidapi.com/actors/list-born-today?month=7&day=27");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-key", "8d213fc82fmsh2bece5fd797525ap134cd7jsn60eaf55ca93a");
            request.AddHeader("x-rapidapi-host", "imdb8.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            ActorsClass actorsClass = CsvWork.jsonStringToCSV<ActorsClass>(response.Content);
            CsvWork.WriteCSV(actorsClass.items, Directory.GetCurrentDirectory() + @"\data.csv");
            EmailClass.email_send("fdf");
        }
        [HttpGet]
        [Route("news")]
       public string news()
        {
            var client = new RestClient("https://bloomberg-market-and-financial-news.p.rapidapi.com/stories/detail?internalID=QON8TWT1UM0Z01");
           // var client = new RestClient("https://bloomberg-market-and-financial-news.p.rapidapi.com/stories/list?template=CURRENCY&id=usdjpy");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-key", "8d213fc82fmsh2bece5fd797525ap134cd7jsn60eaf55ca93a");
            request.AddHeader("x-rapidapi-host", "bloomberg-market-and-financial-news.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            Details actorsClass = CsvWork.jsonStringToCSV<Details>(response.Content);
            CsvWork.WriteCSV(new List<Details>() { actorsClass }, Directory.GetCurrentDirectory() + @"\data.csv");
            EmailClass.email_send("fsdf");
            return response.Content;
        }
        
    }
}
