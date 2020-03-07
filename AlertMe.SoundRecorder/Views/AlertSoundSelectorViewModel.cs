using AlertMe.AlertSoundSelector.Events;
using AlertMe.Domain.Commands;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.IO;
using System.Media;
using System.Windows.Media;

namespace AlertMe.AlertSoundSelector
{
    public class AlertSoundSelectorViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;
        readonly MediaPlayer Player;

        public DelegateCommand PlayCurrentAlertSound { get; set; }
        public DelegateCommand CloseDialog { get; set; }

        string PlanId { get; set; }
        string AlertId { get; set; }

        bool isOpen;
        public bool IsOpen
        {
            get => isOpen;
            set => SetProperty(ref isOpen, value);
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
            CloseDialog = new DelegateCommand(() => IsOpen = false);
            PlayCurrentAlertSound = new DelegateCommand(OnPlaySound);
        }

        void OnOpen(OpenAlertSoundSelectorArgs args)
        {
            IsOpen = true;
            PlanId = args.PlanId;
            AlertId = args.AlertId;
            if (SoundExists())
                AlertSoundExists = true;
        }

        bool SoundExists()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AlertMe/" + PlanId;
            if (File.Exists($"{path}/{AlertId}.mp3"))
                return true;
            return false;
        }

        void OnPlaySound()
        {
            var path = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/AlertMe/{PlanId}/{AlertId}.mp3";
            Player.Open(new Uri(path));
            Player.Play();
        }
    }
}
