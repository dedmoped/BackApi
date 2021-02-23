using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface IUserRepository
    {
        bool Create(User user);
        IEnumerable<User> FindUser(string login);
        IEnumerable<UsersStatistic> GetStatistic();
    }
}
