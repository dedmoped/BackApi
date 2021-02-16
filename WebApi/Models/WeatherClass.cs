﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class WeatherClass
    {
        public int confirmed { get; set; }
        public int recovered { get; set; }
        public int critical { get; set; }
        public int deaths { get; set; }
        public string lastChange { get; set; }
        public string lastUpdate { get; set; }

    }
}
