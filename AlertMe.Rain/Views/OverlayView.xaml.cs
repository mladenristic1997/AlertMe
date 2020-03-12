using AlertMe.Domain.Commands;
using Prism.Events;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AlertMe.Rain
{
    /// <summary>
    /// Interaction logic for OverlayView.xaml
    /// </summary>
    public partial class OverlayView : UserControl
    {
        MediaPlayer Player;
        Random Random;
        List<Droplet> Droplets = new List<Droplet>();
        TimeSpan FrameDuration = new TimeSpan(0, 0, 0, 0, 16);
        bool AnimationInProgress;

        public OverlayView()
        {
            Player = new MediaPlayer();
            Random = new Random();
            Player.MediaEnded += OnMediaEnded;
            InitializeComponent();
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            TriggerRain();
        }

        async Task TriggerRain()
        {
            if (AnimationInProgress)
                return;
            MakeItRainBtn.Visibility = Visibility.Collapsed;
            AnimationInProgress = true;
            Player.Open(new Uri(Environment.CurrentDirectory + "/Resources/rainingmen.mp3", UriKind.Relative));
            Player.Play();
            Droplets = new List<Droplet>();
            int generationCounter = 0;
            while (AnimationInProgress)
            {
                if (generationCounter % 4 == 0)
                    AddNewDroplets();
                generationCounter++;
                await Task.Delay(FrameDuration);
            }
        }

        void AddNewDroplets()
        {
            for (int i = 0; i < Overlay.ActualWidth / 300; i++)
            {
                var x = Math.Round(Random.NextDouble() * Overlay.ActualWidth, 2);
                var margin = new Thickness(x, 0, 0, 0);
                var imagePath = Random.Next(0, 2) == 0 ? "pack://application:,,,/AlertMe.Rain;component/Resources/heart.png" : "pack://application:,,,/AlertMe.Rain;component/Resources/mladen.png";
                var droplet = new Droplet
                {
                    ImagePosition = margin,
                    ImagePath = imagePath,
                    ScreenHeight = Overlay.ActualHeight
                };
                droplet.Init();
                Droplets.Add(droplet);
                Overlay.Children.Add(droplet);
                droplet.BeginAnimation();
            }
        }

        async void OnMediaEnded(object o, EventArgs e)
        {
            AnimationInProgress = false; 
            Droplets.Clear();
            await Task.Delay(2000);
            Overlay.Children.Clear();
            MakeItRainBtn.Visibility = Visibility.Visible;
        }
    }
}
