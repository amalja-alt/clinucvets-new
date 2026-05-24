using ClinicVets.Models;
using Microsoft.Data.Sqlite;

namespace ClinicVets.Repositories;


// alaa 
// this class between the ui and the data access layer ( in memory or database )
// interface because we can have multiple implementations   
// implimintation of the ICustomerRepository interface using SQLite as the data store


public class CustomerRepository(string connectionString) : ICustomerRepository
{
    public bool ExistsByIdentityNumber(string identityNumber)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT COUNT(*)
            FROM Customers
            WHERE IdentityNumber = $identityNumber;
            """;
        command.Parameters.AddWithValue("$identityNumber", identityNumber);

        long count = (long)command.ExecuteScalar()!;
        return count > 0;
    }

    public Customer? FindByIdentityOrPhone(string searchText)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, FullName, IdentityNumber, Phone, Email
            FROM Customers
            WHERE IdentityNumber = $searchText OR Phone = $searchText
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("$searchText", searchText);

        using SqliteDataReader reader = command.ExecuteReader();
        return reader.Read() ? ReadCustomer(reader) : null;
    }

    public Customer? FindById(int customerId)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            SELECT Id, FullName, IdentityNumber, Phone, Email
            FROM Customers
            WHERE Id = $id;
            """;
        command.Parameters.AddWithValue("$id", customerId);

        using SqliteDataReader reader = command.ExecuteReader();
        return reader.Read() ? ReadCustomer(reader) : null;
    }

    public Customer Add(Customer customer)
    {
        using SqliteConnection connection = OpenConnection();
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT INTO Customers (FullName, IdentityNumber, Phone, Email)
            VALUES ($fullName, $identityNumber, $phone, $email);

            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$fullName", customer.FullName);
        command.Parameters.AddWithValue("$identityNumber", customer.IdentityNumber);
        command.Parameters.AddWithValue("$phone", customer.Phone);
        command.Parameters.AddWithValue("$email", customer.Email);

        long customerId = (long)command.ExecuteScalar()!;

        return new Customer
        {
            Id = (int)customerId,
            FullName = customer.FullName,
            IdentityNumber = customer.IdentityNumber,
            Phone = customer.Phone,
            Email = customer.Email
        };
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

    private static Customer ReadCustomer(SqliteDataReader reader)
    {
        return new Customer
        {
            Id = reader.GetInt32(0),
            FullName = reader.GetString(1),
            IdentityNumber = reader.GetString(2),
            Phone = reader.GetString(3),
            Email = reader.GetString(4)
        };
    }
}
