namespace BankAccount
{
    public interface IClock
    {
        DateTime Now { get; }
    }

    public class Clock : IClock
    {
        public DateTime Now { get; set; }
    }
}
