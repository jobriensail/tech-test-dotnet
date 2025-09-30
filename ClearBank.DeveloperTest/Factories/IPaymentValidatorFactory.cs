namespace ClearBank.DeveloperTest.Factories;

public interface IPaymentValidatorFactory
{
    IPaymentValidator GetValidator(PaymentScheme paymentScheme);
}
