namespace OMR.Core.Helpers
{
    using OMR.Core.Helpers.Ioc;

    public static class Bootstraper
    {
        public static IIocContainer IocContainer { get; private set; }

        public static void SetIocContainer(IIocContainer iocContainer)
        {
            IocContainer = iocContainer;
        }

    }

}