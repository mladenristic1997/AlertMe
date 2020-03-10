using AlertMe.AlertSoundSelector;
using AlertMe.Domain;
using AlertMe.Home;
using AlertMe.Plans;
using AlertMe.Rain;
using CommonServiceLocator;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AlertMe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            ShutdownMode = ShutdownMode.OnMainWindowClose;
            return ServiceLocator.Current.GetInstance<Shell.ShellView>();
        }

        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            regionAdapterMappings.RegisterMapping(typeof(StackPanel), Container.Resolve<StackPanelRegionAdapter>());
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IEventAggregator, EventAggregator>();
            containerRegistry.RegisterSingleton<ILocalDataStore, LocalDataStore>();
            containerRegistry.RegisterSingleton<IDialogService, DialogService>();
            containerRegistry.RegisterInstance(Container.Resolve<AppNotifier>());
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            AddModule(moduleCatalog, typeof(HomeModule));
            AddModule(moduleCatalog, typeof(PlansModule));
            AddModule(moduleCatalog, typeof(AlertSoundSelectorModule));
            AddModule(moduleCatalog, typeof(RainModule));
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        static void AddModule(IModuleCatalog moduleCatalog, Type moduleAuthType)
        {
            moduleCatalog.AddModule(new ModuleInfo(moduleAuthType.Name, moduleAuthType.AssemblyQualifiedName));
        }
    }
}
