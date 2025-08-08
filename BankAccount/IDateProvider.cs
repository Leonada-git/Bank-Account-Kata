namespace BankAccount
{
    public interface IDateProvider
    {
        DateOnly Today { get; }
    }
}
