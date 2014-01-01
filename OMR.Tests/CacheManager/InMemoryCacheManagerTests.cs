using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMR.Core.Helpers.Cache;
using System;
using System.Threading;

namespace OMR.Tests.CacheManager
{
    [TestClass]
    public class InMemoryCacheManagerTests
    {
        [TestMethod]
        public void InMemoryCacheManager_GetOrSetTest1()
        {
            ICache cm = new InMemoryCache();

            Func<string> function = () => { return "demo"; };

            var expectedValue = function();

            cm.GetOrSet<string>("key", function, 60);

            var actualValue = cm.Get<string>("key", null);

            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void InMemoryCacheManager_SetGetIntegerValueIn20Seconds()
        {
            ICache cm = new InMemoryCache();
            cm.Set("tmp1", 5, 20);

            Assert.AreEqual(cm.Get<int>("tmp1", -1), 5);
        }

        [TestMethod]
        public void InMemoryCacheManager_DefaultValueTest()
        {
            ICache cm = new InMemoryCache();

            var expected = -3;
            var actual = cm.Get<int>("foo", expected);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void InMemoryCacheManager_SetGetExpandoObjectIn20Seconds()
        {
            ICache cm = new InMemoryCache();

            string str = "test";

            cm.Set("tmp2", str, 20);

            Assert.AreEqual(cm.Get<string>("tmp2", null), str);
        }

        [TestMethod]
        public void InMemoryCacheManager_SetGetExpiredObjectIn1Seconds()
        {
            ICache cm = new InMemoryCache();

            var expected = "Expected";

            cm.Set("key", expected, 1);

            Thread.Sleep(1200);

            var actual = cm.Get<string>("key", null);

            Assert.AreEqual(actual, null);
        }
    }
}
