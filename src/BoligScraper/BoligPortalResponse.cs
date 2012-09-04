using System.Collections.Generic;
using Newtonsoft.Json;

namespace BoligScraper
{
    public class BoligPortalResponse
    {
        public List<BoligPortalProperty> Properties { get; set; }
    }
}