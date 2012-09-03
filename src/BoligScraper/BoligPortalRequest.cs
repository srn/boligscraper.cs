using System.Collections.Generic;
using Newtonsoft.Json;

namespace BoligScraper
{
    public class BoligPortalRequest
    {
        [JsonProperty(PropertyName = "amtId")]
        public AmtEnum Amt { get; set; }
        [JsonProperty(PropertyName = "huslejeMin")]
        public string RentMin { get; set; }
        [JsonProperty(PropertyName = "huslejeMax")]
        public string RentMax { get; set; }
        [JsonProperty(PropertyName = "stoerrelseMin")]
        public string SizeMin { get; set; }
        [JsonProperty(PropertyName = "stoerrelseMax")]
        public string SizeMax { get; set; }
        [JsonProperty(PropertyName = "postnrArr")]
        public List<int> ZipCodes { get; set; }
        [JsonProperty(PropertyName = "boligTypeArr")]
        public List<string> ApartmentType { get; set; }
        [JsonProperty(PropertyName = "lejeLaengdeArr")]
        public List<string> RentLength { get; set; }
        public string Page { get; set; }
        public string Limit { get; set; }
        public string SortCol { get; set; }
        public string SortDesc { get; set; }
        [JsonProperty(PropertyName = "visOnSiteBolig")]
        public int ShowOnSiteApartment { get; set; }
    }
}