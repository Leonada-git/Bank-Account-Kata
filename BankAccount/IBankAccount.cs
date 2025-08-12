namespace BankAccount
{
    public interface IBankAccount
    {
        void Deposit(int amount);
        void Withdraw(int amount);
        void PrintStatement();
    }
}
