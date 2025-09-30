using System.Configuration;

namespace ClearBank.DeveloperTest.Configuration;

public class AppConfigurationProvider : IConfigurationProvider
{
    public string GetDataStoreType()
    {
        return ConfigurationManager.AppSettings["DataStoreType"] ?? "Default";
    }
}
