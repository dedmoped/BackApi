using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class CsvWork
    {


        public static void jsonStringToCSV(string jsonContent)
        {
            var dataTable =JsonConvert.DeserializeObject<List<WeatherClass>>(jsonContent);
            WriteCSV(dataTable, Directory.GetCurrentDirectory() + @"\data.csv");

        }
        public static void WriteCSV<T>(IEnumerable<T> items, string path)
        {
            Type itemType = typeof(T);
            var props = itemType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                .OrderBy(p => p.Name);

            using (var writer = new StreamWriter(path))
            {
                writer.WriteLine(string.Join(", ", props.Select(p => p.Name)));

                foreach (var item in items)
                {
                    writer.WriteLine(string.Join(", ", props.Select(p => p.GetValue(item, null))));
                }
            }
        }
 
        public static void email_send()
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("vilbicos2000@gmail.com");
                mail.To.Add("Vilbikos@gmail.com");
                mail.Subject = "Test Mail - 1";
                mail.Body = "<h1>mail with attachment</h1>";

                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(Directory.GetCurrentDirectory() + @"\data.csv");
                mail.Attachments.Add(attachment);

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

        }
    }

}
