namespace OMR.Core.Helpers.Localization
{
    using System;
    using System.Collections.Generic;

    public class InMemoryLocalizationManager : ILocalizationManager
    {
        const string NAMING_FORMAT = "{0}{1}{2}";
        const string REGION_NAME_SPERATOR = "__";

        private Dictionary<string, object> _localizations;

        public InMemoryLocalizationManager() :this(null)
        {}

        public InMemoryLocalizationManager(Dictionary<string, object> localizations)
        {
            if (localizations == null)
            {
                _localizations = new Dictionary<string, object>();
            }
            else
            {
                _localizations = localizations;
            }
        }

        public T Get<T>(string region, string name)
        {
            string fullName = GetFullKey(region, name);
            return (T)_localizations[fullName];
        }

        public void Set(string region, string name, object value)
        {
            if (name != null && name.Contains("__"))
                throw new InvalidOperationException();

            string fullName = GetFullKey(region, name);
            _localizations.Add(fullName, value);
        }

        public bool Exists(string region, string name)
        {
            string fullName = GetFullKey(region, name);
            return _localizations.ContainsKey(fullName);
        }

        private string GetFullKey(string region, string name)
        {
            return string.Format(NAMING_FORMAT, region, REGION_NAME_SPERATOR, name);
        }

    }
}
