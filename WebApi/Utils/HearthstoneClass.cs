using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Utils
{
    public class HearthstoneClass
    {
        public List<HsClasses> Classic { get; set; }
    }

    public class HsClasses
    { 
            public string cardId { get; set; }
            public string dbfId { get; set; }
            public string name { get; set; }
            public string cardSet { get; set; }
            public string type { get; set; }
            public string health { get; set; }
            public string playerClass { get; set; }
    }
}
