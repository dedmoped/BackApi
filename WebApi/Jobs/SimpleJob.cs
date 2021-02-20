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
    public class SimpleJob : IJob
    {
        IEmailService _emailService;
        public SimpleJob(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.MergedJobDataMap;
             string id = dataMap.GetString("info");
             Apis.CoronaApi.getData("Italy");
           //  SqliteHelper.setLastTime(id.ToString());
            _emailService.Send(id,"9999","0000");
        }

    }
}
