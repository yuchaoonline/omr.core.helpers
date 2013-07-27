using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMR.Core.Helpers.Localization;

namespace OMR.Tests.Localization
{
    [TestClass]
    public class InMemoryLocalizationManagerTests
    {
        [TestMethod]
        public void InMemoryLocalization_etGetStringValue()
        {
            ILocalizationManager lm = new InMemoryLocalizationManager();

            string expected = "expected";

            lm.Set("region", "name", expected);

            string actual = lm.Get<string>("region", "name");

            Assert.AreEqual(expected, actual);
        }

    }
}
