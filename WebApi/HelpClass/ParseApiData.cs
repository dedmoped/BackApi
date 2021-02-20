using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.HelpClass
{
    public class ParseApiData
    {
        public static T jsonStringToCSV<T>(string data)
        {
            var dataTable = JsonConvert.DeserializeObject<T>(data);
            return dataTable;
        }
    }
}
