using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Utils
{
    public class ActorsClass
    {
        public List<items> items { get; set; }
    }

    public class items
    {
        public string body { get; set; }
        public string head { get; set; }
        public string id { get; set; }
        public string link { get; set; }
        public string publishDateTime { get; set; }

    }
}
