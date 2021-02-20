using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Data
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string lastGetDataTime { get; set; }
        public string startTime { get; set; }
        public string CronTime { get; set; }
        public string sourceApi { get; set; }
        public int userId { get; set; }

    }
}
