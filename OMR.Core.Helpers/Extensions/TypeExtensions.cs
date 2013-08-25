namespace OMR.Core.Helpers.Extensions
{
    using System;

    public static class TypeExtensions
    {
        public static T ChangeTypeTo<T>(this object obj) where T : IConvertible
        {
            if (obj == null)
                return default(T);

            try
            {
                return (T)Convert.ChangeType(obj, typeof(T));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
