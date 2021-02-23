using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services;

namespace WebApi.Jobs
{
    public class ActorsJob : IJob
    {
        private readonly IEmailService _emailService;
        private readonly ITaskRepository _taskRepository;
        private readonly IServiceScopeFactory serviceScopeFactory;
        public ActorsJob(IEmailService emailService, ITaskRepository taskRepository, IServiceScopeFactory serviceScopeFactory)
        {
            _emailService = emailService;
            _taskRepository = taskRepository;
            this.serviceScopeFactory = serviceScopeFactory;
        }
        public async Task Execute(IJobExecutionContext context)
        {
           
                JobDataMap dataMap = context.Trigger.JobDataMap;
                string param = dataMap.GetString("params");
                string email = dataMap.GetString("email");
                string id = dataMap.GetString("id");
                var iconfig = (IConfiguration)dataMap.Get("Configuration");

                using (var scope = serviceScopeFactory.CreateScope())
                {
                    Apis.ActorsApi.GetData(param, id, scope);
                    _taskRepository.UpdateDate(id.ToString());
                    await _emailService.Send(email, "Actors-info" + id.ToString(), scope);
                }
                File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\Actors-Info" + id.ToString() + ".csv");


        }


    }
}
