namespace BankAccount
{
    public class BankAccount(IDateProvider dateProvider) : IBankAccount
    {
        private double balance = 0;
        private readonly IDateProvider dateProvider = dateProvider ?? throw new ArgumentNullException(nameof(dateProvider));

        private readonly List<Statement> _statements = [];
        public IEnumerable<Statement> Statements => _statements;

        //public IDateProvider DateProvider { get; } = dateProvider;

        public double GetBalance()
        {
            return balance;
        }
        public void Deposit(double amount)
        {
            balance += amount;

            AddStatmentToHistory(amount);

        }
        public void Withdraw(double amount)
        {
            if (amount > balance)
            {
                throw new ArgumentException("Balance is insufficient.");
            }
            else
            {
                balance -= amount;
                AddStatmentToHistory(amount);
            }
        }

        private void AddStatmentToHistory(double amount)
        {
            Statement statement = new(amount, dateProvider.Today, balance);

            _statements.Add(statement);
        }

        public void PrintStatement()
        {
            Console.WriteLine("OperationDate         | Amount  | Balance");
            Console.WriteLine("------------------------------------------");
            foreach (var statement in _statements)
            {
                Console.WriteLine($"{statement.OperationDate,-22} | {statement.Amount,7} | {statement.Balance,7}");
            }
        }

    }
}