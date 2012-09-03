using System.Collections.Generic;
using Newtonsoft.Json;
using RestSharp;

namespace BoligScraper
{
    public class Scraper
    {
        public BoligPortalResponse Scrape(BoligPortalRequest boligPortalRequest, out IRestResponse restResponse)
        {
            var client = new RestClient { BaseUrl = "http://www.boligportal.dk" };
            var request = new RestRequest(Method.POST) { Resource = "api/soeg_leje_bolig.php" };

            request.AddParameter("serviceName", "getProperties");
            request.AddBody(JsonConvert.SerializeObject(boligPortalRequest));

            restResponse = client.Execute(request);

            return JsonConvert.DeserializeObject<BoligPortalResponse>(restResponse.Content);
        }
    }

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

    public class BoligPortalResponse
    {
        public List<BoligPortalProperty> Properties { get; set; }

        public class BoligPortalProperty
        {
            [JsonProperty(PropertyName = "jqt_adId")]
            public string Id { get; set; }

            [JsonProperty(PropertyName = "jqt_creationDate")]
            public string CreationDate { get; set; }

            [JsonProperty(PropertyName = "jqt_creationDateZ")]
            public string CreationDateZ { get; set; }

            [JsonProperty(PropertyName = "jqt_creationDateF")]
            public string CreationDateF { get; set; }

            [JsonProperty(PropertyName = "jqt_headline")]
            public string Headline { get; set; }

            [JsonProperty(PropertyName = "jqt_adtext")]
            public string Description { get; set; }

            [JsonProperty(PropertyName = "jqt_adHighlight")]
            public int Highlight { get; set; }

            [JsonProperty(PropertyName = "jqt_adSaved")]
            public int Saved { get; set; }

            [JsonProperty(PropertyName = "jqt_active")]
            public bool Active { get; set; }

            [JsonProperty(PropertyName = "jqt_reserved")]
            public bool Reserved { get; set; }

            [JsonProperty(PropertyName = "jqt_udlejet")]
            public bool Rented { get; set; }

            [JsonProperty(PropertyName = "jqt_contacted")]
            public bool Contaced { get; set; }

            [JsonProperty(PropertyName = "jqt_type")]
            public BoligPortalType Type { get; set; }

            public class BoligPortalType
            {
                public string Id { get; set; }
                public string Text { get; set; }
            }

            [JsonProperty(PropertyName = "jqt_rental period")]
            public string RentalPeriod { get; set; }

            [JsonProperty(PropertyName = "jqt_adSublease")]
            public string Sublease { get; set; }

            [JsonProperty(PropertyName = "jqt_furnished")]
            public string Furnished { get; set; }

            [JsonProperty(PropertyName = "jqt_size")]
            public BoligPortalSize Size { get; set; }
            
            public class BoligPortalSize
            {
                public string m2 { get; set; }
            }

            [JsonProperty(PropertyName = "jqt_location")]
            public BoligPortalLocation Location { get; set; }

            public class BoligPortalLocation
            {
                public string ZipCode { get; set; }
                public string City { get; set; }
            }

            [JsonProperty(PropertyName = "jqt_economy")]
            public BoligPortalEconomy Economy { get; set; }

            public class BoligPortalEconomy
            {
                public string Rent { get; set; }
            }

            [JsonProperty(PropertyName = "jqt_adUrl")]
            public string Url { get; set; }

            [JsonProperty(PropertyName = "jqt_images")]
            public List<BoligPortalImages> Images { get; set; }

            public class BoligPortalImages
            {
                public string Thumb { get; set; }
            }

        }
    }

    public enum AmtEnum
    {
        // TODO: Add all 'amt' to this enum list
        Aarhus = 14
    }
}