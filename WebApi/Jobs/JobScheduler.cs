
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
        public static async void Start(IServiceProvider serviceProvider)
        {

            StdSchedulerFactory factory = new StdSchedulerFactory(GetThreadConstraint());
            scheduler = factory.GetScheduler().Result;
            scheduler.JobFactory = serviceProvider.GetService<AspnetCoreJobFactory>();
            await scheduler.Start();

            IJobDetail covidJob = JobBuilder.Create<CoronaJob>()
                .StoreDurably()
                .WithIdentity("CoronaJob")
                .Build();
            await scheduler.AddJob(covidJob, true);
            IJobDetail actorsJob = JobBuilder.Create<ActorsJob>()
               .StoreDurably()
               .WithIdentity("ActorsJob")
               .Build();
            await scheduler.AddJob(actorsJob, true);
            IJobDetail hearthstoneJob = JobBuilder.Create<HearthstoneJob>()
               .StoreDurably()
               .WithIdentity("HearthstoneJob")
               .Build();
            await scheduler.AddJob(hearthstoneJob, true);
        }
        public static async void AddTaskTriggerForJob(Models.Data task,string TriggerId,string email)
        {
            TriggerBuilder triggerBuilder = TriggerBuilder.Create()
                        .WithIdentity(TriggerId)
                        .WithSimpleSchedule(x => x
                        .WithIntervalInMinutes(Convert.ToInt32(task.CronTime))
                        .RepeatForever());

            if (DateTime.Now >= DateTime.Parse(task.startTime))
            {
                triggerBuilder.StartNow();
            }
            else
            {
                triggerBuilder.StartAt(DateTimeOffset.Parse(task.startTime));
            }

            ITrigger trigger = null;
            if (task.sourceApi == "Corona-19")
            {
                trigger = triggerBuilder.ForJob("CoronaJob").Build();
            }
            if (task.sourceApi == "Actors")
            {
                trigger = triggerBuilder.ForJob("ActorsJob").Build();
            }
            if (task.sourceApi == "Hearthstone")
            {
                trigger = triggerBuilder.ForJob("HearthstoneJob").Build();
            }
            trigger.JobDataMap["email"] = email;
            trigger.JobDataMap["params"] = task.apiParams;
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
            SqliteHelper.deleteTask(trriggerid);
            await scheduler.UnscheduleJob(new TriggerKey(trriggerid));
        }
        //public static async void UpdateTaskTrigger(Models.Task task, User user)
        //{
        //    await scheduler.UnscheduleJob(new TriggerKey(task.id.ToString()));
        //    AddTaskTriggerForJob(task, user);
        //}
        private static NameValueCollection GetThreadConstraint() // получаем ограничение на кол-во одновременных потоков для задач
        {
            return new NameValueCollection { 
                { "quartz.threadPool.threadCount", "10" },
                {"quartz.serializer.type" ,"json" },
                {"quartz.jobStore.type","Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" },
                {"quartz.jobStore.dataSource","default"},
                {"quartz.jobStore.driverDelegateType","Quartz.Impl.AdoJobStore.SQLiteDelegate, Quartz" },
                {"quartz.dataSource.default.provider","SQLite"},
                {"quartz.dataSource.default.connectionString","Data Source=Quartz.sqlite; Version = 3;"}};
        }
    }
}
