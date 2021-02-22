using Microsoft.Extensions.Configuration;
using WebApi.Utils;

namespace WebApi.Services
{
    public interface IEmailService
    {
        void Send(string email,string info);
    }
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
      

        public void Send(string email, string info)
        {
            EmailClass SendEmail = new EmailClass(_configuration);
            SendEmail.EmailSend(email,info);
        }
    }
}
