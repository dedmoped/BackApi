
using Quartz;
using Quartz.Impl;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Collections.Specialized;
using WebApi.Models;

namespace WebApi.Jobs
{
    public class JobScheduler
    {
        static IScheduler scheduler;
        static IConfiguration _configuration;
        const string Corona = "Corona-19";
        const string Actors = "Actors";
        const string Hearthstone = "Hearthstone";
        const string CoronaJobKey = "CoronaJob";
        const string ActorsJobKey = "ActorsJob";
        const string HearthstoneJobKey = "HearthstoneJob";

        public static async void Start(IServiceProvider serviceProvider)
        {
            _configuration = serviceProvider.GetService<IConfiguration>();
            StdSchedulerFactory factory = new StdSchedulerFactory(GetThreadConstraint());
            scheduler = factory.GetScheduler().Result;
            scheduler.JobFactory = serviceProvider.GetService<AspnetCoreJobFactory>();
            await scheduler.Start();

            IJobDetail covidJob = JobBuilder.Create<CoronaJob>()
                .StoreDurably()
                .WithIdentity(CoronaJobKey)
                .Build();
            await scheduler.AddJob(covidJob, true);
            IJobDetail actorsJob = JobBuilder.Create<ActorsJob>()
               .StoreDurably()
               .WithIdentity(ActorsJobKey)
               .Build();
            await scheduler.AddJob(actorsJob, true);
            IJobDetail hearthstoneJob = JobBuilder.Create<HearthstoneJob>()
               .StoreDurably()
               .WithIdentity(HearthstoneJobKey)
               .Build();
            await scheduler.AddJob(hearthstoneJob, true);
        }
        public static async void AddTaskTriggerForJob(Models.Data task,string TriggerId,string email)
        {
            TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                        .WithIdentity(TriggerId)
                        .WithCronSchedule(task.CronTime);

            if (DateTime.Now >= DateTime.Parse(task.StartTime))
            {
                triggerBuilder.StartNow();
            }
            else
            {
                triggerBuilder.StartAt(DateTimeOffset.Parse(task.StartTime));
            }

            ITrigger trigger = null;
            switch (task.SourceApi)
            {
                case Corona:
                    trigger = triggerBuilder.ForJob(CoronaJobKey).Build();
                    break;
                case Actors:
                    trigger = triggerBuilder.ForJob(ActorsJobKey).Build();
                    break;
                case Hearthstone:
                    trigger = triggerBuilder.ForJob(HearthstoneJobKey).Build();
                    break;
            }
            trigger.JobDataMap["email"] = email;
            trigger.JobDataMap["params"] = task.ApiParams;
            trigger.JobDataMap["id"] = TriggerId;
            await scheduler.ScheduleJob(trigger);
        }

        internal static void UpdateJob(Data data,string email)
        {
            scheduler.UnscheduleJob(new TriggerKey(data.Id.ToString()));
            AddTaskTriggerForJob(data, data.Id.ToString(), email);
        }

        public static async void Deletejob(string trriggerid)
        {
            await scheduler.UnscheduleJob(new TriggerKey(trriggerid));
        }
        private static NameValueCollection GetThreadConstraint() // получаем ограничение на кол-во одновременных потоков для задач
        {
            return new NameValueCollection { 
                { "quartz.threadPool.threadCount", _configuration["QuartzParams:ThreadCount"] },
                {"quartz.serializer.type" ,_configuration["QuartzParams:SerializerType"] },
                {"quartz.jobStore.type",_configuration["QuartzParams:JobStoreType"]},
                {"quartz.jobStore.dataSource",_configuration["QuartzParams:JobStoreDataSource"]},
                {"quartz.jobStore.driverDelegateType",_configuration["QuartzParams:DriverDelegateType"]},
                {"quartz.dataSource.default.provider",_configuration["QuartzParams:DefaultProvider"]},
                {"quartz.dataSource.default.connectionString",_configuration["QuartzParams:ConnectionString"]}};
        }
    }
}
