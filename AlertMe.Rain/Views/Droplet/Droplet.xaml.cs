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
        public double RotationAngle { get; set; }
        public double FallingSpeed { get; set; }
        public RotationOrientation RotationOrientation { get; set; }
        public string ImagePath { get; set; }

        public Droplet()
        {
            InitializeComponent();
        }

        public void UpdatePosition()
        {
            var currentY = Margin.Top;
            var currentX = Margin.Left;
            Margin = new Thickness(currentX, currentY + FallingSpeed, 0, 0);
            //RotateTransform currentAngle = DropletImage.RenderTransform as RotateTransform;
            //if (RotationOrientation == RotationOrientation.Clockwise)
            //{
            //    var rotateAnimation = new DoubleAnimation(currentAngle.Angle, currentAngle.Angle + RotationAngle, new TimeSpan(0, 0, 0, 0, 16));
            //    var rt = (RotateTransform)DropletImage.RenderTransform;
            //    rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
            //}
            //else
            //{
            //    var rotateAnimation = new DoubleAnimation(currentAngle.Angle, currentAngle.Angle - RotationAngle, new TimeSpan(0, 0, 0, 0, 16));
            //    var rt = (RotateTransform)DropletImage.RenderTransform;
            //    rt.BeginAnimation(RotateTransform.AngleProperty, rotateAnimation);
            //}
        }
    }

    public enum RotationOrientation
    {
        Clockwise,
        CounterClockwise
    }
}
