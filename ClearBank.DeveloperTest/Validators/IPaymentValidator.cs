namespace ClearBank.DeveloperTest.Validators;

public interface IPaymentValidator
{
    ValidationResult IsValid(Account account, MakePaymentRequest request);
}
