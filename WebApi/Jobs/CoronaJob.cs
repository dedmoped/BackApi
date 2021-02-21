using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Jobs
{
    public class CoronaJob : IJob
    {
        IEmailService _emailService;
        public CoronaJob(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.Trigger.JobDataMap;
             string param = dataMap.GetString("params");
            string email = dataMap.GetString("email");
            string id = dataMap.GetString("id");
            Apis.CoronaApi.getData(param,email,_emailService);
            SqliteHelper.setLastTime(id.ToString());

        }

    }
}
