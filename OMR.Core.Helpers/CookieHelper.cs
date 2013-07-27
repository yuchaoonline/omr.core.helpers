namespace OMR.Core.Helpers
{
    using System;
    using System.ComponentModel;
    using System.Web;

    public abstract class CookieHelper
    {
        /// <summary>
        /// O anki HttpRequest nesnesini döndürür.
        /// </summary>
        private static HttpRequest _request
        {
            get
            {
                // HttpContext.Current ya da HttpContext.Current.Request null ise hata fırlat
                AssertHelper.AreNotNull(HttpContext.Current, HttpContext.Current.Request);

                return HttpContext.Current.Request;
            }
        }

        /// <summary>
        /// O anki HttpResponse nesnesini döndürür
        /// </summary>
        private static HttpResponse _response
        {
            get
            {
                // HttpContext.Current ya da HttpContext.Current.Response null ise hata fırlat
                AssertHelper.AreNotNull(HttpContext.Current, HttpContext.Current.Response);

                return HttpContext.Current.Response;
            }
        }

        /// <summary>
        /// Çerezin varlığını kontrol eder
        /// </summary>
        /// <param name="name">Çerezin adı</param>
        /// <returns></returns>
        public static bool Exists(string name)
        {
            // name null ise hata fırlat
            AssertHelper.AreNotNull(name);

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            return _request.Cookies[name] != null;
        }

        /// <summary>
        /// Çerez değerini T tipinde döndürür
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T Get<T>(string name, T defaultValue)
        {
            // name null ise hata fırlat
            AssertHelper.AreNotNull(name);

            // Çerez bulunamamışsa varsayılanı döndür
            if (_request.Cookies[name] == null)
                return defaultValue;

            string cookieValue = _request.Cookies[name].Value;

            // T tipine uygun tip dönüştürücüyü getir
            TypeConverter typeConverter = TypeDescriptor.GetConverter(typeof(T));

            // Converter yoksa ya da uygun converter
            // bulunamamışsa varsayılanı döndür
            if (typeConverter == null || !typeConverter.IsValid(cookieValue))
                return defaultValue;

            try
            {
                // Çerez değerini T tipine döndürmeyi dene
                return (T)typeConverter.ConvertFrom(cookieValue);
            }
            catch (Exception)
            {
                // Hata oluşursa varsayılanı döndür
                return defaultValue;
            }
        }

        /// <summary>
        /// Çereze değer atamak için kullanılır. Çerez varsa yeni
        /// değer atanır yoksa yeni çerez oluşturulur.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="expires"></param>
        public static void Set(string name, string value, DateTime? expires)
        {
            // name ya da value null ise hata fırlat
            AssertHelper.AreNotNullOrEmpty(name, value);

            // HttpCookie örneği oluştur
            HttpCookie httpCookie = new HttpCookie(name, value);
            httpCookie.HttpOnly = true;

            // Geçerlilik süresi belirlenmişse ata
            if (expires.HasValue)
                httpCookie.Expires = expires.Value;

            // Çerezi HttpResponse nesnesine ekle
            _response.Cookies.Add(httpCookie);
        }

        /// <summary>
        /// Varolan çerezin geçerliliğini iptal eder.
        /// </summary>
        /// <param name="name"></param>
        public static void ExpireCookie(string name)
        {
            // name null ise hata fırlat
            AssertHelper.AreNotNullOrEmpty(name);

            // Aynı isimde HttpCookie nesnesi oluştur
            // Geçerlilik süresini 2 gün geride tanımla
            Set(name, string.Empty, DateTime.Now.AddDays(-2));
        }
    }
}
