using System.Collections.Generic;
using Newtonsoft.Json;

namespace BoligScraper
{
    public class BoligPortalRequest
    {
        [JsonProperty(PropertyName = "amtId")]
        public string Amt { get; set; }
        [JsonProperty(PropertyName = "huslejeMin")]
        public string RentMin { get; set; }
        [JsonProperty(PropertyName = "huslejeMax")]
        public string RentMax { get; set; }
        [JsonProperty(PropertyName = "stoerrelseMin")]
        public string SizeMin { get; set; }
        [JsonProperty(PropertyName = "stoerrelseMax")]
        public string SizeMax { get; set; }
        [JsonProperty(PropertyName = "postnrArr")]
        public IList<int> ZipCodes { get; set; }
        [JsonProperty(PropertyName = "boligTypeArr")]
        public IList<string> ApartmentType { get; set; }
        [JsonProperty(PropertyName = "lejeLaengdeArr")]
        public IList<string> RentLength { get; set; }
        [JsonProperty(PropertyName = "page")]
        public string Page { get; set; }
        [JsonProperty(PropertyName = "limit")]
        public string Limit { get; set; }
        [JsonProperty(PropertyName = "sortCol")]
        public string SortCol { get; set; }
        [JsonProperty(PropertyName = "sortDesc")]
        public string SortDesc { get; set; }
        [JsonProperty(PropertyName = "visOnSiteBolig")]
        public int ShowOnSiteApartment { get; set; }
        [JsonProperty(PropertyName = "almen")]
        public int Default { get; set; }
        [JsonProperty(PropertyName = "billeder")]
        public int Pictures { get; set; }
        [JsonProperty(PropertyName = "husdyr")]
        public int HousePets { get; set; }
        [JsonProperty(PropertyName = "mobleret")]
        public int Furnished { get; set; }
        [JsonProperty(PropertyName = "delevenlig")]
        public int Shareable { get; set; }
        [JsonProperty(PropertyName = "fritekst")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "overtagdato")]
        public string TakeOverDate { get; set; } // dafuq
        [JsonProperty(PropertyName = "emailservice")]
        public string EmailService { get; set; }
        [JsonProperty(PropertyName = "kunNyeste")]
        public bool OnlyLatest { get; set; }
        [JsonProperty(PropertyName = "muListeMuId")]
        public string MuListMuId { get; set; }
        [JsonProperty(PropertyName = "fremleje")]
        public int Sublease { get; set; }
    }
}