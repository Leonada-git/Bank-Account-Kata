using BankAccount;
using FluentAssertions;

namespace BankAccount.Test
{
    public class BankAccountTests
    {
        private readonly BankAccount sut;
        private readonly IDateProvider dateProvider;

        public BankAccountTests()
        {
            dateProvider = new DateProvider();

            sut = new BankAccount(dateProvider);
        }


        [Fact]

        public void Balance_is_zero_upon_account_creation()
        {
            var sut = new BankAccount(dateProvider);

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
        public void Throws_when_negative_amount_is_passed()
        {
            var action = () => sut.Deposit(-100);

            action.Should().Throw<ArgumentOutOfRangeException>("Negative amounts are not allowed.");
        }

        [Fact]
        public void Throws_When_balance_is_insufficient()
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
            sut.Deposit(100);

            sut.Statements.Should().ContainSingle();
        }

        [Fact]
        public void Statement_is_not_register_for_failed_operation()
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
        public void Statement_is_registered_in_history()
        {
            GivenTodayIs(new(2023, 8, 5));

            sut.Deposit(100);

            var statement = new Statement(Amount: 100, OperationDate: new(2023, 8, 5), Balance: 100);
            sut.Statements.Single().Should().Be(statement);
        }

        private void GivenTodayIs(DateOnly dateOnly)
        {
            ((DateProvider)dateProvider).Today = dateOnly;
        }
    }
}

public class DateProvider : IDateProvider
{
    public DateOnly Today { get; set; } = new();
}
