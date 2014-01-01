namespace OMR.Core.Helpers.Localization
{
    using OMR.Core.Helpers.Web.HttpApplication;
    using System;
    using System.Web;

    public class LocalizationModule : IHttpModule
    {
        public void Dispose()
        { }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += context_BeginRequest;
        }

        void context_BeginRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;

            //TODO:
            application.Context.Response.Filter = new GlobalResponseFilter(
                application.Context.Response.Filter, (s) => { return s.Replace("#hi#", "heyya"); }
            );
        }
    }
}
