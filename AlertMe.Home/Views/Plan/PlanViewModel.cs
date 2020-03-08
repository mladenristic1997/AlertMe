using AlertMe.Domain;
using AlertMe.Timeline.AlertCheckpoint;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace AlertMe.Home
{
    public class PlanViewModel : BindableBase
    {
        Thread Progress;
        DialogService DialogService;
        MediaPlayer Player;

        DateTime ReferenceTime;
        DateTime PausedAt;
        DateTime CurrentTime;

        public string Id { get; set; }
        public int PlanDuration { get; set; }

        int secondsPassed;
        public int SecondsPassed
        {
            get => secondsPassed;
            set => SetProperty(ref secondsPassed, value);
        }

        int LastAlertAt { get; set; }

        string nextAlertIn;
        public string NextAlertIn
        {
            get => nextAlertIn;
            set => SetProperty(ref nextAlertIn, value);
        }

        string nextMessage;
        public string NextMessage
        {
            get => nextMessage;
            set => SetProperty(ref nextMessage, value);
        }

        bool isOngoing;
        public bool IsOngoing
        {
            get => isOngoing;
            set => SetProperty(ref isOngoing, value);
        }

        bool isPaused;
        public bool IsPaused
        {
            get => isPaused;
            set => SetProperty(ref isPaused, value);
        }

        Thickness progressMargin;
        public Thickness ProgressMargin
        {
            get => progressMargin;
            set => SetProperty(ref progressMargin, value);
        }

        public DelegateCommand Continue { get; set; }
        public DelegateCommand Pause { get; set; }
        public DelegateCommand Reset { get; set; }

        AlertCheckpointViewModel NextAlert { get; set; }
        public ObservableCollection<AlertCheckpointViewModel> AlertCheckpoints { get; set; }
        public List<int> AlertTimes { get; set; }

        public PlanViewModel(IEventAggregator ea)
        {
            DialogService = new DialogService(ea);
            AlertCheckpoints = new ObservableCollection<AlertCheckpointViewModel>();
            AlertTimes = new List<int>();
            IsPaused = true;
            Continue = new DelegateCommand(OnContinue);
            Pause = new DelegateCommand(OnPause);
            Reset = new DelegateCommand(OnReset);
            Player = new MediaPlayer();
            ReferenceTime = DateTime.Now;
            PausedAt = DateTime.Now; 
            SetupProgressThread();
        }

        void SetupProgressThread()
        {
            Progress = new Thread(new ThreadStart(Countdown));
            Progress.IsBackground = true;
            Progress.SetApartmentState(ApartmentState.STA);
            Progress.Name = "ProgressBarCounterThread";
            Progress.Start();
        }

        void OnContinue()
        {
            (IsOngoing, IsPaused) = (true, false);
        }

        void OnPause()
        {
            (IsPaused, IsOngoing) = (true, false);
            PausedAt = DateTime.Now;
        }

        void OnReset()
        {
            if (MessageBox.Show("Are you sure you want to restart the plan?", "Restart", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.No) == MessageBoxResult.Yes)
                ResetTimeline();
        }

        public void ResetTimeline()
        {
            SecondsPassed = 0;
            (IsPaused, IsOngoing) = (true, false);
            ReferenceTime = DateTime.Now;
            PausedAt = DateTime.Now;
            NextAlert = AlertCheckpoints[0];
            NextMessage = NextAlert.Message;
            NextAlertIn = "";
        }

        void Countdown()
        {
            //if current seconds +-1 from an item of a list, take that item and use its index to access alert checkpoint at that index and set its isPassed to true
            NextAlert = AlertCheckpoints[0];
            NextMessage = NextAlert.Message;
            while (true)
            {
                Task.Delay(1000);
                CurrentTime = DateTime.Now;
                if (IsOngoing)
                {
                    SecondsPassed = CurrentTime.Subtract(ReferenceTime).Seconds;
                    ProgressMargin = CalculateMarginThickness(SecondsPassed);
                    var alertHitKey = AlertHit();
                    if (alertHitKey != -1)
                    {
                        var alert = AlertCheckpoints[alertHitKey];
                        if (alert.IsPassed)
                            continue;
                        alert.IsPassed = true;
                        NextMessage = alert.Message;
                        PlaySound(alert.Id);
                        Application.Current.Dispatcher.Invoke(() => {
                            var v = new MessageBoxView() { DataContext = new MessageBoxViewModel { Message = alert.Message } };
                            DialogService.Show(v);
                        });
                        LastAlertAt = SecondsPassed;
                        NextAlert = AlertCheckpoints[alertHitKey];
                    }
                    NextAlertIn = (NextAlert.TotalSeconds - (SecondsPassed - LastAlertAt)).ToString();
                }
                else
                {
                    var diff = CurrentTime.Subtract(PausedAt);
                    var d = new TimeSpan(diff.Ticks);
                    ReferenceTime.Add(d);
                    int seconds = 0;
                    var time = ReferenceTime;
                    foreach (var a in AlertCheckpoints)
                    {
                        seconds += a.TotalSeconds;
                        a.AlertAt = time.Add(new TimeSpan(0, 0, seconds)).ToShortTimeString();
                    }
                }
            }
        }

            int AlertHit()
            {
                if (AlertTimes.IndexOf(SecondsPassed) != -1)
                    return AlertTimes.IndexOf(SecondsPassed);
                return -1;
            }

        Thickness CalculateMarginThickness(int time) => new Thickness(CalculateMargin(time) + 14, 0, 14, 0);
        double CalculateMargin(int time) => Math.Round(750.0 * time / PlanDuration, 2);

        void PlaySound(string alertId)
        {
            if (!SoundExists(alertId))
                return;
            var path = GetSoundPathFile(alertId);
            Player.Open(new Uri(path));
            Player.Play();
        }

        bool SoundExists(string alertId)
        {
            var path = GetSoundPathFile(alertId);
            if (File.Exists(path))
                return true;
            return false;
        }

        string GetSoundPathFile(string alertId) => $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/AlertMe/{Id}/{alertId}.mp3";
    }
}
