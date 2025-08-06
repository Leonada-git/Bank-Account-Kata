namespace BankAccount
{
    public interface IBankAccount
    {
        void Deposit(double amount);
        void Withdraw(double amount);
        void PrintStatement();
    }
}
