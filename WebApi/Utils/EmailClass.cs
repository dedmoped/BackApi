using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApi.Utils
{
    public class EmailClass
    {
        private  IConfiguration configuration;
        public EmailClass(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public  void EmailSend(string emai,string info)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(configuration["Email:FromEmail"]);
                mail.To.Add(emai);
                mail.Subject = info;

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(Directory.GetCurrentDirectory() + @"\"+info+".csv");
                mail.Attachments.Add(attachment);

                using (SmtpClient smtp = new SmtpClient(configuration["Email:SmtpHost"], Convert.ToInt32(configuration["Email:SmtpPort"])))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(configuration["Email:FromEmail"], configuration["Email:Password"]);
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

        }
    }
}
