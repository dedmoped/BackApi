using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Utils
{
    public class ParseApiData
    {
        public static T JsonStringToCSV<T>(string data)
        {
            var dataTable = JsonConvert.DeserializeObject<T>(data);
            return dataTable;
        }
    }
}
