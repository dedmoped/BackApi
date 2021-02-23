using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Repository
{
    public class AbstractRepository<T>
    {
        protected readonly string connectionstring;
        protected ILogger<T> logger;
        public AbstractRepository(IConfiguration configuration, ILogger<T> logger)
        {
            connectionstring = configuration["Database:ConnectionString"];
            this.logger = logger;
        }
    }
}
