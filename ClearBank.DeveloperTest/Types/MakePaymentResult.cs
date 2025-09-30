namespace ClearBank.DeveloperTest.Types;

public sealed class MakePaymentResult
{
    public bool Success { get; }
    public string? FailureReason { get; }
    private MakePaymentResult(bool isValid, string? reason)
    {
        Success = isValid; FailureReason = reason;
    }

    public static MakePaymentResult Succeeded() => new(true, null);
    public static MakePaymentResult Failed(string reason) => new(false, reason);
}
