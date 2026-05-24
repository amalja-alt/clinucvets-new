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
<<<<<<< HEAD
<<<<<<< HEAD
            return $"Data Source={databasePath};Default Timeout=10;Pooling=False";
=======
            return $"Data Source={databasePath}";
>>>>>>> 13bfe672cf043b4c83b8f39f62fc93493951aca9
=======
            return $"Data Source={databasePath};Default Timeout=10;Pooling=False";
>>>>>>> main
        }
    }
}
