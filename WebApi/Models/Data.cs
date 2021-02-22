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
        public string LastGetDataTime { get; set; }
        public string StartTime { get; set; }
        public string CronTime { get; set; }
        public string SourceApi { get; set; }
        public string ApiParams { get; set; }
        public int UserId { get; set; }

    }
}
