using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Utils;
using WebApi.Models;
using WebApi.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace WebApi.Apis
{
    public class ActorsApi
    {
        
        const string allnews = "get-all-news";
        public static void GetData(string parameters,string id, IServiceScope scope)
        {
            IConfiguration _configuration = scope.ServiceProvider.GetService<IConfiguration>();
            ILogger _logger = scope.ServiceProvider.GetService<ILogger<ActorsApi>>();
            try
            {
                _logger.LogInformation("Start Get " + parameters);
                RestClient client;
                if (parameters == allnews)
                {
                    client = new RestClient(_configuration["Url:ActrosUrlAllNews"]);
                }
                else
                {
                    client = new RestClient(_configuration["Url:ActrosUrlTitleNews"]);
                }
                var request = new RestRequest(Method.GET);
                request.AddHeader("x-rapidapi-key", _configuration["Api:Key"]);
                request.AddHeader("x-rapidapi-host", _configuration["Api:ActorsHost"]);
                IRestResponse response = client.Execute(request);
                ActorsClass actorsClass = ParseApiData.JsonStringToCSV<ActorsClass>(response.Content);
                CsvWork.WriteCSV(actorsClass.items, AppDomain.CurrentDomain.BaseDirectory + @"\Actors-Info"+ id + ".csv");
                _logger.LogInformation("End Get " + parameters);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
            }

        }

    }
}
