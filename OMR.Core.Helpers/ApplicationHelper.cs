namespace OMR.Core.Helpers
{
    using System.Web;

    public abstract class ApplicationHelper
    {
        public static ApplicationTypes GetApplicationType()
        {
            if (HttpRuntime.AppDomainAppId != null)
            {
                return ApplicationTypes.Web;
            }

            return ApplicationTypes.Desktop;
        }


        public enum ApplicationTypes
        {
            Web,
            Desktop
        }
    }

}
