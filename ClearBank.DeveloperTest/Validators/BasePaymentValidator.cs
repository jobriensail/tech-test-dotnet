namespace ClearBank.DeveloperTest.Validators;

public abstract class BasePaymentValidator : IPaymentValidator
{
    public ValidationResult IsValid(Account account, MakePaymentRequest request)
    {
        if (account == null)
        {
            return ValidationResult.Fail(PaymentFailureReasons.AccountNotFound);
        }

        if (!IsPaymentSchemeAllowed(account))
        {
            return ValidationResult.Fail(PaymentFailureReasons.SchemeNotAllowed);
        }

        return ValidateSpecificRules(account, request);
    }

    protected abstract ValidationResult ValidateSpecificRules(Account account, MakePaymentRequest request);
    protected abstract AllowedPaymentSchemes GetRequiredScheme();

    private bool IsPaymentSchemeAllowed(Account account)
    {
        return account.AllowedPaymentSchemes.HasFlag(GetRequiredScheme());
    }
}