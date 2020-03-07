using NAudio.Lame;
using NAudio.Mixer;
using NAudio.Wave;
using Prism.Mvvm;
using System;
using System.IO;
using System.Linq;

namespace AlertMe.AlertSoundSelector
{
    public class AudioRecorder : BindableBase
    {
        WaveIn WaveIn;
        public SampleAggregator SampleAggregator { get; private set; }
        UnsignedMixerControl volumeControl;
        double desiredVolume = 100;
        WaveFileWriter Writer;
        string FileName { get; set; }

        public AudioRecorder()
        {
            SampleAggregator = new SampleAggregator();
            RecordingFormat = new WaveFormat(44100, 1);
        }

        WaveFormat recordingFormat;
        public WaveFormat RecordingFormat
        {
            get
            {
                return recordingFormat;
            }
            set
            {
                recordingFormat = value;
                SampleAggregator.NotificationCount = value.SampleRate / 10;
            }
        }

        public void BeginMonitoring(int recordingDevice)
        {
            WaveIn = new WaveIn();
            WaveIn.DeviceNumber = recordingDevice;
            WaveIn.DataAvailable += OnDataAvailable;
            WaveIn.RecordingStopped += OnRecordingStopped;
            WaveIn.WaveFormat = RecordingFormat;
            WaveIn.StartRecording();
            TryGetVolumeControl();
        }

        void OnRecordingStopped(object sender, StoppedEventArgs e)
        {
            RecordingState = RecordingState.Stopped;
            Writer.Dispose();
        }

        public void BeginRecording(string fileName)
        {
            FileName = $"{fileName}.wav";
            Writer = new WaveFileWriter(FileName, RecordingFormat);
            RecordingState = RecordingState.Recording;
        }

        public void Stop()
        {
            if (RecordingState == RecordingState.Recording)
            {
                WaveIn.StopRecording();
            }
        }

        public bool SaveFileAsMp3(int bitRate = 128)
        {
            try
            {
                var mp3FileName = FileName.Replace(".wav", ".mp3");
                using (var reader = new AudioFileReader(FileName))
                {
                    using (var writer = new LameMP3FileWriter(mp3FileName, reader.WaveFormat, bitRate))
                    {
                        reader.CopyTo(writer);
                        writer.Flush();
                    }
                }
                var fileInfo = new FileInfo(FileName) { Attributes = FileAttributes.Normal };
                fileInfo.Delete();
                return true;
            }
            catch
            {
                return false;
            }
        }

        void TryGetVolumeControl()
        {
            int waveInDeviceNumber = WaveIn.DeviceNumber;
            if (Environment.OSVersion.Version.Major >= 6) // Vista and over
            {
                var mixerLine = WaveIn.GetMixerLine();
                //new MixerLine((IntPtr)waveInDeviceNumber, 0, MixerFlags.WaveIn);
                foreach (var control in mixerLine.Controls)
                {
                    if (control.ControlType == MixerControlType.Volume)
                    {
                        volumeControl = control as UnsignedMixerControl;
                        MicrophoneLevel = desiredVolume;
                        break;
                    }
                }
            }
            else
            {
                var mixer = new Mixer(waveInDeviceNumber);
                foreach (var destination in mixer.Destinations
                    .Where(d => d.ComponentType == MixerLineComponentType.DestinationWaveIn))
                {
                    foreach (var source in destination.Sources
                        .Where(source => source.ComponentType == MixerLineComponentType.SourceMicrophone))
                    {
                        foreach (var control in source.Controls
                            .Where(control => control.ControlType == MixerControlType.Volume))
                        {
                            volumeControl = control as UnsignedMixerControl;
                            MicrophoneLevel = desiredVolume;
                            break;
                        }
                    }
                }
            }

        }

        public double MicrophoneLevel
        {
            get
            {
                return desiredVolume;
            }
            set
            {
                desiredVolume = value;
                if (volumeControl != null)
                {
                    volumeControl.Percent = value;
                }
            }
        }

        RecordingState RecordingState { get; set; }

        public TimeSpan RecordedTime
        {
            get
            {
                if (Writer == null)
                {
                    return TimeSpan.Zero;
                }
                return TimeSpan.FromSeconds((double)Writer.Length / Writer.WaveFormat.AverageBytesPerSecond);
            }
        }

        void OnDataAvailable(object sender, WaveInEventArgs e)
        {
            byte[] buffer = e.Buffer;
            int bytesRecorded = e.BytesRecorded;
            WriteToFile(buffer, bytesRecorded);

            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((buffer[index + 1] << 8) |
                                        buffer[index + 0]);
                float sample32 = sample / 32768f;
                SampleAggregator.Add(sample32);
            }
        }

        void WriteToFile(byte[] buffer, int bytesRecorded)
        {
            long maxFileLength = RecordingFormat.AverageBytesPerSecond * 60;
            if (RecordingState == RecordingState.Recording
               || RecordingState == RecordingState.RequestedStop)
            {
                var toWrite = (int)Math.Min(maxFileLength - Writer.Length, bytesRecorded);
                if (toWrite > 0)
                {
                    Writer.Write(buffer, 0, bytesRecorded);
                }
                else
                {
                    Stop();
                }
            }
        }
    }

    public enum RecordingState
    {
        Stopped,
        Monitoring,
        Recording,
        RequestedStop
    }
}
