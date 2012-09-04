using System;
using System.Configuration;

namespace BoligScraper
{
    public static class AppSettingsHelper
    {
        public static T GetValue<T>(string key)
        {
            string value = ConfigurationManager.AppSettings[key];

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }
    }
}
