namespace OMR.Core.Helpers
{
    using System.ComponentModel;
    using System.Configuration;

    public class ConfigHelper
    {
        public static string AppSetting(string key)
        {
            AssertHelper.AreNotNull(key);

            return AppSetting<string>(key, default(string));
        }

        public static T AppSetting<T>(string key)
        {
            AssertHelper.AreNotNull(key);

            return AppSetting<T>(key, default(T));
        }

        public static T AppSetting<T>(string key, T defaultValue)
        {
            AssertHelper.AreNotNull(key);

            string value = ConfigurationManager.AppSettings[key];

            if (value == null)
                return defaultValue;

            try
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));
                T retVal = (T)typeConverter.ConvertFrom(value);

                return retVal;
            }
            catch
            {
                return defaultValue;
            }
        }

        public static string ConenctionString(string key)
        {
            AssertHelper.AreNotNull(key);

            var csSetting = ConfigurationManager.ConnectionStrings[key];
            return csSetting == null ? string.Empty : csSetting.ConnectionString;
        }
    }
}