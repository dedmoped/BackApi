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

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<User> GetData()
        {

            List<User> list = SqliteHelper.GetData();

            return list;
        }

        [HttpGet]
        [Authorize(Roles ="user")]
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
            SqliteHelper.SetUserdata(formdata);
            return Ok();
        }
        [HttpGet]
        [Route("football")]
        public void foot()
        {
            var client = new RestClient("https://covid-19-data.p.rapidapi.com/totals?format=json");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-key", "8d213fc82fmsh2bece5fd797525ap134cd7jsn60eaf55ca93a");
            request.AddHeader("x-rapidapi-host", "covid-19-data.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            CsvWork.email_send();
          //  CsvWork.jsonStringToCSV(response.Content);
        }

       
        
    }
}
