using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using RestSharp;

namespace BoligScraper.Job
{
    internal class Program
    {
        private static Scraper _boligScraper;
        private static UserPreference _userPreference;
        private static BoligPortalRequest _boligPortalRequest;

        private static bool _firstRun = true;

        private static void Tick()
        {
            Console.WriteLine(string.Format("{0} :: Scraping", DateTime.Now));

            IRestResponse restResponse;
            BoligPortalResponse boligPortalResponse = _boligScraper.Scrape(_boligPortalRequest, out restResponse);

            if (restResponse.StatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(string.Format("Received an invalid status code: {0}. Should be {1}", restResponse.StatusCode, HttpStatusCode.OK));
                return;
            }

            if (boligPortalResponse == null || boligPortalResponse.Properties == null || !boligPortalResponse.Properties.Any())
            {
                Console.WriteLine("Received a non-deserializable response.");
                return;
            }

            if (!_firstRun)
            {
                List<string> newIds = boligPortalResponse.Properties.Select(p => p.Id).ToList();
                IList<string> compareCachedIdsWithNewIds = _boligScraper.CompareAndReturnNewIds(newIds);

                compareCachedIdsWithNewIds.ForEach((i, id) => HandleEmail(boligPortalResponse, id, _userPreference));
            }

            Console.WriteLine(string.Format("{0} :: Waiting for next callback", DateTime.Now));

            _firstRun = false;
        }

        private static void Main(string[] args)
        {
            _boligScraper = new Scraper();

            _userPreference = GetUserPreference();
            _boligPortalRequest = new BoligPortalRequest
                                      {
                                          Amt = ((int) _userPreference.Region).ToString(),
                                          RentMin = "0",
                                          RentMax = _userPreference.RentMax,
                                          ZipCodes = _userPreference.ZipCodes,
                                          ApartmentType = _userPreference.ApartmentTypes,
                                          RentLength = new List<string> {"4"},
                                          Page = "1",
                                          Limit = "15",
                                          SortCol = "3",
                                          SortDesc = "1"
                                      };

            Console.WriteLine("{0} :: Creating infinite loop\n", DateTime.Now);

            // infinite loop
            while (true)
            {
                Tick();

                Thread.Sleep(120000); // 2 minutes
            }
        }

        private static void HandleEmail(BoligPortalResponse boligPortalResponse, string id,
                                        UserPreference userPreference)
        {
            BoligPortalProperty boligPortalProperty = boligPortalResponse.Properties.SingleOrDefault(p => p.Id == id);

            if (boligPortalProperty == null)
                return;

            _boligScraper.SendEmail(boligPortalProperty, userPreference);

            Console.WriteLine(string.Format("{0} :: Send email for '{1}' to '{2}'", DateTime.Now, boligPortalProperty.Id, userPreference.Email));
        }

        private static UserPreference GetUserPreference()
        {
            var region = AppSettingsHelper.GetValue<string>("Region");
            var regionEnum = (RegionEnum) Enum.Parse(typeof (RegionEnum), region, true);

            var userPreference = new UserPreference
                                     {
                                         Region = regionEnum,
                                         Email = AppSettingsHelper.GetValue<string>("Email"),
                                         RentMax = AppSettingsHelper.GetValue<string>("RentMax"),
                                         ZipCodes = GetZipCodes(),
                                         ApartmentTypes = GetApartmentTypes()
                                     };

            Console.WriteLine(string.Format("{0} ::\nRegion: {1}\n Email: {2}\n RentMax: {3}\n ZipCodes: {4}\n ApartmentTypes: {5}\n", DateTime.Now, userPreference.Region, userPreference.Email, userPreference.RentMax, AppSettingsHelper.GetValue<string>("ZipCodes"), AppSettingsHelper.GetValue<string>("ApartmentTypes")));

            return userPreference;
        }

        // TODO: Refactor these two _almost_ identical methods
        private static IList<int> GetZipCodes()
        {
            var zipCodes = new List<int>();

            string appSettingsHelper = AppSettingsHelper.GetValue<string>("ZipCodes").Replace(" ", string.Empty);
            string[] splittedZipCodes = appSettingsHelper.Split(Convert.ToChar(","));
            splittedZipCodes.ForEach((i, zipCode) => zipCodes.Add(int.Parse(zipCode)));

            return zipCodes;
        }

        private static IList<string> GetApartmentTypes()
        {
            var apartmentTypes = new List<string>();

            string apartmentTypesFromAppSettings = AppSettingsHelper.GetValue<string>("ApartmentTypes").Replace(" ", string.Empty);
            string[] splittedApartmentTypes = apartmentTypesFromAppSettings.Split(Convert.ToChar(","));
            splittedApartmentTypes.ForEach((i, apartmentType) => apartmentTypes.Add(apartmentType));

            return apartmentTypes;
        }
    }
}