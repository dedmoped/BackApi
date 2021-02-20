using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.HelpClass;
using WebApi.Models;

namespace WebApi.Apis
{
    public class CoronaApi
    {
        public static void getData( string parameters)
        {
            var client = new RestClient("https://covid-19-data.p.rapidapi.com/country?name=" + parameters);
            if (parameters==null)
            client = new RestClient("https://covid-19-data.p.rapidapi.com/totals");
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-key", "8d213fc82fmsh2bece5fd797525ap134cd7jsn60eaf55ca93a");
            request.AddHeader("x-rapidapi-host", "covid-19-data.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            List<CovidClass> covidClasses = ParseApiData.jsonStringToCSV<List<CovidClass>>(response.Content);
            CsvWork.WriteCSV(covidClasses, Directory.GetCurrentDirectory() + @"\data.csv");
        }
    }
}
