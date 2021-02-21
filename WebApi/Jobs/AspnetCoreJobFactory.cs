using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Jobs
{
    public class AspnetCoreJobFactory:IJobFactory
    {

            protected readonly IServiceScopeFactory serviceScopeFactory;

            public AspnetCoreJobFactory(IServiceScopeFactory serviceScopeFactory)
            {
                this.serviceScopeFactory = serviceScopeFactory;
            }

            public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
            {
                using (var scope = serviceScopeFactory.CreateScope())
                {
                    var job = scope.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
                    return job;
                }

            }

        public void ReturnJob(IJob job)
        {
           
        }
    }
}
