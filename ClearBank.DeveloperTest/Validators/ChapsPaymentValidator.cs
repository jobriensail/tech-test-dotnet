namespace ClearBank.DeveloperTest.Validators;

public class ChapsPaymentValidator : BasePaymentValidator
{
    protected override ValidationResult ValidateSpecificRules(Account account, MakePaymentRequest request)
        => account.Status == AccountStatus.Live
            ? ValidationResult.Success()
            : ValidationResult.Fail(PaymentFailureReasons.AccountNotLive);

    protected override AllowedPaymentSchemes GetRequiredScheme() => AllowedPaymentSchemes.Chaps;
}
