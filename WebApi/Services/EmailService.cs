using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApi.HelpClass;

namespace WebApi.Services
{
    public interface IEmailService
    {
        void Send(string receiver, string subject, string body);
    }
    public class EmailService:IEmailService
    {
      

        public void Send(string receiver, string subject, string body)
        {
            EmailClass.email_send(receiver);
        }
    }
}
