using System;

namespace OMR.Core.Helpers.Logging
{
    public interface ILogFactory
    {
        ILogger GetLogger(string name);

        ILogger GetLogger(Type type);
    }
}
