using AlertMe.AlertSoundSelector.Events;
using AlertMe.Domain.Commands;
using AlertMe.Domain.Events;
using NAudio.Wave;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlertMe.AlertSoundSelector
{
    public class RecordNewViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;
        AudioRecorder Recorder;

        public DelegateCommand StartRecording { get; set; }
        public DelegateCommand StopRecording { get; set; }

        float LastPeak { get; set; }

        float currentInputLevel;
        public float CurrentInputLevel
        {
            get => LastPeak * 100;
            set => SetProperty(ref currentInputLevel, value);
        }

        public double MicrophoneLevel
        {
            get { return Recorder.MicrophoneLevel; }
            set { Recorder.MicrophoneLevel = value; }
        }

        public List<string> AvailableMicrophones { get; set; }

        string selectedMicrophone;
        public string SelectedMicrophone
        {
            get => selectedMicrophone;
            set 
            {
                SetProperty(ref selectedMicrophone, value);
                BeginMonitoring();
            }
        }

        string statusText;
        public string StatusText
        {
            get => statusText;
            set => SetProperty(ref statusText, value);
        }

        bool isRecording;
        public bool IsRecording
        {
            get => isRecording;
            set => SetProperty(ref isRecording, value);
        }

        bool isIdle;
        public bool IsIdle
        {
            get => isIdle;
            set => SetProperty(ref isIdle, value);
        }

        string recordedTime;
        public string RecordedTime
        {
            get
            {
                var current = Recorder.RecordedTime;
                return String.Format("{0:D2}:{1:D2}.{2:D3}",
                    current.Minutes, current.Seconds, current.Milliseconds);
            }
            set => SetProperty(ref recordedTime, value);
        }

        bool isPristine;
        public bool IsPristine
        {
            get => isPristine;
            set => SetProperty(ref isPristine, value);
        }

        string AlertId { get; set; }
        string PlanId { get; set; }

        public RecordNewViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            Recorder = new AudioRecorder();
            Recorder.SampleAggregator.MaximumCalculated += OnRecorderMaximumCalculated;
            AvailableMicrophones = new List<string>();
            EventAggregator.GetEvent<OpenAlertSoundSelector>().Subscribe(OnOpen);
            StartRecording = new DelegateCommand(OnStartRecording);
            StopRecording = new DelegateCommand(OnStopRecording);
            EventAggregator.GetEvent<AlertSoundSelectorClosed>().Subscribe(OnClose);
            StatusText = "Record new";
            InitializeMicrophones();
        }

        void OnRecorderMaximumCalculated(object sender, MaxSampleEventArgs e)
        {
            LastPeak = Math.Max(e.MaxSample, Math.Abs(e.MinSample));
            RaisePropertyChanged("CurrentInputLevel");
            RaisePropertyChanged("RecordedTime");
        }

        void OnOpen(OpenAlertSoundSelectorArgs e) 
        { 
            AlertId = e.AlertId; 
            PlanId = e.PlanId;
            IsIdle = true;
            IsPristine = true;
            BeginMonitoring();
        }

        void BeginMonitoring()
        {
            if (string.IsNullOrEmpty(SelectedMicrophone))
            {
                EventAggregator.GetEvent<ApplicationErrorOccured>().Publish(new ApplicationErrorOccuredArgs { Error = "Please select a microphone" });
                return;
            }
            Recorder.BeginMonitoring(AvailableMicrophones.IndexOf(SelectedMicrophone));
        }

        void OnClose()
        {
            if (IsRecording)
                OnStopRecording();
        }

        void OnStartRecording()
        {
            if (string.IsNullOrEmpty(SelectedMicrophone))
            {
                EventAggregator.GetEvent<ApplicationErrorOccured>().Publish(new ApplicationErrorOccuredArgs { Error = "Please select a microphone" });
                return;
            }
            IsRecording = true;
            IsIdle = false;
            StatusText = "Recording";
            var filePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/AlertMe/{PlanId}/{AlertId}";
            Recorder.BeginRecording(filePath);
        }

        async void OnStopRecording()
        {
            Recorder.Stop();
            await TrySaveAsMp3();
            IsRecording = false;
            IsIdle = true;
            IsPristine = false;
            StatusText = "Recorded";
            RaisePropertyChanged("RecordedTime");
            EventAggregator.GetEvent<AlertSoundSelected>().Publish();
        }

        async Task TrySaveAsMp3()
        {
            while (!Recorder.SaveFileAsMp3())
                await Task.Delay(1000);
        }

        void InitializeMicrophones()
        {
            int num = WaveIn.DeviceCount;
            for (int i = 0; i < num; i++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(i);
                AvailableMicrophones.Add(deviceInfo.ProductName);
            }
        }
    }
}
