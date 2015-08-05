using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace WCC.UI.IoC.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(Component.For<IWCCMainRepository>().ImplementedBy<WCCMainRepository>().LifestylePerWebRequest());

            //container.Register(Component.For<ISystemService>().ImplementedBy<SystemService>().LifestylePerWebRequest());
            //container.Register(Component.For<IDosageService>().ImplementedBy<DosageService>().LifestylePerWebRequest());
            //container.Register(Component.For<IUserService>().ImplementedBy<UserService>().LifestylePerWebRequest());
            //container.Register(Component.For<ITestResultService>().ImplementedBy<TestResultService>().LifestylePerWebRequest());

            //container.Register(Component.For<IEmailNotificationService>().ImplementedBy<EmailNotificationService>().LifestyleSingleton());
            //container.Register(Component.For<ISMSNotificationService>().ImplementedBy<SMSNotificationService>().LifestyleSingleton());

        }
    }
}