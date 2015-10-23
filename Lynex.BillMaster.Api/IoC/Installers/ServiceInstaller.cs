using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using log4net;
using Lynex.BillMaster.Api.Filters;
using Lynex.BillMaster.Service;
using Lynex.BillMaster.Service.Interface;
using Lynex.Common.Database;

namespace Lynex.BillMaster.Api.IoC.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IDatabaseService>().ImplementedBy<DatabaseService>().LifestylePerWebRequest());
            container.Register(Component.For<IAddressService>().ImplementedBy<AddressService>().LifestylePerWebRequest());
            container.Register(Component.For<IUserService>().ImplementedBy<UserService>().LifestylePerWebRequest());
            container.Register(Component.For<ILog>().Instance(LogManager.GetLogger(typeof(WebApiApplication))));

            container.Register(Component.For<AuthenticateFilter>());
            //container.Register(Component.For<IDosageService>().ImplementedBy<DosageService>().LifestylePerWebRequest());
            //container.Register(Component.For<IUserService>().ImplementedBy<UserService>().LifestylePerWebRequest());
            //container.Register(Component.For<ITestResultService>().ImplementedBy<TestResultService>().LifestylePerWebRequest());

            //container.Register(Component.For<IEmailNotificationService>().ImplementedBy<EmailNotificationService>().LifestyleSingleton());
            //container.Register(Component.For<ISMSNotificationService>().ImplementedBy<SMSNotificationService>().LifestyleSingleton());

        }
    }
}