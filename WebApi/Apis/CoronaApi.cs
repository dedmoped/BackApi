using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;
using WebApi.Utils;

namespace WebApi.Apis
{
    public class CoronaApi
    {
        static IEmailService _emailService;
        const string allnews = "totals";
        public CoronaApi(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public static void GetData( string parameters,string email,IEmailService service)
        {
            try
            {
                RestClient client;
                if (parameters == allnews)
                {
                    client = new RestClient("https://covid-19-data.p.rapidapi.com/totals");
                }
                else
                {
                    client = new RestClient("https://covid-19-data.p.rapidapi.com/country?name=" + parameters);
                }
                var request = new RestRequest(Method.GET);
                request.AddHeader("x-rapidapi-key", "8d213fc82fmsh2bece5fd797525ap134cd7jsn60eaf55ca93a");
                request.AddHeader("x-rapidapi-host", "covid-19-data.p.rapidapi.com");
                IRestResponse response = client.Execute(request);
                List<CovidClass> covidClasses = ParseApiData.JsonStringToCSV<List<CovidClass>>(response.Content);
                CsvWork.WriteCSV(covidClasses, Directory.GetCurrentDirectory() + @"\Corona-Info.csv");
                service.Send(email, "Corona-Info");
              //  Directory.Delete(@"\Corona-Info.csv");
            }
            catch
            {

            }
        }
    }
}
