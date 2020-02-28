namespace AlertMe.Domain
{
    public class Alert
    {
        public string Id { get; set; }
        public int Hours { get; set; }
        public int Minutes { get; set; }
        public int Seconds { get; set; }
        public string Message { get; set; }
        public AlertType AlertType { get; set; }

        public int CalculateInSeconds() => Hours * 60 * 60 + Minutes * 60 + Seconds;
    }
}
