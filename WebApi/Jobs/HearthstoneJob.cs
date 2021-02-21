using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Jobs
{
    public class HearthstoneJob:IJob
    {
        static IEmailService _emailService;
        public HearthstoneJob(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap dataMap = context.Trigger.JobDataMap;
            string param = dataMap.GetString("params");
            string email = dataMap.GetString("email");
            Apis.HearthstoneApi.getData(param, email, _emailService);
            string id = dataMap.GetString("id");
            SqliteHelper.setLastTime(id.ToString());
        }
    }
}
