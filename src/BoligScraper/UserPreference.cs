using System.Collections.Generic;

namespace BoligScraper
{
    public class UserPreference
    {
        public string Email { get; set; }
        public RegionEnum Region { get; set; }
        public string RentMax { get; set; }
        public IList<int> ZipCodes { get; set; }
        public IList<string> ApartmentTypes { get; set; }
    }
}