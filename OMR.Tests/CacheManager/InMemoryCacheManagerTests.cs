using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMR.Core.Helpers.Cache;
using System.Threading;

namespace OMR.Tests.CacheManager
{
    [TestClass]
    public class InMemoryCacheManagerTests
    {
        [TestMethod]
        public void InMemoryCacheManager_SetGetIntegerValueIn20Seconds()
        {
            ICacheManager cm = new InMemoryCacheManager();
            cm.Set("tmp1", 5, 20);

            Assert.AreEqual(cm.Get<int>("tmp1", -1), 5);
        }

        [TestMethod]
        public void InMemoryCacheManager_DefaultValueTest()
        {
            ICacheManager cm = new InMemoryCacheManager();

            var expected = -3;
            var actual = cm.Get<int>("foo", expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InMemoryCacheManager_SetGetExpandoObjectIn20Seconds()
        {
            ICacheManager cm = new InMemoryCacheManager();

            string str = "test";

            cm.Set("tmp2", str, 20);

            Assert.AreEqual(cm.Get<string>("tmp2", null), str);
        }

        [TestMethod]
        public void InMemoryCacheManager_SetGetExpiredObjectIn1Seconds()
        {
            ICacheManager cm = new InMemoryCacheManager();

            var expected = "Expected";

            cm.Set("key", expected, 1);

            Thread.Sleep(1200);

            var actual = cm.Get<string>("key", null);

            Assert.AreEqual(actual, null);
        }
    }
}
