namespace OMR.Core.Helpers
{
    using System;
    using System.IO;
    using System.Web.Hosting;

    public static class PathHelper
    {
        public static string AbsoluteRoot { get; set; }

        static PathHelper()
        {
            if (ApplicationHelper.GetApplicationType() == ApplicationHelper.ApplicationTypes.Web)
            {
                AbsoluteRoot = GetAbsolutePath("~/");
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        public static string GetAbsolutePath(string relativePath)
        {
            AssertHelper.AreNotNullOrEmpty(relativePath);

            if (relativePath.StartsWith("~/"))
                return HostingEnvironment.MapPath(relativePath);

            return Path.Combine(AbsoluteRoot, relativePath);
        }

        public static string AbsoluteToRelativePath(string absolutePath)
        {
            AssertHelper.AreNotNullOrEmpty(absolutePath);

            if (AbsoluteRoot.Length > absolutePath.Length)
                return string.Empty;

            return absolutePath.Replace(AbsoluteRoot, string.Empty);
        }
    }
}
