﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace WebApi.HelpClass
{
    public class EmailClass
    {
        public static void email_send(string id)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("vilbicos2000@gmail.com");
                mail.To.Add("Vilbicos2000@gmail.com");
                mail.Subject = "Test Mail - 1";
                mail.Body = id;

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(Directory.GetCurrentDirectory() + @"\data.csv");
                mail.Attachments.Add(attachment);

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new System.Net.NetworkCredential("testmaster621@gmail.com", "Passw0rd1328");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

        }
    }
}