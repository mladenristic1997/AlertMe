namespace AlertMe.Timeline
{
    public class Alert : BindableBase
    {
        string id;
        public string Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        int totalSeconds;
        public int TotalSeconds
        {
            get => totalSeconds;
            set => SetProperty(ref totalSeconds, value);
        }
    }
}
