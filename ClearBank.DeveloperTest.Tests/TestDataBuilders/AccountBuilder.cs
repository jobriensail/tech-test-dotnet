using System;
using Bogus;

namespace ClearBank.DeveloperTest.Tests.TestDataBuilders;

public class AccountBuilder
{
    private readonly Account _account;
    private static readonly Faker _faker = new();

    public AccountBuilder()
    {
        _account = new Account
        {
            AccountNumber = _faker.Finance.Account(),
            Balance = Math.Round(_faker.Finance.Amount(1000, 50000), 2),
            Status = AccountStatus.Live,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs |
                                   AllowedPaymentSchemes.FasterPayments |
                                   AllowedPaymentSchemes.Chaps
        };
    }

    public static AccountBuilder AnAccount() => new();

    public AccountBuilder WithBalance(decimal balance)
    {
        _account.Balance = balance;
        return this;
    }

    public AccountBuilder WithAccountNumber(string accountNumber)
    {
        _account.AccountNumber = accountNumber;
        return this;
    }

    public AccountBuilder WithStatus(AccountStatus status)
    {
        _account.Status = status;
        return this;
    }

    public AccountBuilder WithAllowedPaymentSchemes(AllowedPaymentSchemes schemes)
    {
        _account.AllowedPaymentSchemes = schemes;
        return this;
    }

    public AccountBuilder ThatAllowsOnlyBacs()
    {
        _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs;
        return this;
    }

    public AccountBuilder ThatAllowsOnlyFasterPayments()
    {
        _account.AllowedPaymentSchemes = AllowedPaymentSchemes.FasterPayments;
        return this;
    }

    public AccountBuilder ThatAllowsOnlyChaps()
    {
        _account.AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps;
        return this;
    }

    public AccountBuilder ThatIsDisabled()
    {
        _account.Status = AccountStatus.Disabled;
        return this;
    }

    public AccountBuilder ThatIsInboundOnly()
    {
        _account.Status = AccountStatus.InboundPaymentsOnly;
        return this;
    }

    public AccountBuilder WithInsufficientFunds()
    {
        _account.Balance = 0.01m;
        return this;
    }

    public Account Build() => _account;

    public static implicit operator Account(AccountBuilder builder) => builder.Build();
}