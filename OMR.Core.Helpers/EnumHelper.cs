namespace OMR.Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;

    public static class EnumHelper
    {
        public static int GetValue<T>(string description)
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                if (description == GetDescription(item as Enum))
                {
                    return (int)item;
                }
            }

            return -1;
        }

        public static string GetDescription(this Enum e)
        {
            var fi = e.GetType().GetField(e.ToString());
            var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes == null || attributes.Length == 0)
                throw new InvalidEnumArgumentException("Description attribute not found.");

            return attributes[0].Description;
        }

        public static Dictionary<string, int> GetAsDictionary<T>()
        {
            var r = new Dictionary<string, int>();

            var collection = GetAsCustomType<T, KeyValuePair<string, int>>(
                                                            (s, v) => { return new KeyValuePair<string, int>(s, v); }
                                                        );

            foreach (var item in collection)
            {
                r.Add(item.Key, item.Value);
            }

            return r;
        }

        public static IEnumerable<TTarget> GetAsCustomType<TEnum, TTarget>(Func<string, int, TTarget> parser)
        {
            var values = Enum.GetValues(typeof(TEnum));
            var r = new TTarget[values.Length];

            foreach (var item in values)
            {
                yield return parser(GetDescription((Enum)item), (int)item);
            }
        }

    }


}
