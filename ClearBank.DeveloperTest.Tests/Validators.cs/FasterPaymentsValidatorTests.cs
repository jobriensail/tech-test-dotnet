namespace ClearBank.DeveloperTest.Tests.Validators;

public class FasterPaymentsValidatorTests
{
    private readonly FasterPaymentsValidator _sut = new();

    [Fact]
    public void IsValid_WhenAccountIsNull_ShouldReturnFalse()
    {
        // Arrange
        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForFasterPayment();

        // Act
        var result = _sut.IsValid(null, request);

        // Assert
        result.IsValid.Should().BeFalse(PaymentFailureReasons.AccountNotFound);
    }

    [Fact]
    public void IsValid_WhenSchemeNotAllowed_ShouldReturnFalse()
    {
        // Arrange
        var account = AccountBuilder
            .AnAccount()
            .WithBalance(10000m)
            .WithAllowedPaymentSchemes(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.Chaps);

        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForFasterPayment()
            .WithAmount(100m);

        // Act
        var result = _sut.IsValid(account, request);

        // Assert
        result.IsValid.Should().BeFalse(PaymentFailureReasons.UnsupportedScheme);
    }
}