using AlertMe.AlertSoundSelector.Events;
using AlertMe.Domain.Commands;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.IO;
using System.Media;

namespace AlertMe.AlertSoundSelector
{
    public class AlertSoundSelectorViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;

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
            EventAggregator.GetEvent<OpenAlertSoundSelector>().Subscribe(OnOpen);
            EventAggregator.GetEvent<AlertSoundSelected>().Subscribe(() => AlertSoundExists = true);
            CloseDialog = new DelegateCommand(() => IsOpen = false);
            PlayCurrentAlertSound = new DelegateCommand(OnPlaySound);
            AlertSoundExists = CheckIfSoundExists();
        }

        void OnOpen(OpenAlertSoundSelectorArgs args)
        {
            IsOpen = true;
            PlanId = args.PlanId;
            AlertId = args.AlertId;
            //load sound if exists
            //
        }

        void OnPlaySound()
        {
            //play sound by name, navigate to folder planId/alertId.mp3 or alertId.wav
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AlertMe/" + PlanId;
            if (File.Exists($"{path}.mp3"))
            {
                var player = new SoundPlayer($"{path}.mp3");
                player.Play();
            }
        }

        bool CheckIfSoundExists()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/AlertMe/" + PlanId;
            if (File.Exists($"{path}.mp3"))
                return true;
            if (File.Exists($"{path}.wav"))
                return true;
            return false;
        }
    }
}
