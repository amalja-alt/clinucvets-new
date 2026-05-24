using ClinicVets.Data;

namespace ClinicVets.Tests.TestSupport;

internal sealed class SqliteTestDatabase : IDisposable
{
    public string DatabasePath { get; } = Path.Combine(
        Path.GetTempPath(),
        $"clinicvets-tests-{Guid.NewGuid():N}.db");

    public string ConnectionString => $"Data Source={DatabasePath};Default Timeout=10;Pooling=False";

    public void Initialize()
    {
        ClinicDatabaseInitializer initializer = new(ConnectionString);
        initializer.Initialize();
    }

    public void Dispose()
    {
        if (File.Exists(DatabasePath))
        {
            File.Delete(DatabasePath);
        }
    }
}
