namespace ClearBank.DeveloperTest.Tests.Factories;

public class DataStoreFactoryTests
{
    private readonly Mock<IConfigurationProvider> _configProviderMock;
    private readonly DataStoreFactory _sut;

    public DataStoreFactoryTests()
    {
        _configProviderMock = new Mock<IConfigurationProvider>();
        _sut = new DataStoreFactory(_configProviderMock.Object);
    }

    [Theory]
    [InlineData("Default")]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("anything")]
    public void CreateDataStore_WhenConfigIsNotBackup_ReturnsDefaultDataStore(string configValue)
    {
        // Arrange
        _configProviderMock.Setup(x => x.GetDataStoreType()).Returns(configValue);

        // Act
        var result = _sut.CreateDataStore();

        // Assert
        result.Should().BeOfType<AccountDataStore>();
    }

    [Theory]
    [InlineData("Backup")]
    [InlineData("backup")]
    [InlineData("BACKUP")]
    public void CreateDataStore_WhenConfigIsBackup_ReturnsBackupDataStore(string configValue)
    {
        // Arrange
        _configProviderMock.Setup(x => x.GetDataStoreType()).Returns(configValue);

        // Act
        var result = _sut.CreateDataStore();

        // Assert
        result.Should().BeOfType<BackupAccountDataStore>();
    }
}