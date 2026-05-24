using ClinicVets.Models;
using ClinicVets.Repositories.interfacesrepo;
using Microsoft.Data.Sqlite;

namespace ClinicVets.Repositories;

public class ClinicLookupRepository(string connectionString) : IClinicLookupRepository
{
    public IReadOnlyList<Customer> GetAllCustomers()
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, FullName, IdentityNumber, Phone, Email
            FROM Customers
            ORDER BY FullName;
            """;

        List<Customer> customers = [];
        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            customers.Add(new Customer
            {
                Id = reader.GetInt32(0),
                FullName = reader.GetString(1),
                IdentityNumber = reader.GetString(2),
                Phone = reader.GetString(3),
                Email = reader.GetString(4)
            });
        }

        return customers;
    }

    public int CountVisits()
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "SELECT COUNT(*) FROM Visits;";

        return Convert.ToInt32(command.ExecuteScalar());
    }

    public int CountVisitsForDate(DateOnly date)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT COUNT(*)
            FROM Visits
            WHERE substr(VisitDateTime, 1, 10) = $date;
            """;
        command.Parameters.AddWithValue("$date", date.ToString("yyyy-MM-dd"));

        return Convert.ToInt32(command.ExecuteScalar());
    }

    public IReadOnlyList<DashboardVisitSummary> GetVisitsForDate(DateOnly date)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT
                Visits.Id,
                Visits.VisitDateTime,
                Animals.Name,
                Customers.FullName,
                Employees.Username,
                Visits.Reason,
                Visits.Diagnosis
            FROM Visits
            INNER JOIN Animals ON Animals.Id = Visits.AnimalId
            INNER JOIN Customers ON Customers.Id = Animals.OwnerCustomerId
            INNER JOIN Employees ON Employees.Id = Visits.VeterinarianEmployeeId
            WHERE substr(Visits.VisitDateTime, 1, 10) = $date
            ORDER BY Visits.VisitDateTime;
            """;
        command.Parameters.AddWithValue("$date", date.ToString("yyyy-MM-dd"));

        return ReadVisitSummaries(command);
    }

    public IReadOnlyList<DashboardVisitSummary> GetRecentVisits(int maxCount)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT
                Visits.Id,
                Visits.VisitDateTime,
                Animals.Name,
                Customers.FullName,
                Employees.Username,
                Visits.Reason,
                Visits.Diagnosis
            FROM Visits
            INNER JOIN Animals ON Animals.Id = Visits.AnimalId
            INNER JOIN Customers ON Customers.Id = Animals.OwnerCustomerId
            INNER JOIN Employees ON Employees.Id = Visits.VeterinarianEmployeeId
            ORDER BY Visits.VisitDateTime DESC
            LIMIT $maxCount;
            """;
        command.Parameters.AddWithValue("$maxCount", Math.Max(1, maxCount));

        return ReadVisitSummaries(command);
    }

    private static IReadOnlyList<DashboardVisitSummary> ReadVisitSummaries(SqliteCommand command)
    {
        List<DashboardVisitSummary> visits = [];
        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            visits.Add(new DashboardVisitSummary
            {
                VisitId = reader.GetInt32(0),
                VisitDateTime = DateTime.Parse(reader.GetString(1)),
                PetName = reader.GetString(2),
                OwnerName = reader.GetString(3),
                VeterinarianName = reader.GetString(4),
                Reason = reader.GetString(5),
                Diagnosis = reader.GetString(6)
            });
        }

        return visits;
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
