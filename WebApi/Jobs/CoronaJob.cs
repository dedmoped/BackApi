using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Repository;
using WebApi.Services;

namespace WebApi.Jobs
{
    public class CoronaJob : IJob
    {
        private readonly IEmailService _emailService;
        private readonly ITaskRepository _taskRepository;
        private readonly IServiceScopeFactory serviceScopeFactory;
        public CoronaJob(IEmailService emailService,ITaskRepository taskRepository,IServiceScopeFactory serviceScopeFactory )
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
            using (var scope = serviceScopeFactory.CreateScope())
            {
                Apis.CoronaApi.GetData(param,id, scope);
                _taskRepository.UpdateDate(id.ToString());
                await _emailService.Send(email,"Corona-Info"+id.ToString(),scope);
            }
            File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\Corona-Info" + id.ToString() + ".csv");
        }

    }
}
