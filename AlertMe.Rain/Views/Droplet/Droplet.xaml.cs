using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlertMe.Rain
{
    /// <summary>
    /// Interaction logic for DropletView.xaml
    /// </summary>
    public partial class Droplet : UserControl
    {
        int AnimationDuration = 2;
        public double ScreenHeight { get; set; }
        public string ImagePath { get; set; }
        public Thickness ImagePosition { get; set; }

        public Droplet()
        {
            InitializeComponent();
        }

        public void Init()
        {
            DropletImage.Source = new BitmapImage(new Uri(ImagePath));
            DropletImage.Margin = ImagePosition;
            DropletImage.UpdateLayout();
        }

        public void BeginAnimation()
        {
            ThicknessAnimation translate = new ThicknessAnimation(ImagePosition, new Thickness(ImagePosition.Left, ScreenHeight, 0, 0), TimeSpan.FromSeconds(AnimationDuration));
            DropletImage.BeginAnimation(MarginProperty, translate);
        }
    }
}
