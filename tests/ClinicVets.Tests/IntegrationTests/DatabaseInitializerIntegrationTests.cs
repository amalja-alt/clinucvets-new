using ClinicVets.Tests.TestSupport;
using Microsoft.Data.Sqlite;

namespace ClinicVets.Tests.IntegrationTests;

public class DatabaseInitializerIntegrationTests
{
    [Fact]
    public void Initialize_CreatesAssignmentTablesAndLookupData_InFreshSqliteDatabase()
    {
        using SqliteTestDatabase database = new();

        database.Initialize();

        using SqliteConnection connection = new(database.ConnectionString);
        connection.Open();

        Assert.True(TableExists(connection, "Roles"));
        Assert.True(TableExists(connection, "Employees"));
        Assert.True(TableExists(connection, "Customers"));
        Assert.True(TableExists(connection, "AnimalCategories"));
        Assert.True(TableExists(connection, "Animals"));
        Assert.True(TableExists(connection, "Medicines"));
        Assert.True(TableExists(connection, "Visits"));
        Assert.True(TableExists(connection, "VisitMedicines"));
        Assert.Equal(1, CountRows(connection, "Roles", "Name = 'Secretary'"));
        Assert.Equal(1, CountRows(connection, "Roles", "Name = 'Veterinarian'"));
        Assert.Equal(4, CountRows(connection, "AnimalCategories", "1 = 1"));
    }

    [Fact]
    public void Initialize_CanRunTwice_WithoutDuplicatingSeedRows()
    {
        using SqliteTestDatabase database = new();

        database.Initialize();
        database.Initialize();

        using SqliteConnection connection = new(database.ConnectionString);
        connection.Open();

        Assert.Equal(2, CountRows(connection, "Roles", "1 = 1"));
        Assert.Equal(4, CountRows(connection, "AnimalCategories", "1 = 1"));
    }

    private static bool TableExists(SqliteConnection connection, string tableName)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT COUNT(*)
            FROM sqlite_master
            WHERE type = 'table' AND name = $tableName;
            """;
        command.Parameters.AddWithValue("$tableName", tableName);
        return (long)command.ExecuteScalar()! == 1;
    }

    private static long CountRows(SqliteConnection connection, string tableName, string whereClause)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = $"SELECT COUNT(*) FROM {tableName} WHERE {whereClause};";
        return (long)command.ExecuteScalar()!;
    }
}
