using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Mail;
using FluentEmail;
using Newtonsoft.Json;
using RestSharp;

namespace BoligScraper
{
    public class Scraper
    {
        private IList<string> _cachedBoligPortalIds;

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

        public IList<string> CompareCachedIdsWithNewIds(IList<string> newIds)
        {
            if (_cachedBoligPortalIds == null || !_cachedBoligPortalIds.Any())
                return newIds;

            var mergedIds = new List<string>();
            mergedIds.AddRange(_cachedBoligPortalIds);
            mergedIds.AddRange(newIds);

            IList<string> compareCachedIdsWithNewIds = mergedIds.Except(_cachedBoligPortalIds).ToList();

            _cachedBoligPortalIds = newIds;

            return compareCachedIdsWithNewIds;
        }

        public void SendEmail(BoligPortalProperty boligPortalProperty, UserPreference userPreference)
        {
            var email = Email
                         .From("myopentracker@gmail.com", "boligscraper.io")
                         .To(userPreference.Email)
                         .Subject(string.Format("{0} - {1}", boligPortalProperty.Headline, boligPortalProperty.Economy.Rent))
                         .Body(string.Format("http://www.boligportal.dk{0}", boligPortalProperty.Url));

            email.SendAsync(MailDeliveryComplete);
        }

        private static void MailDeliveryComplete(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // handle error
            }
            else if (e.Cancelled)
            {
                // handle cancelled
            }
            else
            {
                // handle sent email
                var message = (MailMessage)e.UserState;
            }
        }
    }
}