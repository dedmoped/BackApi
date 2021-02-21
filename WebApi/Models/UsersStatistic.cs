using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UsersStatistic
    {
         public int userId{ get; set; }
         public string userEmail{ get; set; }
         public int countTask{ get; set; }
    }
}
