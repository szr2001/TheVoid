namespace TheVoid.ViewModels
{
    public class VoidInteractionsVM
    {
        public bool CanWrite { get; set; }
        public bool CanRead { get; set; }

        public TimeSpan WriteDelay { get; set; }
        public TimeSpan ReadDelay { get; set; }

        public int TotalMessagesRead { get; set; }
        public int TotalMessagesWrite { get; set; }
    }
}
