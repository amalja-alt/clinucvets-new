using ClinicVets.Models;
using ClinicVets.Repositories.interfacesrepo;
using Microsoft.Data.Sqlite;

namespace ClinicVets.Repositories;

public class MedicineRepository(string connectionString) : IMedicineRepository
{
    public IReadOnlyList<Medicine> GetAll()
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, Name, Price, QuantityInStock
            FROM Medicines
            ORDER BY Name;
            """;

        return ReadMedicines(command);
    }

    public IReadOnlyList<Medicine> FindByIds(IEnumerable<int> medicineIds)
    {
        List<int> ids = medicineIds.Distinct().ToList();
        if (ids.Count == 0)
        {
            return [];
        }

        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        List<string> parameterNames = [];

        for (int index = 0; index < ids.Count; index++)
        {
            string parameterName = $"$id{index}";
            parameterNames.Add(parameterName);
            command.Parameters.AddWithValue(parameterName, ids[index]);
        }

        command.CommandText =
            $"""
            SELECT Id, Name, Price, QuantityInStock
            FROM Medicines
            WHERE Id IN ({string.Join(", ", parameterNames)})
            ORDER BY Name;
            """;

        return ReadMedicines(command);
    }

    public Medicine Add(Medicine medicine)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO Medicines (Name, Price, QuantityInStock)
            VALUES ($name, $price, $quantityInStock);

            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$name", medicine.Name);
        command.Parameters.AddWithValue("$price", Convert.ToDouble(medicine.Price));
        command.Parameters.AddWithValue("$quantityInStock", medicine.QuantityInStock);

        long medicineId = (long)command.ExecuteScalar()!;

        return new Medicine
        {
            Id = (int)medicineId,
            Name = medicine.Name,
            Price = medicine.Price,
            QuantityInStock = medicine.QuantityInStock
        };
    }

    public bool Remove(int medicineId)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "DELETE FROM Medicines WHERE Id = $id;";
        command.Parameters.AddWithValue("$id", medicineId);

        return command.ExecuteNonQuery() > 0;
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

    private static IReadOnlyList<Medicine> ReadMedicines(SqliteCommand command)
    {
        List<Medicine> medicines = [];
        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            medicines.Add(new Medicine
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Price = Convert.ToDecimal(reader.GetDouble(2)),
                QuantityInStock = reader.GetInt32(3)
            });
        }

        return medicines;
    }
}
