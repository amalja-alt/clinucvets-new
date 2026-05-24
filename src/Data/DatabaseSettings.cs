namespace ClinicVets.Data;

public static class DatabaseSettings
{
    // the name of the database file that we will use to store the data of the clinic
    // instead of writing the SQLite connection string everywhere in the code 
    // we will use this class to get the connection string and the database file name
    public const string DatabaseFileName = "clinicvets.db";

    public static string ConnectionString
    {
        get
        {
            string databasePath = Path.Combine(AppContext.BaseDirectory, DatabaseFileName);
            return $"Data Source={databasePath};Default Timeout=10;Pooling=False";
        }
    }
}
