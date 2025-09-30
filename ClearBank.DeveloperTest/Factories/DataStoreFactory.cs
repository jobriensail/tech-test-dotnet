using System;

namespace ClearBank.DeveloperTest.Factories;

public class DataStoreFactory : IDataStoreFactory
{
    private readonly IConfigurationProvider _configurationProvider;

    public DataStoreFactory(IConfigurationProvider configurationProvider)
    {
        _configurationProvider = configurationProvider ?? throw new ArgumentNullException(nameof(configurationProvider));
    }

    public IAccountDataStore CreateDataStore()
    {
        var dataStoreType = _configurationProvider.GetDataStoreType();

        return dataStoreType?.ToUpperInvariant() == "BACKUP"
            ? new BackupAccountDataStore()
            : new AccountDataStore();
    }
}