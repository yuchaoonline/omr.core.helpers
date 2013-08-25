using Microsoft.VisualStudio.TestTools.UnitTesting;
using OMR.Core.Helpers.Ioc;

namespace OMR.Tests.Ioc
{
    [TestClass]
    public class IocContainerTests
    {
        [TestMethod]
        public void IocContainer_RegisterSimpleTypeAndCreate()
        {
            IocContainer c = new IocContainer();

            var expected = 5;
            c.Register<int>(() => expected);

            var actual = c.Create<int>();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IocContainer_RegisterIntefaceToClassAndCreate()
        {
            IocContainer c = new IocContainer();

            var expected = 5;
            c.Register<IDummy>(() => new DummyClass(expected));

            var actual = c.Create<IDummy>().GetValue();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IocContainer_RegisterIntefaceToClassAndCreateNewInstance()
        {
            IocContainer c = new IocContainer();
            c.Register<IProvider, AProvider>();

            var actual = c.Create<IProvider>().Name;

            Assert.AreEqual("A", actual);
        }
    }

    class mat
    {
        public static double Topla(double s1, double s2)
        {
            return s1 + s2;
        }
    }

    public interface IDummy
    {
        int GetValue();
    }


    public class DummyClass : IDummy
    {
        private int _i;

        public DummyClass(int i)
        {
            _i = i;
        }

        public int GetValue()
        {
            return _i;
        }
    }

    interface IProvider
    {
        string Name { get; }
    }

    class AProvider : IProvider
    {
        public string Name
        {
            get { return "A"; }
        }
        
    }

}
