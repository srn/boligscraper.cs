using System;
using System.Collections.Generic;
using System.Linq;
using RestSharp;

namespace BoligScraper.Job
{
    class Program
    {
        private static Scraper _boligScraper;

        static void Main(string[] args)
        {
            _boligScraper = new Scraper();


            UserPreference userPreference = GetUserPreference();

            var boligPortalRequest = new BoligPortalRequest
            {
                Amt = ((int)userPreference.Region).ToString(),
                RentMin = "0",
                RentMax = userPreference.RentMax,
                SizeMin = "0",
                SizeMax = "0",
                ZipCodes = userPreference.ZipCodes,
                ApartmentType = new List<string> { "2" },
                RentLength = new List<string> { "4" },
                Page = "1",
                Limit = "15",
                SortCol = "3",
                SortDesc = "1",
                ShowOnSiteApartment = 0,
                Default = -1,
                Pictures = -1,
                HousePets = -1,
                Furnished = -1,
                Shareable = -1,
                OnlyLatest = false,
                Sublease = -1
            };

            IRestResponse restResponse;
            BoligPortalResponse boligPortalResponse = _boligScraper.Scrape(boligPortalRequest, out restResponse);

            List<string> newIds = boligPortalResponse.Properties.Select(p => p.Id).ToList();
            IList<string> compareCachedIdsWithNewIds = _boligScraper.CompareCachedIdsWithNewIds(newIds);

            compareCachedIdsWithNewIds.ForEach((i, id) =>
                                                   {
                                                       BoligPortalProperty boligPortalProperty = boligPortalResponse.Properties.SingleOrDefault(p => p.Id == id);

                                                       if (boligPortalProperty == null)
                                                           return;

                                                       _boligScraper.SendEmail(boligPortalProperty, userPreference);
                                                       Console.ReadLine();

                                                       Console.WriteLine(string.Format("Send email for '{0}'", boligPortalProperty.Id));
                                                   });

            Console.ReadLine();
        }

        private static UserPreference GetUserPreference()
        {
            var region = AppSettingsHelper.GetValue<string>("Region");
            var regionEnum = (RegionEnum) Enum.Parse(typeof (RegionEnum), region, true);

            // TODO: userPreference.ApartmentType = AppSettingsHelper.GetValue<ApartmentTypeEnum>("ApartmentType");
            var userPreference = new UserPreference
                                     {
                                         Region = regionEnum,
                                         Email = AppSettingsHelper.GetValue<string>("Email"),
                                         RentMax = AppSettingsHelper.GetValue<string>("RentMax"),
                                         ZipCodes = GetAppSettingsZipCodes()
                                     };
            return userPreference;
        }

        private static List<int> GetAppSettingsZipCodes()
        {
            var zipCodes = new List<int>();
            
            var appSettingsHelper = AppSettingsHelper.GetValue<string>("ZipCodes").Replace(" ", string.Empty);
            string[] splittedZipCodes = appSettingsHelper.Split(Convert.ToChar(","));
            splittedZipCodes.ForEach((i, zipCode) => zipCodes.Add(int.Parse(zipCode)));

            return zipCodes;
        }
    }
}
