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
}