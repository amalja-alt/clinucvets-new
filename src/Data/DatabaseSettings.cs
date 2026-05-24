namespace ClinicVets.Data;

public static class DatabaseSettings
{
    public const string DatabaseFileName = "clinicvets.db";

    public static string ConnectionString
    {
        get
        {
            string databasePath = Path.Combine(AppContext.BaseDirectory, DatabaseFileName);
<<<<<<< HEAD
            return $"Data Source={databasePath};Default Timeout=10;Pooling=False";
=======
            return $"Data Source={databasePath}";
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
        }
    }
}
