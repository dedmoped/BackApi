using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;

namespace WebApi.Repository
{
    public interface ITaskRepository
    {
        IEnumerable<Data> GetTask(string id);
        string Create(Data item,string id);
        void Update(Data item);
        void Delete(string id);
        void UpdateDate(string taskid);

    }
}
