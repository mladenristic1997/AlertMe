﻿
using AlertMe.Timeline.AlertCheckpoint;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace AlertMe.Timeline
{
    public class ProgressViewModel : BindableBase
    {
        Thread Counter;
        DialogService DialogService;

        public DelegateCommand Continue { get; set; }
        public DelegateCommand Pause { get; set; }

        bool isPaused;
        public bool IsPaused
        {
            get => isPaused;
            set => SetProperty(ref isPaused, value);
        }

        bool isOngoing;
        public bool IsOngoing
        {
            get => isOngoing;
            set => SetProperty(ref isOngoing, value);
        }

        DateTime ReferenceTime;
        DateTime PausedAt;
        DateTime CurrentTime;
        
        int planDuration;
        public int PlanDuration
        {
            get => planDuration;
            set
            {
                SetProperty(ref planDuration, value);
                UpdateView();
            }
        }

        int secondsPassed;
        public int SecondsPassed
        {
            get => secondsPassed;
            set
            {
                SetProperty(ref secondsPassed, value);
                UpdateView();
            }
        }

        Thickness progressMargin;
        public Thickness ProgressMargin
        {
            get => progressMargin;
            set => SetProperty(ref progressMargin, value);
        }

        double controlWidth;
        public double ControlWidth
        {
            get => controlWidth;
            set
            {
                SetProperty(ref controlWidth, value);
                UpdateView();
            }
        }

        public ObservableCollection<Alert> Alerts { get; set; }

        public ObservableCollection<AlertCheckpointViewModel> AlertCheckpoints { get; set; }
        public Dictionary<int, Alert> AlertTimesInSeconds { get; set; }

        string nextMessage;
        public string NextMessage
        {
            get => nextMessage;
            set => SetProperty(ref nextMessage, value);
        }

        string nextAlertIn;
        public string NextAlertIn
        {
            get => nextAlertIn;
            set => SetProperty(ref nextAlertIn, value);
        }

        int LastAlertAt { get; set; }

        public ProgressViewModel()
        {
            Alerts = new ObservableCollection<Alert>();
            AlertCheckpoints = new ObservableCollection<AlertCheckpointViewModel>();
            AlertTimesInSeconds = new Dictionary<int, Alert>();
            DialogService = new DialogService();
            Continue = new DelegateCommand(OnContinue);
            Pause = new DelegateCommand(OnPause);
            ReferenceTime = DateTime.Now;
            PausedAt = DateTime.Now;
            Counter = new Thread(new ThreadStart(Countdown));
            Counter.IsBackground = true;
            Counter.SetApartmentState(ApartmentState.STA);
            Counter.Name = "ProgressBarCounterThread";
            Counter.Start();
        }

        void OnContinue()
        {
            IsPaused = false;
            IsOngoing = true;
        }

        void OnPause()
        {
            PausedAt = DateTime.Now;
            IsPaused = true;
            IsOngoing = false;
        }

        public void UpdateView()
        {
            AlertCheckpoints = null;
            foreach (var a in Alerts)
                a.Update = null;
            var list = new ObservableCollection<AlertCheckpointViewModel>();
            DateTime now = ReferenceTime;
            foreach (var a in Alerts)
            {
                now.AddSeconds(a.TotalSeconds);
                var vm = new AlertCheckpoint.AlertCheckpointViewModel();
                vm.Id = a.Id;
                vm.Message = a.Message;
                vm.AlertAt = now.ToShortTimeString();
                vm.LeftMargin = CalculateMargin(a.TotalSeconds);
                list.Add(vm);
            }
            AlertCheckpoints = list;
            ProgressMargin = CalculateMarginThickness(SecondsPassed);
        }

        public void UpdateNext()
        {
            if (AlertTimesInSeconds.Count > 0)
            {
                var min = AlertTimesInSeconds.Keys.Min();
                var entry = AlertTimesInSeconds[min];
                NextMessage = entry.Message;
                NextAlertIn = (entry.TotalSeconds - (SecondsPassed - LastAlertAt)).ToString();
            }
        }

        double CalculateMargin(int time) => Math.Round((ControlWidth * time / PlanDuration), 2);
        Thickness CalculateMarginThickness(int time) => new Thickness(CalculateMargin(time), 0, 0, 0);

        public void AssignAlertTimes()
        {
            AlertTimesInSeconds = new Dictionary<int, Alert>();
            int secs = 0;
            foreach (var a in Alerts)
            {
                secs += a.TotalSeconds;
                AlertTimesInSeconds.Add(secs, a);
            }
        }

        void Countdown()
        {
            while (true)
            {
                CurrentTime = DateTime.Now;
                if (IsOngoing)
                {
                    SecondsPassed = CurrentTime.Subtract(ReferenceTime).Seconds;
                    ProgressMargin = CalculateMarginThickness(SecondsPassed);
                    var alertHitKey = AlertHit();
                    if (alertHitKey != -1)
                    {
                        var alert = AlertTimesInSeconds[alertHitKey];
                        if (alert.IsPassed)
                            continue;
                        alert.IsPassed = true;
                        //play sound from path contained in alert
                        var v = new MessageBoxView() { DataContext = new MessageBoxViewModel { Message = alert.Message } };
                        DialogService.Show(v);
                        AlertTimesInSeconds.Remove(alertHitKey);
                        LastAlertAt = SecondsPassed;
                    }
                }
                else
                {
                    var diff = PausedAt.Subtract(ReferenceTime);
                    var d = new TimeSpan(diff.Ticks);
                    ReferenceTime.Add(d);
                    UpdateView();
                }
                UpdateNext();
                Task.Delay(980);
            }
        }

            int AlertHit()
            {
                var delta = 1;
                if (AlertTimesInSeconds.ContainsKey(SecondsPassed + delta))
                    return SecondsPassed + delta;
                if (AlertTimesInSeconds.ContainsKey(SecondsPassed))
                    return SecondsPassed;
                if (AlertTimesInSeconds.ContainsKey(SecondsPassed - delta))
                    return SecondsPassed - delta;
                return -1;
            }
    }
}
