using AlertMe.Config;
using AlertMe.Domain;
using CommonServiceLocator;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            AddModule(moduleCatalog, typeof(ConfigModule));
            base.ConfigureModuleCatalog(moduleCatalog);
        }

        static void AddModule(IModuleCatalog moduleCatalog, Type moduleAuthType)
        {
            moduleCatalog.AddModule(new ModuleInfo(moduleAuthType.Name, moduleAuthType.AssemblyQualifiedName));
        }
    }
}
