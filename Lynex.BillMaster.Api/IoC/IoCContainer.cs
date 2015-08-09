using System.Web.Http;
using System.Web.Mvc;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Lynex.BillMaster.Api.IoC.Factory;

namespace Lynex.BillMaster.Api.IoC
{
    public static class IoCContainer
    {
        private static IWindsorContainer _container;

        public static void Setup()
        {
            _container = new WindsorContainer().Install(FromAssembly.This());
            GlobalConfiguration.Configuration.DependencyResolver = new WindsorDependencyResolver(_container);
            var controllerFactory = new WindsorControllerFactory(_container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
        }
    }
}