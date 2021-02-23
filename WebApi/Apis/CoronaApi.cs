using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
        private static IEmailService _emailService;
        const string totals = "totals";
        public CoronaApi(IEmailService emailService)
        {
            _emailService = emailService;
        }
        public static void GetData( string parameters,string id, IServiceScope scope)
        {
            IConfiguration _configuration = scope.ServiceProvider.GetService<IConfiguration>();
            ILogger _logger = scope.ServiceProvider.GetService<ILogger<CoronaApi>>();
            try
            {
                _logger.LogInformation("Get CoronaData " + parameters);
                RestClient client;
                if (parameters == totals)
                {
                    client = new RestClient(_configuration["Url:CoronaUrlTotals"]);
                }
                else
                {
                    client = new RestClient(_configuration["Url:CoronaUrlCountry"] + parameters);
                }
                var request = new RestRequest(Method.GET);
                request.AddHeader("x-rapidapi-key", _configuration["Api:Key"]);
                request.AddHeader("x-rapidapi-host", _configuration["Api:CoronaHost"]);
                IRestResponse response = client.Execute(request);
                List<CovidClass> covidClasses = ParseApiData.JsonStringToCSV<List<CovidClass>>(response.Content);
                CsvWork.WriteCSV(covidClasses, AppDomain.CurrentDomain.BaseDirectory + @"\Corona-Info" + id + ".csv");
                //  Directory.Delete(@"\Corona-Info.csv");
                _logger.LogInformation("Finish get CoronaData " +parameters);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
