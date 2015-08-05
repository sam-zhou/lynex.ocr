using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;


namespace WCC.UI.IoC.Installers
{
    public class SettingInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(Component.For<IWCCSettings>().ImplementedBy<WCCSettings>());

            
        }
    }
}