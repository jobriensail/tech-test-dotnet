using System;
using Bogus;

namespace ClearBank.DeveloperTest.Tests.TestDataBuilders;

public class PaymentRequestBuilder
{
    private readonly Faker<MakePaymentRequest> _faker;
    private MakePaymentRequest _request;

    public PaymentRequestBuilder()
    {
        _faker = new Faker<MakePaymentRequest>()
            .RuleFor(x => x.DebtorAccountNumber, f => f.Finance.Account())
            .RuleFor(x => x.CreditorAccountNumber, f => f.Finance.Account())
            .RuleFor(x => x.Amount, f => Math.Round(f.Finance.Amount(100, 10000), 2))
            .RuleFor(x => x.PaymentDate, f => f.Date.Soon())
            .RuleFor(x => x.PaymentScheme, f => f.PickRandom<PaymentScheme>());

        _request = _faker.Generate();
    }

    public static PaymentRequestBuilder APaymentRequest() => new();

    public PaymentRequestBuilder WithAmount(decimal amount)
    {
        _request.Amount = amount;
        return this;
    }

    public PaymentRequestBuilder WithPaymentScheme(PaymentScheme scheme)
    {
        _request.PaymentScheme = scheme;
        return this;
    }

    public PaymentRequestBuilder WithDebtorAccount(string accountNumber)
    {
        _request.DebtorAccountNumber = accountNumber;
        return this;
    }

    public PaymentRequestBuilder ForBacsPayment()
    {
        _request.PaymentScheme = PaymentScheme.Bacs;
        return this;
    }

    public PaymentRequestBuilder ForFasterPayment()
    {
        _request.PaymentScheme = PaymentScheme.FasterPayments;
        return this;
    }

    public PaymentRequestBuilder ForChapsPayment()
    {
        _request.PaymentScheme = PaymentScheme.Chaps;
        return this;
    }

    public MakePaymentRequest Build() => _request;

    public static implicit operator MakePaymentRequest(PaymentRequestBuilder builder) => builder.Build();
}