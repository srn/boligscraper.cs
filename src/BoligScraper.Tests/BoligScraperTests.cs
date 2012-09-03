using System.Collections.Generic;
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
                                             Amt = AmtEnum.Aarhus,
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
    }
}
