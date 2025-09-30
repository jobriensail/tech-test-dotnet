using System;

namespace ClearBank.DeveloperTest.Services;

public class PaymentService : IPaymentService
{
    private readonly IDataStoreFactory _dataStoreFactory;
    private readonly IPaymentValidatorFactory _paymentValidatorFactory;

    public PaymentService(IDataStoreFactory dataStoreFactory,
                          IPaymentValidatorFactory paymentValidatorFactory)
    {
        _dataStoreFactory = dataStoreFactory ?? throw new ArgumentNullException(nameof(dataStoreFactory));
        _paymentValidatorFactory = paymentValidatorFactory ?? throw new ArgumentNullException(nameof(paymentValidatorFactory));
    }

    public MakePaymentResult MakePayment(MakePaymentRequest request)
    {

        if (request == null)
        {
            return MakePaymentResult.Failed(PaymentFailureReasons.InvalidRequest);
        }

        if (request.Amount <= 0)
        {
            return MakePaymentResult.Failed(PaymentFailureReasons.AmountNonPositive);
        }

        var dataStore = _dataStoreFactory.CreateDataStore();

        var account = dataStore.GetAccount(request.DebtorAccountNumber);

        var validator = _paymentValidatorFactory.GetValidator(request.PaymentScheme);

        var validationResult = validator.IsValid(account, request);

        if (!validationResult.IsValid)
        {
            return MakePaymentResult.Failed(validationResult.FailureReason);
        }

        account.Balance -= request.Amount;
        dataStore.UpdateAccount(account);

        return MakePaymentResult.Succeeded();
    }
}
