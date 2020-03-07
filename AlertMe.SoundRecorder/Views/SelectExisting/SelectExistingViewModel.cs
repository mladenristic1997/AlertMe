using AlertMe.AlertSoundSelector.Events;
using AlertMe.Domain.Commands;
using AlertMe.Domain.Events;
using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.IO;

namespace AlertMe.AlertSoundSelector
{
    public class SelectExistingViewModel : BindableBase
    {
        readonly IEventAggregator EventAggregator;

        public DelegateCommand SelectSound { get; set; }

        string AlertId { get; set; }
        string PlanId { get; set; }

        public SelectExistingViewModel(IEventAggregator ea)
        {
            EventAggregator = ea;
            EventAggregator.GetEvent<OpenAlertSoundSelector>().Subscribe(e => { AlertId = e.AlertId; PlanId = e.PlanId; });
            SelectSound = new DelegateCommand(OnSelectSound);
        }

        void OnSelectSound()
        {
            var dialog = new CommonOpenFileDialog();
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                if (!dialog.FileName.Contains(".mp3"))
                {
                    EventAggregator.GetEvent<ApplicationErrorOccured>().Publish(new ApplicationErrorOccuredArgs { Error = "Please select an .mp3 file" });
                    return;
                }
                CopySoundFile(dialog.FileName);
                EventAggregator.GetEvent<AlertSoundSelected>().Publish();
            }
        }

        void CopySoundFile(string filePath)
        {
            var newFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}/AlertMe/{PlanId}/{AlertId}.mp3";
            File.Copy(filePath, newFilePath, true);
        }
    }
}
