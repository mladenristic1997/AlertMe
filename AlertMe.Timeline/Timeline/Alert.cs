using System;

namespace AlertMe.Timeline
{
    public class Alert : BindableBase
    {
        public Action Update;

        string id;
        public string Id
        {
            get => id;
            set
            {
                SetProperty(ref id, value);
                Update?.Invoke();
            }
        }

        string message;
        public string Message
        {
            get => message;
            set
            {
                SetProperty(ref message, value);
                Update?.Invoke();
            }
        }

        int totalSeconds;
        public int TotalSeconds
        {
            get => totalSeconds;
            set
            {
                SetProperty(ref totalSeconds, value);
                Update?.Invoke();
            }
        }
    }
}
