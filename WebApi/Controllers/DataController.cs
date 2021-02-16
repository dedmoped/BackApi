using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApi.Models;

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
        public void addTask([FromForm] string data)
        {
            Data formdata = JsonConvert.DeserializeObject<Data>(data);
            SqliteHelper.SetUserdata(formdata);
        }
    }
}
