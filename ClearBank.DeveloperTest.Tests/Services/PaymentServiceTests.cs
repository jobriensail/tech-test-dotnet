namespace ClearBank.DeveloperTest.Tests.Services;

public class PaymentServiceTests
{
    private readonly Mock<IDataStoreFactory> _dataStoreFactoryMock;
    private readonly Mock<IPaymentValidatorFactory> _paymentValidatorFactoryMock;
    private readonly Mock<IAccountDataStore> _dataStoreMock;
    private readonly PaymentService _sut;

    public PaymentServiceTests()
    {
        _dataStoreFactoryMock = new Mock<IDataStoreFactory>();
        _paymentValidatorFactoryMock = new Mock<IPaymentValidatorFactory>();
        _dataStoreMock = new Mock<IAccountDataStore>();

        _dataStoreFactoryMock
            .Setup(x => x.CreateDataStore())
            .Returns(_dataStoreMock.Object);

        _sut = new PaymentService(_dataStoreFactoryMock.Object, _paymentValidatorFactoryMock.Object);
    }

    [Fact]
    public void MakePayment_WhenRequestIsNull_ReturnsFailure()
    {
        // Act
        var result = _sut.MakePayment(null);

        // Assert
        result.Success.Should().BeFalse();
        result.FailureReason.Should().Be(PaymentFailureReasons.InvalidRequest);
        _dataStoreMock.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Never);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100.50)]
    public void MakePayment_WhenAmountIsZeroOrNegative_ReturnsFailure(decimal amount)
    {
        // Arrange
        var request = new MakePaymentRequest
        {
            Amount = amount,
            DebtorAccountNumber = "123456",
            PaymentScheme = PaymentScheme.Bacs
        };

        // Act
        var result = _sut.MakePayment(request);

        // Assert
        result.Success.Should().BeFalse();
        result.FailureReason.Should().Be(PaymentFailureReasons.AmountNonPositive);
        _dataStoreMock.Verify(x => x.GetAccount(It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void MakePayment_WhenAccountNotFound_ReturnsFailure()
    {
        // Arrange
        var request = new MakePaymentRequest
        {
            Amount = 100,
            DebtorAccountNumber = "123456",
            PaymentScheme = PaymentScheme.Bacs
        };

        _dataStoreMock
            .Setup(x => x.GetAccount(request.DebtorAccountNumber))
            .Returns((Account)null);

        var validatorMock = new Mock<IPaymentValidator>();
        validatorMock
            .Setup(x => x.IsValid(null, request))
            .Returns(ValidationResult.Fail(PaymentFailureReasons.AccountNotFound));

        _paymentValidatorFactoryMock
            .Setup(x => x.GetValidator(request.PaymentScheme))
            .Returns(validatorMock.Object);

        // Act
        var result = _sut.MakePayment(request);

        // Assert
        result.Success.Should().BeFalse();
        result.FailureReason.Should().Be(PaymentFailureReasons.AccountNotFound);
        _dataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
    }

    [Fact]
    public void MakePayment_WhenValidationFails_ReturnsFailure()
    {
        // Arrange
        var account = new Account
        {
            AccountNumber = "123456",
            Balance = 1000,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Chaps
        };

        var request = new MakePaymentRequest
        {
            Amount = 100,
            DebtorAccountNumber = account.AccountNumber,
            PaymentScheme = PaymentScheme.Bacs
        };

        _dataStoreMock
            .Setup(x => x.GetAccount(request.DebtorAccountNumber))
            .Returns(account);

        var validatorMock = new Mock<IPaymentValidator>();
        validatorMock
            .Setup(x => x.IsValid(account, request))
            .Returns(ValidationResult.Fail(PaymentFailureReasons.SchemeNotAllowed));

        _paymentValidatorFactoryMock
            .Setup(x => x.GetValidator(request.PaymentScheme))
            .Returns(validatorMock.Object);

        // Act
        var result = _sut.MakePayment(request);

        // Assert
        result.Success.Should().BeFalse();
        result.FailureReason.Should().Be(PaymentFailureReasons.SchemeNotAllowed);
        _dataStoreMock.Verify(x => x.UpdateAccount(It.IsAny<Account>()), Times.Never);
    }

    [Fact]
    public void MakePayment_WhenValidationSucceeds_DeductsAmountAndReturnsSuccess()
    {
        // Arrange
        var initialBalance = 1000m;
        var paymentAmount = 100m;

        var account = new Account
        {
            AccountNumber = "123456",
            Balance = initialBalance,
            AllowedPaymentSchemes = AllowedPaymentSchemes.Bacs,
            Status = AccountStatus.Live
        };

        var request = new MakePaymentRequest
        {
            Amount = paymentAmount,
            DebtorAccountNumber = account.AccountNumber,
            PaymentScheme = PaymentScheme.Bacs
        };

        _dataStoreMock
            .Setup(x => x.GetAccount(request.DebtorAccountNumber))
            .Returns(account);

        var validatorMock = new Mock<IPaymentValidator>();
        validatorMock
            .Setup(x => x.IsValid(account, request))
            .Returns(ValidationResult.Success());

        _paymentValidatorFactoryMock
            .Setup(x => x.GetValidator(request.PaymentScheme))
            .Returns(validatorMock.Object);

        // Act
        var result = _sut.MakePayment(request);

        // Assert
        result.Success.Should().BeTrue();
        account.Balance.Should().Be(initialBalance - paymentAmount);
        _dataStoreMock.Verify(x => x.UpdateAccount(account), Times.Once);
    }
}