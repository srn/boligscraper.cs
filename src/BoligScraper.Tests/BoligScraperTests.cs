using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RestSharp;

namespace BoligScraper.Tests
{
    [TestFixture]
    public class BoligScraperTests
    {
        private Scraper _boligScraper;

        [SetUp]
        public void SetUp()
        {
            _boligScraper = new Scraper();
        }

        [Test]
        public void CanCreateBoligPortalRequestAndDeserializeData()
        {
            // Arrange
            var boligPortalRequest = new BoligPortalRequest
                                         {
                                             Amt = RegionEnum.Aarhus,
                                             RentMin = "0",
                                             RentMax = "10000",
                                             ZipCodes = new List<int> {8000, 8200},
                                             ApartmentType = new List<string> {"1", "2", "3", "4"},
                                             RentLength = new List<string> {"4"},
                                             Page = "1",
                                             Limit = "15",
                                             SortCol = "3",
                                             SortDesc = "1"
                                         };

            // Act
            IRestResponse restResponse = null;
            BoligPortalResponse boligPortalResponse = _boligScraper.Scrape(boligPortalRequest, out restResponse);

            // Assert
            Assert.That(restResponse, Is.Not.Null);
            Assert.That(boligPortalRequest, Is.Not.Null);
            Assert.That(boligPortalResponse.Properties.Count, Is.Not.EqualTo(0));
        }

        [Test]
        public void CanCompareOldIdsWithNewIdsAndReturnNewIdsOnly()
        {
            // Arrange
            var oldIds = new List<int> { 123, 1234, 123456, 1233 };
            var newIds = new List<int> { 123, 1234, 12345 };

            // Act
            var mergedIds = new List<int>();
            mergedIds.AddRange(oldIds);
            mergedIds.AddRange(newIds);

            IList<int> except = mergedIds.Except(oldIds).ToList();

            // Assert
            Assert.That(except, Is.Not.Null);
            Assert.That(except.Count(), Is.EqualTo(1));
            Assert.That(except.SingleOrDefault(), Is.EqualTo(12345));
        }
    }
}
