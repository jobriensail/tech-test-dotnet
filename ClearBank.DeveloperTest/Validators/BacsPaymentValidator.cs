namespace ClearBank.DeveloperTest.Validators;

public class BacsPaymentValidator : BasePaymentValidator
{
    protected override ValidationResult ValidateSpecificRules(Account account, MakePaymentRequest request)
    => account.Balance >= request.Amount
        ? ValidationResult.Success()
        : ValidationResult.Fail(PaymentFailureReasons.InsufficientFunds);
    protected override AllowedPaymentSchemes GetRequiredScheme() => AllowedPaymentSchemes.Bacs;
}
