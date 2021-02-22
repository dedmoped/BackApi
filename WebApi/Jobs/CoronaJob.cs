using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public CoronaJob(IEmailService emailService,ITaskRepository taskRepository)
        {
            _emailService = emailService;
            _taskRepository = taskRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.Trigger.JobDataMap;
             string param = dataMap.GetString("params");
            string email = dataMap.GetString("email");
            string id = dataMap.GetString("id");
            Apis.CoronaApi.GetData(param,email,_emailService);
            _taskRepository.UpdateDate(id.ToString());

        }

    }
}
