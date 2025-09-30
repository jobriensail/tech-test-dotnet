namespace ClearBank.DeveloperTest.Validators;

public class FasterPaymentsValidator : BasePaymentValidator
{
    protected override ValidationResult ValidateSpecificRules(Account account, MakePaymentRequest request)
    {
        return ValidationResult.Success();
    }

    protected override AllowedPaymentSchemes GetRequiredScheme() => AllowedPaymentSchemes.FasterPayments;
}
