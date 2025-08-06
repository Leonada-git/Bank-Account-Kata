using FluentAssertions;

namespace BankAccount.Test
{
    public class BankAccountTests
    {
        private readonly BankAccount sut;
        private readonly Clock clock;

        public BankAccountTests()
        {
            clock = new Clock
            {
                Now = DateTime.Now,
            };

            sut = new BankAccount(clock);

        }

        [Fact]
        public void Balance_is_zero_upon_account_creation()
        {
            var sut = new BankAccount(clock);

            var balance = sut.GetBalance();

            balance.Should().Be(0);
        }

        [Fact]
        public void Deposit_increases_balance()
        {
            sut.Deposit(100);

            double NewBalance = sut.GetBalance();
            NewBalance.Should().Be(100);
        }

        [Fact]
        public void Withdrawal_fails_if_balance_is_insufficient()
        {
            sut.Deposit(100);

            Action act = () => sut.Withdraw(110);

            act.Should().Throw<ArgumentException>()
               .WithMessage("Balance is insufficient.");
        }

        [Fact]
        public void Withdrawal_decreases_balance()
        {
            sut.Deposit(100);

            sut.Withdraw(50);

            double newBalance = sut.GetBalance();
            newBalance.Should().Be(50);
        }

        [Fact]
        public void Account_has_no_statement_upon_creation()
        {
            sut.Statements.Should().BeEmpty();
        }

        [Fact]
        public void Registers_a_statement_after_operation()
        {
            const double amount = 100;

            sut.Deposit(amount);

            sut.Statements.Should().ContainSingle();
        }

        [Fact]
        public void Does_not_register_a_statement_for_failed_operation()
        {

            try
            {
                sut.Withdraw(100);
                Assert.Fail("Withdrawal should have failed.");
            }
            catch (ArgumentException) { }

            sut.Statements.Should().BeEmpty();

        }

        [Fact]
        public void Amount_is_registered_in_history()
        {
            sut.Deposit(100);

            Statement Statment = sut.Statements.Single();
            Statment.Amount.Should().Be(100);
        }

        [Fact]
        public void Balance_is_registered_in_history()
        {
            sut.Deposit(100);

            Statement Statment = sut.Statements.Single();
            Statment.Balance.Should().Be(100);
        }

        [Fact]
        public void Date_is_registered_in_history()
        {
            var operationDate = clock.Now;

            sut.Deposit(100);

            Statement Statment = sut.Statements.Single();
            Statment.OperationDate.Should().Be(operationDate);
        }
    }
}
