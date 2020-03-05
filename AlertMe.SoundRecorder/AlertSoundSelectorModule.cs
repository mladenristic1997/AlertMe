using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace AlertMe.AlertSoundSelector
{
    public class AlertSoundSelectorModule : IModule
    {
        readonly IRegionManager RegionManager;

        public AlertSoundSelectorModule(IRegionManager regionManager)
        {
            RegionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            RegionManager.RegisterViewWithRegion(AlertSoundSelectorRegionNames.RecordNewRegion, typeof(RecordNewView));
            RegionManager.RegisterViewWithRegion(AlertSoundSelectorRegionNames.SelectExistingRegion, typeof(SelectExistingView));
        }
    }
}
