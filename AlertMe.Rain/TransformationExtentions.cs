using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace AlertMe.Rain
{
    public static class TransformationExtentions
    {
        public static T GetTransform<T>(this UIElement uie) where T : Transform
        {
            var group = uie.RenderTransform as TransformGroup;
            if (group == null)
            {
                group = new TransformGroup();
                if (uie.RenderTransform != null)
                    group.Children.Add(uie.RenderTransform);

                uie.RenderTransform = group;
            }

            var child = group.Children.FirstOrDefault(x => x.GetType() == typeof(T));
            if (child == null)
            {
                child = Activator.CreateInstance<T>();
                group.Children.Add(child);
            }

            return child as T;
        }
    }
}
