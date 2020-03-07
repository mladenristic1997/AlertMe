using AlertMe.AlertSoundSelector.Events;
using AlertMe.Domain.Commands;
using AlertMe.Domain.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.IO;
using System.Windows.Media;

namespace AlertMe.AlertSoundSelector
{
    public class AlertSoundSelectorViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;
        readonly MediaPlayer Player;

        public DelegateCommand PlayCurrentAlertSound { get; set; }
        public DelegateCommand StopPlayingCurrentAlertSound { get; set; }
        public DelegateCommand CloseDialog { get; set; }

        string PlanId { get; set; }
        string AlertId { get; set; }

        bool isOpen;
        public bool IsOpen
        {
            get => isOpen;
            set => SetProperty(ref isOpen, value);
        }

        bool isPlaying;
        public bool IsPlaying
        {
            get => isPlaying;
            set => SetProperty(ref isPlaying, value);
        }

        bool isStopped;
        public bool IsStopped
        {
            get => isStopped;
            set => SetProperty(ref isStopped, value);
        }

        bool alertSoundExists;
        public bool AlertSoundExists
        {
            get => alertSoundExists;
            set => SetProperty(ref alertSoundExists, value);
        }

        public AlertSoundSelectorViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            Player = new MediaPlayer();
            EventAggregator.GetEvent<OpenAlertSoundSelector>().Subscribe(OnOpen);
            EventAggregator.GetEvent<AlertSoundSelected>().Subscribe(() => AlertSoundExists = true);
            CloseDialog = new DelegateCommand(OnClose);
            PlayCurrentAlertSound = new DelegateCommand(OnPlaySound);
            StopPlayingCurrentAlertSound = new DelegateCommand(OnStopPlaying);
        }

        void OnOpen(OpenAlertSoundSelectorArgs args)
        {
            IsOpen = true;
            PlanId = args.PlanId;
            AlertId = args.AlertId;
            if (SoundExists())
            {
                AlertSoundExists = true;
                IsStopped = true;
            }
        }

        bool SoundExists()
        {
            var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/AlertMe/{PlanId}/{AlertId}.mp3";
            if (File.Exists(path))
                return true;
            return false;
        }

        void OnPlaySound()
        {
            try
            {
                var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/AlertMe/{PlanId}/{AlertId}.mp3";
                Player.Open(new Uri(path));
                IsPlaying = true;
                IsStopped = false;
                Player.Play();
            }
            catch
            {
                EventAggregator.GetEvent<ApplicationErrorOccured>().Publish(new ApplicationErrorOccuredArgs { Error = "Unable to play the sound file" });
            }
        }

        void OnStopPlaying()
        {
            IsPlaying = false;
            IsStopped = true;
            Player.Stop();
        }

        void OnClose() 
        {
            Player.Stop();
            IsPlaying = false;
            IsStopped = true;
            IsOpen = false; 
            AlertSoundExists = false; 
        }
    }
}
