using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Utils;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Apis
{
    public class ActorsApi
    {
        const string allnews = "get-all-news";
        public static void GetData(string parameters,string email,IEmailService service)
        {
            RestClient client;
            if (parameters == allnews)
            {
                client = new RestClient("https://imdb8.p.rapidapi.com/actors/get-all-news?nconst=nm0001667");
            }
            else
            {
                 client = new RestClient("https://imdb8.p.rapidapi.com/title/get-news?tconst=tt0944947&limit=25");
            }
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-key", "8d213fc82fmsh2bece5fd797525ap134cd7jsn60eaf55ca93a");
            request.AddHeader("x-rapidapi-host", "imdb8.p.rapidapi.com");
            IRestResponse response = client.Execute(request);
            ActorsClass actorsClass = ParseApiData.JsonStringToCSV<ActorsClass>(response.Content);
            CsvWork.WriteCSV(actorsClass.items, Directory.GetCurrentDirectory() + @"\Actors-Info.csv");
            service.Send(email, "Actors-Info");
          // Directory.Delete(@"\Actors-Info.csv");

        }

    }
}
