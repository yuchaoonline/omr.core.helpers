using System;
using System.Collections.Generic;
using System.IO;

namespace OMR.Core.Helpers.Web.HttpApplication
{
    public class GlobalResponseFilter : BaseReponseFilter
    {
        private List<Func<string, string>> _functions;

        public GlobalResponseFilter(Stream stream, List<Func<string, string>> functions)
            : base(stream)
        {
            _functions = functions;
        }

        public GlobalResponseFilter(Stream stream, Func<string, string> function)
            : base(stream)
        {
            _functions = new List<Func<string, string>>() { function };
        }

        public override string FilterString(string input)
        {
            if (_functions == null)
                return input;

            foreach (var item in _functions)
            {
                if (item == null)
                    continue;

                input = item(input);
            }

            return input;
        }
    }


}
