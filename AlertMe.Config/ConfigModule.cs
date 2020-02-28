using AlertMe.Domain;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AlertMe.Config
{
    public class ConfigModule : IModule
    {
        IRegionManager RegionManager;

        public ConfigModule(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegionManager.RegisterViewWithRegion(RegionNames.ConfigRegion, typeof(ConfigView));
        }
    }
}
