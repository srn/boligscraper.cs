using System.Collections.Generic;

namespace BoligScraper
{
    public class UserPreference
    {
        public string Email { get; set; }
        public RegionEnum Region { get; set; }
        public string RentMax { get; set; }
        public List<int> ZipCodes { get; set; }
        public ApartmentTypeEnum ApartmentType { get; set; }
    }

    public enum ApartmentTypeEnum
    {

    }
}