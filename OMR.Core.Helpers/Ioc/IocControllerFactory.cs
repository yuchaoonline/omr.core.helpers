using System;
namespace OMR.Core.Helpers.Ioc
{
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    public class IocControllerFactory : DefaultControllerFactory
    {
        public IocControllerFactory()
            : base()
        {
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
                throw new HttpException(404, "Page not found: " + requestContext.HttpContext.Request.Path);

            if (!typeof(IController).IsAssignableFrom(controllerType))
                throw new InvalidCastException("Controller not found");

            object[] parameters = null;

            var constructor = controllerType.GetConstructors().FirstOrDefault(c => c.GetParameters().Length > 0);
            if (constructor != null)
            {
                var parametersInfo = constructor.GetParameters();
                parameters = new object[parametersInfo.Length];

                for (int i = 0; i < parametersInfo.Length; i++)
                {
                    var p = parametersInfo[i];

                    if (!Bootstraper.IocContainer.Contains((p.ParameterType)))
                        throw new ApplicationException(controllerType.Name + " type can not be found on IoC container");

                    parameters[i] = Bootstraper.IocContainer.Create(p.ParameterType);
                }
            }

            try
            {
                return (IController)Activator.CreateInstance(controllerType, parameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
