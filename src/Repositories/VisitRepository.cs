using ClinicVets.Models;
using ClinicVets.Repositories.interfacesrepo;
using Microsoft.Data.Sqlite;

namespace ClinicVets.Repositories;

public class VisitRepository(string connectionString) : IVisitRepository
{
    public Visit Add(Visit visit)
    {
        List<Medicine> medicinesGiven = visit.MedicinesGiven.ToList();
        using SqliteConnection connection = OpenConnection();
        using SqliteTransaction transaction = connection.BeginTransaction();

        using SqliteCommand visitCommand = connection.CreateCommand();
        visitCommand.Transaction = transaction;
        visitCommand.CommandText =
            """
            INSERT INTO Visits (
                AnimalId,
                VeterinarianEmployeeId,
                VisitDateTime,
                Reason,
                Diagnosis,
                BaseVisitPrice)
            VALUES (
                $animalId,
                $veterinarianEmployeeId,
                $visitDateTime,
                $reason,
                $diagnosis,
                $baseVisitPrice);

            SELECT last_insert_rowid();
            """;
        visitCommand.Parameters.AddWithValue("$animalId", visit.AnimalId);
        visitCommand.Parameters.AddWithValue("$veterinarianEmployeeId", visit.VeterinarianId);
        visitCommand.Parameters.AddWithValue("$visitDateTime", visit.VisitDateTime.ToString("O"));
        visitCommand.Parameters.AddWithValue("$reason", visit.Reason);
        visitCommand.Parameters.AddWithValue("$diagnosis", visit.Diagnosis);
        visitCommand.Parameters.AddWithValue("$baseVisitPrice", Convert.ToDouble(visit.BaseVisitPrice));

        long visitId = (long)visitCommand.ExecuteScalar()!;

        foreach (Medicine medicine in medicinesGiven)
        {
            using SqliteCommand medicineCommand = connection.CreateCommand();
            medicineCommand.Transaction = transaction;
            medicineCommand.CommandText =
                """
                INSERT INTO VisitMedicines (VisitId, MedicineId, Quantity, UnitPrice)
                VALUES ($visitId, $medicineId, 1, $unitPrice);
                """;
            medicineCommand.Parameters.AddWithValue("$visitId", visitId);
            medicineCommand.Parameters.AddWithValue("$medicineId", medicine.Id);
            medicineCommand.Parameters.AddWithValue("$unitPrice", Convert.ToDouble(medicine.Price));
            medicineCommand.ExecuteNonQuery();
        }

        transaction.Commit();

        Visit savedVisit = new()
        {
            Id = (int)visitId,
            AnimalId = visit.AnimalId,
            VeterinarianId = visit.VeterinarianId,
            Reason = visit.Reason,
            Diagnosis = visit.Diagnosis,
            VisitDateTime = visit.VisitDateTime,
            BaseVisitPrice = visit.BaseVisitPrice
        };

        savedVisit.MedicinesGiven.AddRange(medicinesGiven);
        return savedVisit;
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
