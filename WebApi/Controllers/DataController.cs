using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using Newtonsoft.Json;
using WebApi.Jobs;
using System.Threading.Tasks;
using System;
using System.Security.Claims;
using System.Linq;
using WebApi.Repository;
using Microsoft.Extensions.Logging;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private string UserId => User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;
        private string Email => User.Claims.Single(c => c.Type == ClaimTypes.Email).Value;
        private readonly ITaskRepository _taskRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<DataController> _logger;
        public DataController(ITaskRepository taskRepository,IUserRepository userRepository,ILogger<DataController> logger)
        {
            _taskRepository = taskRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "user")]
        [Route("task")]
        public IActionResult GetTasks()
        {
            _logger.LogInformation("Start GetTasks");
            return Ok(_taskRepository.GetTask(UserId));
        }

        [HttpPost]
        [Authorize(Roles ="user")]
        [Route("update")]
        public IActionResult UpdateTasks([FromBody] Data data)
        {
            _logger.LogInformation("UpdateTasks");
            _taskRepository.Update(data);
            data.StartTime = DateTime.Now.ToString();
            JobScheduler.UpdateJob(data,Email);
            _logger.LogInformation("Tasks end update");
            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = "user")]
        [Route("task/{id}")]
          public void delete(string id)
        {
            _logger.LogInformation("Delete task and user");
            _taskRepository.Delete(id);
            JobScheduler.Deletejob(id.ToString());
        }

        [HttpGet]
        [Authorize(Roles="admin")]
        [Route("statistic")]
        public IActionResult UsersStatisctic()
        {
            _logger.LogInformation("GetStatistic");
            return Ok(_userRepository.GetStatistic());
        }

        [HttpPost]
        [Authorize(Roles="user")]
        [Route("job")]
        public async Task<IActionResult> StartJob([FromForm] string data)
        {
            try
            {
                _logger.LogInformation("Start crete Task");
                Data FormData = JsonConvert.DeserializeObject<Data>(data);
                if (FormData.StartTime == "")
                {
                    FormData.StartTime = DateTime.Now.ToString();
                    FormData.LastGetDataTime = DateTime.Now.ToString();
                }
                if (FormData.SourceApi == "")
                {
                    _logger.LogError("Choose Api");
                    return StatusCode(500);
                }
                string taskid = _taskRepository.Create(FormData, UserId);
                if (taskid != null)
                {
                    JobScheduler.AddTaskTriggerForJob(FormData, taskid, Email);
                }
                _logger.LogInformation("Task created");
                return Ok();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
           
        }
        
    }
}
