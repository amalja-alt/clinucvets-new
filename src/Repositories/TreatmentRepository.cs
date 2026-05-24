using ClinicVets.Models;
using ClinicVets.Repositories.interfacesrepo;
using Microsoft.Data.Sqlite;

namespace ClinicVets.Repositories;

public class TreatmentRepository(string connectionString) : ITreatmentRepository
{
    public Treatment Add(Treatment treatment)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand medicineCommand = connection.CreateCommand();
        medicineCommand.CommandText =
            """
            SELECT Name, Price
            FROM Medicines
            WHERE Id = $medicineId;
            """;
        medicineCommand.Parameters.AddWithValue("$medicineId", treatment.MedicineId);

        using SqliteDataReader medicineReader = medicineCommand.ExecuteReader();
        if (!medicineReader.Read())
        {
            throw new InvalidOperationException($"Medicine '{treatment.MedicineId}' does not exist in the database.");
        }

        string medicineName = medicineReader.GetString(0);
        decimal medicinePrice = Convert.ToDecimal(medicineReader.GetDouble(1));
        medicineReader.Close();

        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO VisitMedicines (VisitId, MedicineId, Quantity, UnitPrice)
            VALUES ($visitId, $medicineId, $quantity, $unitPrice);

            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$visitId", treatment.VisitId);
        command.Parameters.AddWithValue("$medicineId", treatment.MedicineId);
        command.Parameters.AddWithValue("$quantity", treatment.Quantity);
        command.Parameters.AddWithValue("$unitPrice", Convert.ToDouble(medicinePrice));

        long treatmentId = (long)command.ExecuteScalar()!;

        return new Treatment
        {
            Id = (int)treatmentId,
            VisitId = treatment.VisitId,
            MedicineId = treatment.MedicineId,
            MedicineName = medicineName,
            Quantity = treatment.Quantity,
            MedicinePrice = medicinePrice,
            TreatmentDate = treatment.TreatmentDate
        };
    }

    public IReadOnlyList<Treatment> GetByVisitId(int visitId)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT VisitMedicines.Id,
                   VisitMedicines.VisitId,
                   VisitMedicines.MedicineId,
                   Medicines.Name,
                   VisitMedicines.Quantity,
                   VisitMedicines.UnitPrice,
                   Visits.VisitDateTime
            FROM VisitMedicines
            INNER JOIN Medicines ON Medicines.Id = VisitMedicines.MedicineId
            INNER JOIN Visits ON Visits.Id = VisitMedicines.VisitId
            WHERE VisitMedicines.VisitId = $visitId
            ORDER BY VisitMedicines.Id;
            """;
        command.Parameters.AddWithValue("$visitId", visitId);

        List<Treatment> treatments = [];
        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            treatments.Add(new Treatment
            {
                Id = reader.GetInt32(0),
                VisitId = reader.GetInt32(1),
                MedicineId = reader.GetInt32(2),
                MedicineName = reader.GetString(3),
                Quantity = reader.GetInt32(4),
                MedicinePrice = Convert.ToDecimal(reader.GetDouble(5)),
                TreatmentDate = DateTime.Parse(reader.GetString(6))
            });
        }

        return treatments;
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
}
