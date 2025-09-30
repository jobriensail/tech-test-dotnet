namespace ClearBank.DeveloperTest.Services.Failures;

public static class PaymentFailureReasons
{
    public const string InvalidRequest = "Request is required.";
    public const string AmountNonPositive = "Amount must be greater than 0.";
    public const string UnsupportedScheme = "Unsupported payment scheme.";

    public const string AccountNotFound = "Account not found.";
    public const string SchemeNotAllowed = "Payment scheme not allowed for this account.";
    public const string InsufficientFunds = "Insufficient funds.";
    public const string AccountNotLive = "Account is not live.";
}
