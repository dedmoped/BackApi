using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services;

namespace WebApi.Jobs
{
    public class HearthstoneJob:IJob
    {
        private readonly IEmailService _emailService;
        private readonly ITaskRepository _taskRepository;
        private readonly IServiceScopeFactory serviceScopeFactory;
        public HearthstoneJob(IEmailService emailService, ITaskRepository taskRepository, IServiceScopeFactory scope)
        {
            _emailService = emailService;
            _taskRepository = taskRepository;
            serviceScopeFactory = scope;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.Trigger.JobDataMap;
            string param = dataMap.GetString("params");
            string email = dataMap.GetString("email");
            var iconfig = (IConfiguration)dataMap.Get("Configuration");
          
            string id = dataMap.GetString("id");
            using (var scope = serviceScopeFactory.CreateScope())
            {
                Apis.HearthstoneApi.GetData(param,id,scope);
                _taskRepository.UpdateDate(id.ToString());
                await _emailService.Send(email,"Hearthstone-Info"+ id.ToString(), scope);
            }
            File.Delete(AppDomain.CurrentDomain.BaseDirectory +@"\Hearthstone-Info" + id.ToString() + ".csv");
        }
    }
}
