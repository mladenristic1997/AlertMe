using AlertMe.Domain;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AlertMe.Plans
{
    public class PlansModule : IModule
    {
        IRegionManager RegionManager;

        public PlansModule(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegionManager.RegisterViewWithRegion(RegionNames.PlansRegion, typeof(PlansView));
        }
    }
}
