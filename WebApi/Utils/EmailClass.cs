using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
       
        public static async Task  EmailSend(string emai,string info,IServiceScope scope)
        {
            IConfiguration _configuration = scope.ServiceProvider.GetService<IConfiguration>();
            ILogger<EmailClass> _logger = scope.ServiceProvider.GetService<ILogger<EmailClass>>();
            try
            {
                
                _logger.LogInformation("Start Send email with info" + info);
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(_configuration["Email:FromEmail"]);
                    mail.To.Add(emai);
                    mail.Subject = info;

                    System.Net.Mail.Attachment attachment;
                    attachment = new System.Net.Mail.Attachment(AppDomain.CurrentDomain.BaseDirectory + @"\" + info + ".csv");
                    mail.Attachments.Add(attachment);

                    using (SmtpClient smtp = new SmtpClient(_configuration["Email:SmtpHost"], Convert.ToInt32(_configuration["Email:SmtpPort"])))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential(_configuration["Email:FromEmail"], _configuration["Email:Password"]);
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                    }
                }
                _logger.LogInformation("End Send email with info" + info);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message, "Send Email Fail");
            }
        }
    }
}
