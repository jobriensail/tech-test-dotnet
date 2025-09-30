namespace ClearBank.DeveloperTest.Tests.Validators;

public class BasePaymentValidatorTests
{
    private class TestValidator : BasePaymentValidator
    {
        public ValidationResult SpecificRulesReturn { get; set; } = ValidationResult.Success();

        protected override ValidationResult ValidateSpecificRules(Account account, MakePaymentRequest request)
        {
            return SpecificRulesReturn;
        }

        protected override AllowedPaymentSchemes GetRequiredScheme() => AllowedPaymentSchemes.Bacs;
    }

    [Fact]
    public void IsValid_WhenAccountIsNull_ReturnsFalse()
    {
        // Arrange
        var validator = new TestValidator();

        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForBacsPayment();

        // Act
        var result = validator.IsValid(null, request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WhenPaymentSchemeNotAllowed_ReturnsFalse()
    {
        // Arrange
        var validator = new TestValidator();

        var account = AccountBuilder
            .AnAccount()
            .WithBalance(10000m)
            .WithAllowedPaymentSchemes(AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps);

        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForBacsPayment()
            .WithAmount(100m);

        // Act
        var result = validator.IsValid(account, request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WhenCommonValidationPassesButSpecificFails_ReturnsFalse()
    {
        // Arrange
        var validator = new TestValidator { SpecificRulesReturn = ValidationResult.Fail(PaymentFailureReasons.AccountNotFound)};
        // Arrange
        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForBacsPayment()
            .WithAmount(100m);

        // Act
        var result = validator.IsValid(null, request);

        // Assert
        result.IsValid.Should().BeFalse();
    }

    [Fact]
    public void IsValid_WhenAllValidationPasses_ReturnsTrue()
    {
        // Arrange
        var validator = new TestValidator { SpecificRulesReturn = ValidationResult.Success() };
        // Arrange
        var account = AccountBuilder
            .AnAccount()
            .WithAllowedPaymentSchemes(AllowedPaymentSchemes.Bacs);

        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForBacsPayment();

        // Act
        var result = validator.IsValid(account, request);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [InlineData(AllowedPaymentSchemes.Bacs, true)]
    [InlineData(AllowedPaymentSchemes.Bacs | AllowedPaymentSchemes.FasterPayments, true)]
    [InlineData(AllowedPaymentSchemes.FasterPayments, false)]
    [InlineData(AllowedPaymentSchemes.Chaps, false)]
    [InlineData(AllowedPaymentSchemes.FasterPayments | AllowedPaymentSchemes.Chaps, false)]
    public void IsValid_WithVariousAllowedSchemes_ValidatesCorrectly(
        AllowedPaymentSchemes allowedSchemes, bool expectedResult)
    {
        // Arrange
        var validator = new TestValidator();

        var account = AccountBuilder
            .AnAccount()
            .WithBalance(10000m)
            .WithAllowedPaymentSchemes(allowedSchemes);

        var request = PaymentRequestBuilder
            .APaymentRequest()
            .ForBacsPayment();

        // Act
        var result = validator.IsValid(account, request);

        // Assert
        result.IsValid.Should().Be(expectedResult);
    }
}