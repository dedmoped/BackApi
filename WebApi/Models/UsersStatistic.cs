using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class UsersStatistic
    {
         public int UserId{ get; set; }
         public string UserEmail{ get; set; }
         public int CountTask{ get; set; }
    }
}
