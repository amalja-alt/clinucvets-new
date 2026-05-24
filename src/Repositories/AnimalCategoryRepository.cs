using ClinicVets.Models;
using ClinicVets.Repositories.interfacesrepo;
using Microsoft.Data.Sqlite;

namespace ClinicVets.Repositories;

public class AnimalCategoryRepository(string connectionString) : IAnimalCategoryRepository
{
    public IReadOnlyList<AnimalCategory> GetAll()
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Name
            FROM AnimalCategories
            ORDER BY Name;
            """;

        return ReadCategories(command);
    }

    public AnimalCategory? GetById(int categoryId)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Name
            FROM AnimalCategories
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$id", categoryId);

        using SqliteDataReader reader = command.ExecuteReader();
        return reader.Read() ? ReadCategory(reader) : null;
    }

    public bool ExistsByName(string name, int? excludeCategoryId = null)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT COUNT(*)
            FROM AnimalCategories
            WHERE lower(Name) = lower($name)
              AND ($excludeId IS NULL OR Id != $excludeId);
            """;
        command.Parameters.AddWithValue("$name", name.Trim());
        command.Parameters.AddWithValue("$excludeId", excludeCategoryId is null ? DBNull.Value : excludeCategoryId.Value);

        long count = (long)command.ExecuteScalar()!;
        return count > 0;
    }

    public AnimalCategory Add(AnimalCategory category)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO AnimalCategories (Name)
            VALUES ($name);

            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$name", category.Name.Trim());

        long categoryId = (long)command.ExecuteScalar()!;

        return new AnimalCategory
        {
            Id = (int)categoryId,
            Name = category.Name.Trim()
        };
    }

    public bool Remove(int categoryId)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM AnimalCategories WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", categoryId);

        return command.ExecuteNonQuery() > 0;
    }

    public int CountAnimalsUsingCategory(int categoryId)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Animals WHERE CategoryId = $id;";
        command.Parameters.AddWithValue("$id", categoryId);

        return Convert.ToInt32(command.ExecuteScalar());
    }

    private SqliteConnection OpenConnection()
    {
        SqliteConnection connection = new(connectionString);
        connection.Open();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();

        return connection;
    }

    private static IReadOnlyList<AnimalCategory> ReadCategories(SqliteCommand command)
    {
        List<AnimalCategory> categories = [];
        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            categories.Add(ReadCategory(reader));
        }

        return categories;
    }

    private static AnimalCategory ReadCategory(SqliteDataReader reader)
    {
        return new AnimalCategory
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1)
        };
    }
}
