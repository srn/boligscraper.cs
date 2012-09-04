using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RestSharp;

namespace BoligScraper
{
    public class Scraper
    {
        private IList<string> _cachedBoligPortalIds;

        public BoligPortalResponse Scrape(BoligPortalRequest boligPortalRequest, out IRestResponse restResponse)
        {
            var client = new RestClient { BaseUrl = "http://www.boligportal.dk" };
            var request = new RestRequest(Method.POST) { Resource = "api/soeg_leje_bolig.php" };

            request.AddParameter("serviceName", "getProperties");
            request.AddBody(JsonConvert.SerializeObject(boligPortalRequest));

            restResponse = client.Execute(request);

            var boligPortalResponse = JsonConvert.DeserializeObject<BoligPortalResponse>(restResponse.Content);

            _cachedBoligPortalIds = boligPortalResponse.Properties.Select(p => p.Id).ToList();

            return boligPortalResponse;
        }

        public void CompareCachedIdsWithNewIds()
        {
            throw new NotImplementedException();
        }
    }
}