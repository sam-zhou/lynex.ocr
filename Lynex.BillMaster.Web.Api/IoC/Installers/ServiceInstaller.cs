using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using log4net;
using Lynex.BillMaster.Service;
using Lynex.BillMaster.Service.Interface;
using Lynex.Common.Database;
using Lynex.Common.Service;
using Lynex.Common.Service.Interface;


namespace Lynex.BillMaster.Web.Api.IoC.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IDatabaseService>().ImplementedBy<DatabaseService>().LifestylePerWebRequest());
            container.Register(Component.For<IAddressService>().ImplementedBy<AddressService>().LifestylePerWebRequest());
            container.Register(Component.For<IUserService>().ImplementedBy<UserService>().LifestylePerWebRequest());
            container.Register(Component.For<IAuthenticationService>().ImplementedBy<AuthenticationService>().LifestylePerWebRequest());
            container.Register(Component.For<ILog>().Instance(LogManager.GetLogger(typeof(WebApiApplication))));

            //container.Register(Component.For<AuthenticateFilter>());
            //container.Register(Component.For<IDosageService>().ImplementedBy<DosageService>().LifestylePerWebRequest());
            //container.Register(Component.For<IUserService>().ImplementedBy<UserService>().LifestylePerWebRequest());
            //container.Register(Component.For<ITestResultService>().ImplementedBy<TestResultService>().LifestylePerWebRequest());

            //container.Register(Component.For<IEmailNotificationService>().ImplementedBy<EmailNotificationService>().LifestyleSingleton());
            //container.Register(Component.For<ISMSNotificationService>().ImplementedBy<SMSNotificationService>().LifestyleSingleton());

        }
    }
}