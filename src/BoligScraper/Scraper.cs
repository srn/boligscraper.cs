﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FluentEmail;
using Newtonsoft.Json;
using RestSharp;

namespace BoligScraper
{
    public class Scraper
    {
        private static List<string> _cachedBoligPortalIds;

        public BoligPortalResponse Scrape(BoligPortalRequest boligPortalRequest, out IRestResponse restResponse)
        {
            string serializedBoligPortalRequest = JsonConvert.SerializeObject(boligPortalRequest).Replace("null", "\"\"");

            var client = new RestClient { BaseUrl = "http://www.boligportal.dk" };
            var request = new RestRequest(Method.POST) { Resource = "api/soeg_leje_bolig.php" };

            request.AddParameter("serviceName", "getProperties");
            request.AddParameter("data", serializedBoligPortalRequest);

            restResponse = client.Execute(request);

            var boligPortalResponse = JsonConvert.DeserializeObject<BoligPortalResponse>(restResponse.Content);

            return boligPortalResponse;
        }

        public IList<string> CompareAndReturnNewIds(IList<string> newIds)
        {
            if (_cachedBoligPortalIds == null || !_cachedBoligPortalIds.Any())
            {
                _cachedBoligPortalIds = newIds.ToList();

                return newIds;
            }

            var mergedIds = new List<string>();
            mergedIds.AddRange(_cachedBoligPortalIds);
            mergedIds.AddRange(newIds);

            IList<string> compareCachedIdsWithNewIds = mergedIds.Except(_cachedBoligPortalIds).ToList();

            _cachedBoligPortalIds.Clear();
            _cachedBoligPortalIds = newIds.ToList();

            return compareCachedIdsWithNewIds;
        }

        public void SendEmail(BoligPortalProperty boligPortalProperty, UserPreference userPreference)
        {
            var email = Email.From("myopentracker@gmail.com", "boligscraper.io")
                         .To(userPreference.Email)
                         .Subject(String.Format("{0} - {1}", boligPortalProperty.Headline, boligPortalProperty.Economy.Rent))
                         .Body(String.Format("http://www.boligportal.dk{0}", boligPortalProperty.Url));

            email.Send();
        }
    }
}