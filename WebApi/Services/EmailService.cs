using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApi.Utils;

namespace WebApi.Services
{
    public interface IEmailService
    {
        Task Send(string email,string info,IServiceScope scope); 
    }
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
      

        public async Task Send(string email, string info,IServiceScope scope)
        {
           await EmailClass.EmailSend(email,info,scope);
        }
    }
}
