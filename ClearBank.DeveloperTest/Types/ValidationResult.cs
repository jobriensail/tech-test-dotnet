namespace ClearBank.DeveloperTest.Types;

public sealed class ValidationResult
{
    public bool IsValid { get; }
    public string? FailureReason { get; }
    private ValidationResult(bool isValid, string? reason)
    {
        IsValid = isValid; FailureReason = reason;
    }

    public static ValidationResult Success() => new(true, null);
    public static ValidationResult Fail(string reason) => new(false, reason);
}
