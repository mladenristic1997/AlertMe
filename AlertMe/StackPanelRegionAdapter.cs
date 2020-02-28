using Prism.Regions;
using System.Windows;
using System.Windows.Controls;

namespace AlertMe
{
    public class StackPanelRegionAdapter : RegionAdapterBase<StackPanel>
    {

        public StackPanelRegionAdapter(IRegionBehaviorFactory regionBehaviorFactory) : base(regionBehaviorFactory)
        {
        }

        protected override void Adapt(IRegion region, StackPanel regionTarget)
        {
            region.Views.CollectionChanged += (s, e) => {
                if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                    foreach (FrameworkElement fe in e.NewItems)
                        regionTarget.Children.Add(fe);
            };
        }

        protected override IRegion CreateRegion()
        {
            return new AllActiveRegion();
        }
    }
}