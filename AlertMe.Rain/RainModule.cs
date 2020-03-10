using AlertMe.Domain;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AlertMe.Rain
{
    public class RainModule : IModule
    {
        IRegionManager RegionManager;

        public RainModule(IRegionManager rm)
        {
            RegionManager = rm;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            return;
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegionManager.RegisterViewWithRegion(RegionNames.RainRegion, typeof(OverlayView));
        }
    }
}
