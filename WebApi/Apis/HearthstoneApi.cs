using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using WebApi.Models;
using WebApi.Services;
using WebApi.Utils;

namespace WebApi.Apis
{
    public class HearthstoneApi
    {
       
        const string basicards = "Basic-Cards";
        public static void GetData(string parameters, string id, IServiceScope scope)
        {
            IConfiguration _configuration = scope.ServiceProvider.GetService<IConfiguration>();
            ILogger _logger = scope.ServiceProvider.GetService<ILogger<HearthstoneApi>>();
            try
            {
                _logger.LogInformation("Start get HearthstoneData " + parameters);
                RestClient client;
                if (parameters == basicards)
                    client = new RestClient(_configuration["Url:HearthstoneUrlBasicCards"]);
                else
                    client = new RestClient(_configuration["Url:HearthstoneUrlClassCards"] + parameters);
                var request = new RestRequest(Method.GET);
                request.AddHeader("x-rapidapi-key", _configuration["Api:Key"]);
                request.AddHeader("x-rapidapi-host", _configuration["Api:HearthstoneHost"]);
                IRestResponse response = client.Execute(request);
                if (parameters == "Basic-Cards")
                {
                    HearthstoneClass covidClasses = ParseApiData.JsonStringToCSV<HearthstoneClass>(response.Content);
                    CsvWork.WriteCSV(covidClasses.Classic, AppDomain.CurrentDomain.BaseDirectory + @"\Hearthstone-Info"+id+".csv");
                }
                else
                {

                    List<HsClasses> covidClasses = ParseApiData.JsonStringToCSV<List<HsClasses>>(response.Content);
                    CsvWork.WriteCSV(covidClasses, AppDomain.CurrentDomain.BaseDirectory + @"\Hearthstone-Info" + id + ".csv");
                }
                _logger.LogInformation("Finish get HearthstoneData " + parameters);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
