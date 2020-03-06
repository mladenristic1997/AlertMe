namespace AlertMe.Timeline
{
    public static class TimelineDesignTime
    {
        public static TimelineViewModel ViewModel
        {
            get => new TimelineViewModel
            {
                ControlWidth = 800,
                PlanDuration = 150,
                AlertCheckpoints = new System.Collections.ObjectModel.ObservableCollection<AlertCheckpoint.AlertCheckpointViewModel>
                {
                    new AlertCheckpoint.AlertCheckpointViewModel
                    {
                        Margin = new System.Windows.Thickness(100, 0, 0, 0),
                        Message = "jebije",
                        AlertAt = "12:00",
                        Id = "1"
                    },
                    new AlertCheckpoint.AlertCheckpointViewModel
                    {
                        Margin = new System.Windows.Thickness(400, 0, 0, 0),
                        Message = "jebije",
                        AlertAt = "12:40",
                        Id = "2"
                    },
                }
            };
        }
    }
}
