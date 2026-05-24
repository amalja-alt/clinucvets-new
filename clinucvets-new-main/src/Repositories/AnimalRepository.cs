using ClinicVets.Models;
using Microsoft.Data.Sqlite;

namespace ClinicVets.Repositories;

public class AnimalRepository(string connectionString) : IAnimalRepository
{
    public bool ExistsById(int animalId)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Animals WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", animalId);

        long count = (long)command.ExecuteScalar()!;
        return count > 0;
    }

    public bool ExistsByChipNumber(string chipNumber)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Animals WHERE ChipNumber = $chipNumber;";
        command.Parameters.AddWithValue("$chipNumber", chipNumber);

        long count = (long)command.ExecuteScalar()!;
        return count > 0;
    }

    public Animal Add(Animal animal)
    {
        using SqliteConnection connection = OpenConnection();
        int categoryId = GetCategoryId(connection, animal.Type);

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO Animals (
                Name,
                ChipNumber,
                CategoryId,
                WeightKg,
                BirthDate,
                LastVaccinationDate,
                OwnerCustomerId)
            VALUES (
                $name,
                $chipNumber,
                $categoryId,
                $weightKg,
                $birthDate,
                $lastVaccinationDate,
                $ownerCustomerId);

            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$name", animal.Name);
        command.Parameters.AddWithValue("$chipNumber", animal.ChipNumber);
        command.Parameters.AddWithValue("$categoryId", categoryId);
        command.Parameters.AddWithValue("$weightKg", Convert.ToDouble(animal.WeightKg));
        command.Parameters.AddWithValue("$birthDate", FormatDate(animal.BirthDate));
        command.Parameters.AddWithValue("$lastVaccinationDate", FormatDate(animal.LastVaccinationDate));
        command.Parameters.AddWithValue("$ownerCustomerId", animal.OwnerCustomerId);

        long animalId = (long)command.ExecuteScalar()!;

        return new Animal
        {
            Id = (int)animalId,
            Name = animal.Name,
            ChipNumber = animal.ChipNumber,
            Type = animal.Type,
            WeightKg = animal.WeightKg,
            BirthDate = animal.BirthDate,
            LastVaccinationDate = animal.LastVaccinationDate,
            OwnerCustomerId = animal.OwnerCustomerId
        };
    }

    public IReadOnlyList<Animal> FindByOwnerCustomerId(int customerId)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Animals.Id,
                   Animals.Name,
                   Animals.ChipNumber,
                   AnimalCategories.Name AS CategoryName,
                   Animals.WeightKg,
                   Animals.BirthDate,
                   Animals.LastVaccinationDate,
                   Animals.OwnerCustomerId
            FROM Animals
            INNER JOIN AnimalCategories ON AnimalCategories.Id = Animals.CategoryId
            WHERE Animals.OwnerCustomerId = $customerId
            ORDER BY Animals.Name;
            """;
        command.Parameters.AddWithValue("$customerId", customerId);

        return ReadAnimals(command);
    }

    public IReadOnlyList<Animal> SearchByNameOrChip(string searchText)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Animals.Id,
                   Animals.Name,
                   Animals.ChipNumber,
                   AnimalCategories.Name AS CategoryName,
                   Animals.WeightKg,
                   Animals.BirthDate,
                   Animals.LastVaccinationDate,
                   Animals.OwnerCustomerId
            FROM Animals
            INNER JOIN AnimalCategories ON AnimalCategories.Id = Animals.CategoryId
            WHERE Animals.Name LIKE $nameSearch COLLATE NOCASE
               OR Animals.ChipNumber = $chipNumber
            ORDER BY Animals.Name;
            """;
        command.Parameters.AddWithValue("$nameSearch", $"%{searchText}%");
        command.Parameters.AddWithValue("$chipNumber", searchText);

        return ReadAnimals(command);
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

    private static int GetCategoryId(SqliteConnection connection, AnimalType type)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT Id FROM AnimalCategories WHERE Name = $name;";
        command.Parameters.AddWithValue("$name", type.ToString());

        object? value = command.ExecuteScalar();
        if (value is null)
        {
            throw new InvalidOperationException($"Animal category '{type}' does not exist in the database.");
        }

        return Convert.ToInt32(value);
    }

    private static IReadOnlyList<Animal> ReadAnimals(SqliteCommand command)
    {
        List<Animal> animals = [];
        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            animals.Add(ReadAnimal(reader));
        }

        return animals;
    }

    private static Animal ReadAnimal(SqliteDataReader reader)
    {
        return new Animal
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            ChipNumber = reader.GetString(2),
            Type = Enum.Parse<AnimalType>(reader.GetString(3)),
            WeightKg = Convert.ToDecimal(reader.GetDouble(4)),
            BirthDate = DateOnly.Parse(reader.GetString(5)),
            LastVaccinationDate = DateOnly.Parse(reader.GetString(6)),
            OwnerCustomerId = reader.GetInt32(7)
        };
    }

    private static string FormatDate(DateOnly date) => date.ToString("yyyy-MM-dd");
}
