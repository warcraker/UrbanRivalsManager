using System;
using System.Collections.Generic;
using System.Text;

namespace Warcraker.UrbanRivals.URManager.Configuration
{
    public class ConfigurationManager
    {
        public static class SettingKeys
        {
            public const string LANGUAGE = "language";
        }

        private readonly IDictionary<string, string> _memorySettings = new Dictionary<string, string>();

        public string GetValue(string key)
        {
            string value;

            if (_memorySettings.ContainsKey(key))
            {
                value = _memorySettings[key];
            }

            throw new ArgumentException($"{nameof(ConfigurationManager)}.{nameof(GetValue)} - Invalid {nameof(key)}: {key}");
        }

        public void SetValue(string key)
        {
            throw new ArgumentException($"{nameof(ConfigurationManager)}.{nameof(SetValue)} - Invalid {nameof(key)}: {key}");
        }
    }
}
