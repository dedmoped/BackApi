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
        void Send(string email,string info);
    }
    public class EmailService:IEmailService
    {
      

        public void Send(string email, string info)
        {
            EmailClass.email_send(email,info);
        }
    }
}
