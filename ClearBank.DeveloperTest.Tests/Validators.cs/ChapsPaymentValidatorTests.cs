namespace ClearBank.DeveloperTest.Tests.Validators;

public class ChapsPaymentValidatorTests
{
    private readonly ChapsPaymentValidator _sut = new();

    [Fact]
    public void IsValid_WhenAccountIsNull_ReturnsFalse()
    {
        // Arrange
        var request = PaymentRequestBuilder
                        .APaymentRequest()
                        .ForChapsPayment();

        // Act
        var result = _sut.IsValid(null, request);

        // Assert
        result.IsValid.Should().BeFalse(PaymentFailureReasons.AccountNotFound);
    }

    [Fact]
    public void IsValid_WhenAccountDoesNotAllowChaps_ReturnsFalse()
    {
        // Arrange
        var account = AccountBuilder
            .AnAccount()
            .WithBalance(10000m)
            .WithAllowedPaymentSchemes(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments);

        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForChapsPayment()
            .WithAmount(100m);

        // Act
        var result = _sut.IsValid(account, request);

        // Assert
        result.IsValid.Should().BeFalse(PaymentFailureReasons.SchemeNotAllowed);
    }

    [Theory]
    [InlineData(AccountStatus.Disabled)]
    [InlineData(AccountStatus.InboundPaymentsOnly)]
    public void IsValid_WhenAccountStatusIsNotLive_ReturnsFalse(AccountStatus status)
    {
        // Arrange
        var account = AccountBuilder
            .AnAccount()
            .WithStatus(status)
            .WithBalance(10000m)
            .WithAllowedPaymentSchemes(AllowedPaymentSchemes.Chaps);

        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForChapsPayment()
            .WithAmount(100m);

        // Act
        var result = _sut.IsValid(account, request);

        // Assert
        result.IsValid.Should().BeFalse(PaymentFailureReasons.AccountNotLive);
    }

    [Fact]
    public void IsValid_WhenAllConditionsMet_ReturnsTrue()
    {
        // Arrange
        var account = AccountBuilder
            .AnAccount()
            .WithBalance(10000m)
            .WithAllowedPaymentSchemes(AllowedPaymentSchemes.Chaps);

        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForChapsPayment()
            .WithAmount(100m);

        // Act
        var result = _sut.IsValid(account, request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}