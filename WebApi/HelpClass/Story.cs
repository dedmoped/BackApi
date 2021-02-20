using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class Story
    {
        public string resourceType { get; set; }
        public string card { get; set; }
        public string title { get; set; }
        public int published { get; set; }
        public string internalID { get; set; }
        public string thumbnailImage { get; set; }
        public string primarySite { get; set; }
        public string shortURL { get; set; }
        public string longURL { get; set; }
        public string label { get; set; }
    }

    public class Root
    {
        public List<Story> stories { get; set; }
        public string title { get; set; }
    }
    public class Details
    {
        public string id { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public string internalID { get; set; }
        public string byline { get; set; }
        public bool archived { get; set; }
        public string longURL { get; set; }
        public string shortURL { get; set; }
        public string authoredRegion { get; set; }
        public string primarySite { get; set; }
        public string brand { get; set; }
        public string primaryCategory { get; set; }
        public string attributor { get; set; }
        public int published { get; set; }
        public int updatedAt { get; set; }
        public string resourceType { get; set; }
        public string resourceId { get; set; }
        public int wordCount { get; set; }
        public string type { get; set; }
        public string card { get; set; }
        public bool isMetered { get; set; }
        public bool disableAds { get; set; }
    }

}
