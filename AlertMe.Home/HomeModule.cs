using AlertMe.Domain;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AlertMe.Home
{
    public class HomeModule : IModule
    {
        IRegionManager RegionManager;

        public HomeModule(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegionManager.RegisterViewWithRegion(RegionNames.HomeRegion, typeof(HomeView));
        }
    }
}
