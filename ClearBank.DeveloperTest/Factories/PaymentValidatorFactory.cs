using System;

namespace ClearBank.DeveloperTest.Factories;

public class PaymentValidatorFactory : IPaymentValidatorFactory
{
    public IPaymentValidator GetValidator(PaymentScheme paymentScheme)
    {
        return paymentScheme switch
        {
            PaymentScheme.Bacs => new BacsPaymentValidator(),
            PaymentScheme.FasterPayments => new FasterPaymentsValidator(),
            PaymentScheme.Chaps => new ChapsPaymentValidator(),
            _ => throw new NotSupportedException($"Payment scheme {paymentScheme} is not supported")
        };
    }
}
