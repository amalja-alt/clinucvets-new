namespace ClinicVets.Data;

public static class DatabaseSettings
{
    public const string DatabaseFileName = "clinicvets.db";

    public static string ConnectionString
    {
        get
        {
            string databasePath = Path.Combine(AppContext.BaseDirectory, DatabaseFileName);
            return $"Data Source={databasePath}";
        }
    }
}
