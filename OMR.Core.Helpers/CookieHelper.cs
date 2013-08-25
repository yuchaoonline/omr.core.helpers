namespace OMR.Core.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Web;

    public abstract class CookieHelper
    {
        private static HttpRequest _request
        {
            get
            {
                AssertHelper.AreNotNull(HttpContext.Current, HttpContext.Current.Request);

                return HttpContext.Current.Request;
            }
        }

        private static HttpResponse _response
        {
            get
            {
                AssertHelper.AreNotNull(HttpContext.Current, HttpContext.Current.Response);

                return HttpContext.Current.Response;
            }
        }

        public static bool Exists(string name)
        {
            AssertHelper.AreNotNull(name);

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _request.Cookies[name] != null;
        }

        public static T Get<T>(string name, T defaultValue)
        {
            AssertHelper.AreNotNull(name);

            if (_request.Cookies[name] == null)
                return defaultValue;

            string cookieValue = _request.Cookies[name].Value;

            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));

            if (typeConverter == null || !typeConverter.IsValid(cookieValue))
                return defaultValue;

            try
            {
                return (T)typeConverter.ConvertFrom(cookieValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static void Set(string name, string value, DateTime? expires)
        {
            AssertHelper.AreNotNullOrEmpty(name, value);

            HttpCookie httpCookie = new HttpCookie(name, value);
            httpCookie.HttpOnly = true;

            if (expires.HasValue)
                httpCookie.Expires = expires.Value;

            _response.Cookies.Add(httpCookie);
        }

        public static void ExpireCookie(string name)
        {
            AssertHelper.AreNotNullOrEmpty(name);

            Set(name, string.Empty, DateTime.Now.AddDays(-2));
        }

    }
}
