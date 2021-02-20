using Quartz;
using Quartz.Simpl;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Jobs
{
    public class AspnetCoreJobFactory:SimpleJobFactory
    {
        IServiceProvider _provider;
        public AspnetCoreJobFactory(IServiceProvider provider)
        {
            _provider = provider;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                return (IJob)this._provider.GetService(bundle.JobDetail.JobType);
            }
            catch(Exception ex)
            {
                throw new SchedulerException(string.Format("Problem while instantiating job '{0}' from IOC", bundle.JobDetail.Key));
            }
        }
    }
}
