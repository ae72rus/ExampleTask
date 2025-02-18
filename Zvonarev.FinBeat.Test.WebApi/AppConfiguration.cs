using Zvonarev.FinBeat.Test.HttpDbLogging.Configuration;
using Zvonarev.FinBeat.Test.Storage.Configuration;

namespace Zvonarev.FinBeat.Test.WebApi;

internal class AppConfiguration
{
    public HttpLoggingConfiguration HttpLogging { get; init; } = HttpLoggingConfiguration.Default;
    public StorageConfiguration Storage { get; init; }
}