namespace ClearBank.DeveloperTest.Tests.Validators;

public class BacsPaymentValidatorTests
{
    private readonly BacsPaymentValidator _sut = new();

    [Fact]
    public void IsValid_WhenAccountIsNull_ReturnsFalse()
    {
        // Arrange
        var request = new MakePaymentRequest { PaymentScheme = PaymentScheme.Bacs };

        // Act
        var result = _sut.IsValid(null, request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WhenAccountDoesNotAllowBacs_ReturnsFalse()
    {
        // Arrange
        var account = AccountBuilder
            .AnAccount()
            .WithBalance(10000m)
            .WithAllowedPaymentSchemes(AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps);

        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForBacsPayment()
            .WithAmount(100m);

        // Act
        var result = _sut.IsValid(account, request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WhenAccountAllowsBacs_ReturnsTrue()
    {
        // Arrange
        var account = AccountBuilder
            .AnAccount()
            .WithBalance(10000m)
            .WithAllowedPaymentSchemes(AllowedPaymentSchemes.Bacs);

        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForBacsPayment()
            .WithAmount(100m);

        // Act
        var result = _sut.IsValid(account, request);

        // Assert
        result.IsValid.Should().BeTrue();
    }
}