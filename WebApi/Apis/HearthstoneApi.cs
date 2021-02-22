using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using WebApi.Models;
using WebApi.Services;
using WebApi.Utils;

namespace WebApi.Apis
{
    public class HearthstoneApi
    {
        const string basicards = "Basic-Cards";
        public static void GetData(string parameters,string email,IEmailService service)
        {
            
                RestClient client;
                if (parameters == basicards)
                    client = new RestClient("https://omgvamp-hearthstone-v1.p.rapidapi.com/cards");
                else
                    client = new RestClient("https://omgvamp-hearthstone-v1.p.rapidapi.com/cards/classes/" + parameters);
                var request = new RestRequest(Method.GET);
                request.AddHeader("x-rapidapi-key", "8d213fc82fmsh2bece5fd797525ap134cd7jsn60eaf55ca93a");
                request.AddHeader("x-rapidapi-host", "omgvamp-hearthstone-v1.p.rapidapi.com");
                IRestResponse response = client.Execute(request);
            if (parameters == "Basic-Cards")
            {
                HearthstoneClass covidClasses = ParseApiData.JsonStringToCSV<HearthstoneClass>(response.Content);
                CsvWork.WriteCSV(covidClasses.Classic, Directory.GetCurrentDirectory() + @"\Hearthstone-Info.csv");
            }
            else
            {
               
                List<HsClasses> covidClasses = ParseApiData.JsonStringToCSV<List<HsClasses>>(response.Content);
                CsvWork.WriteCSV(covidClasses, Directory.GetCurrentDirectory() + @"\Hearthstone-Info.csv");
            }
            service.Send(email, "Hearthstone-Info");
        }
    }
}
