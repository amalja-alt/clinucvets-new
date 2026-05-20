using Microsoft.Data.Sqlite;
using ClinicVets.Services;
using ClinicVets.Validators;

namespace ClinicVets.Data;

/// <summary>
/// Creates the SQLite schema and required seed data for ClinicVets.
/// </summary>
public class ClinicDatabaseInitializer(string connectionString)
{
    /// <summary>
    /// Creates required tables and seeds fixed lookup data.
    /// </summary>
    public void Initialize()
    {
        using SqliteConnection connection = new(connectionString);
        connection.Open();

        EnableForeignKeys(connection);
        CreateRolesTable(connection);
        CreateEmployeesTable(connection);
        CreateCustomersTable(connection);
        CreateAnimalCategoriesTable(connection);
        CreateAnimalsTable(connection);
        CreateMedicinesTable(connection);
        CreateVisitsTable(connection);
        CreateVisitMedicinesTable(connection);
        SeedRoles(connection);
        RemoveUnsupportedRoles(connection);
        SeedAnimalCategories(connection);
        SeedOrUpdateDemoData(connection);
    }

    private static void EnableForeignKeys(SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();
    }

    private static void CreateRolesTable(SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Roles (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );
            """;
        command.ExecuteNonQuery();
    }

    private static void CreateEmployeesTable(SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Employees (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Username TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                EmployeeNumber TEXT NOT NULL UNIQUE,
                Email TEXT NOT NULL UNIQUE,
                IdentityNumber TEXT NOT NULL UNIQUE,
                RoleId INTEGER NOT NULL,
                FOREIGN KEY (RoleId) REFERENCES Roles(Id)
            );
            """;
        command.ExecuteNonQuery();
    }

    private static void CreateCustomersTable(SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Customers (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                FullName TEXT NOT NULL,
                IdentityNumber TEXT NOT NULL UNIQUE,
                Phone TEXT NOT NULL,
                Email TEXT NOT NULL UNIQUE
            );
            """;
        command.ExecuteNonQuery();
    }

    private static void CreateAnimalCategoriesTable(SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS AnimalCategories (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE
            );
            """;
        command.ExecuteNonQuery();
    }

    private static void CreateAnimalsTable(SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Animals (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                ChipNumber TEXT NOT NULL UNIQUE,
                CategoryId INTEGER NOT NULL,
                WeightKg REAL NOT NULL CHECK (WeightKg > 0),
                BirthDate TEXT NOT NULL,
                LastVaccinationDate TEXT NOT NULL,
                OwnerCustomerId INTEGER NOT NULL,
                FOREIGN KEY (CategoryId) REFERENCES AnimalCategories(Id),
                FOREIGN KEY (OwnerCustomerId) REFERENCES Customers(Id)
            );
            """;
        command.ExecuteNonQuery();
    }

    private static void CreateMedicinesTable(SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Medicines (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL UNIQUE,
                Price REAL NOT NULL CHECK (Price >= 0),
                QuantityInStock INTEGER NOT NULL CHECK (QuantityInStock >= 0)
            );
            """;
        command.ExecuteNonQuery();
    }

    private static void CreateVisitsTable(SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS Visits (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                AnimalId INTEGER NOT NULL,
                VeterinarianEmployeeId INTEGER NOT NULL,
                VisitDateTime TEXT NOT NULL,
                Reason TEXT NOT NULL,
                Diagnosis TEXT NOT NULL,
                BaseVisitPrice REAL NOT NULL CHECK (BaseVisitPrice >= 0),
                FOREIGN KEY (AnimalId) REFERENCES Animals(Id),
                FOREIGN KEY (VeterinarianEmployeeId) REFERENCES Employees(Id)
            );
            """;
        command.ExecuteNonQuery();
    }

    private static void CreateVisitMedicinesTable(SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            CREATE TABLE IF NOT EXISTS VisitMedicines (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                VisitId INTEGER NOT NULL,
                MedicineId INTEGER NOT NULL,
                Quantity INTEGER NOT NULL DEFAULT 1 CHECK (Quantity > 0),
                UnitPrice REAL NOT NULL CHECK (UnitPrice >= 0),
                FOREIGN KEY (VisitId) REFERENCES Visits(Id) ON DELETE CASCADE,
                FOREIGN KEY (MedicineId) REFERENCES Medicines(Id),
                UNIQUE (VisitId, MedicineId)
            );
            """;
        command.ExecuteNonQuery();
    }

    private static void SeedRoles(SqliteConnection connection)
    {
        InsertRoleIfMissing(connection, "Veterinarian");
        InsertRoleIfMissing(connection, "Secretary");
    }

    private static void SeedAnimalCategories(SqliteConnection connection)
    {
        InsertAnimalCategoryIfMissing(connection, "Dog");
        InsertAnimalCategoryIfMissing(connection, "Cat");
        InsertAnimalCategoryIfMissing(connection, "Reptile");
        InsertAnimalCategoryIfMissing(connection, "Bird");
    }

    private static void InsertRoleIfMissing(SqliteConnection connection, string roleName)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT OR IGNORE INTO Roles (Name)
            VALUES ($name);
            """;
        command.Parameters.AddWithValue("$name", roleName);
        command.ExecuteNonQuery();
    }

    private static void RemoveUnsupportedRoles(SqliteConnection connection)
    {
        using SqliteTransaction transaction = connection.BeginTransaction();
        int secretaryRoleId = GetRoleId(connection, transaction, "Secretary");

        using SqliteCommand updateEmployeesCommand = connection.CreateCommand();
        updateEmployeesCommand.Transaction = transaction;
        updateEmployeesCommand.CommandText =
            """
            UPDATE Employees
            SET RoleId = $secretaryRoleId
            WHERE RoleId IN (
                SELECT Id
                FROM Roles
                WHERE Name NOT IN ('Veterinarian', 'Secretary')
            );
            """;
        updateEmployeesCommand.Parameters.AddWithValue("$secretaryRoleId", secretaryRoleId);
        updateEmployeesCommand.ExecuteNonQuery();

        using SqliteCommand deleteRolesCommand = connection.CreateCommand();
        deleteRolesCommand.Transaction = transaction;
        deleteRolesCommand.CommandText =
            """
            DELETE FROM Roles
            WHERE Name NOT IN ('Veterinarian', 'Secretary');
            """;
        deleteRolesCommand.ExecuteNonQuery();

        transaction.Commit();
    }

    private static void InsertAnimalCategoryIfMissing(SqliteConnection connection, string categoryName)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText =
            """
            INSERT OR IGNORE INTO AnimalCategories (Name)
            VALUES ($name);
            """;
        command.Parameters.AddWithValue("$name", categoryName);
        command.ExecuteNonQuery();
    }

    private static void SeedOrUpdateDemoData(SqliteConnection connection)
    {
        using SqliteTransaction transaction = connection.BeginTransaction();

        int secretaryRoleId = GetRoleId(connection, transaction, "Secretary");
        int veterinarianRoleId = GetRoleId(connection, transaction, "Veterinarian");

        int secretaryId = UpsertDemoEmployee(connection, transaction, "secret1", "Secret#1", "9002", "secretary@clinicvets.com", "100000009", secretaryRoleId);
        int veterinarianId = UpsertDemoEmployee(connection, transaction, "vetuser", "Vetuser#1", "9003", "vet@clinicvets.com", "100000017", veterinarianRoleId);

        int customer1Id = InsertCustomerIfMissing(connection, transaction, "Dana Levi", "123456782", "0501234567", "dana.levi@example.com");
        int customer2Id = InsertCustomerIfMissing(connection, transaction, "Noam Cohen", "234567899", "0527654321", "noam.cohen@example.com");
        int customer3Id = InsertCustomerIfMissing(connection, transaction, "Maya Amir", "345678916", "0541112233", "maya.amir@example.com");

        int dogCategoryId = GetCategoryId(connection, transaction, "Dog");
        int catCategoryId = GetCategoryId(connection, transaction, "Cat");
        int birdCategoryId = GetCategoryId(connection, transaction, "Bird");

        int dogId = InsertAnimalIfMissing(connection, transaction, "Buddy", "DOG-1001", dogCategoryId, 18.5, "2021-04-12", "2025-03-20", customer1Id);
        int catId = InsertAnimalIfMissing(connection, transaction, "Luna", "CAT-2001", catCategoryId, 4.3, "2022-08-05", "2024-02-15", customer2Id);
        int birdId = InsertAnimalIfMissing(connection, transaction, "Kiwi", "BRD-3001", birdCategoryId, 0.4, "2023-11-10", "2025-05-01", customer3Id);

        int vaccineId = InsertMedicineIfMissing(connection, transaction, "Annual Vaccine", 120.00, 25);
        int antibioticId = InsertMedicineIfMissing(connection, transaction, "Antibiotic Drops", 85.50, 18);
        InsertMedicineIfMissing(connection, transaction, "Pain Relief Tablets", 64.90, 30);

        int visit1Id = InsertVisitIfMissing(connection, transaction, dogId, veterinarianId, "2026-05-01T10:30:00.0000000", "Annual checkup", "Healthy dog, vaccine administered", 150.00);
        int visit2Id = InsertVisitIfMissing(connection, transaction, catId, veterinarianId, "2026-05-05T14:15:00.0000000", "Eye irritation", "Mild infection, drops prescribed", 150.00);
        int todayVisit1Id = InsertVisitIfMissing(connection, transaction, dogId, veterinarianId, DateTime.Today.AddHours(9).ToString("O"), "Annual wellness visit", "Scheduled checkup", 150.00);
        int todayVisit2Id = InsertVisitIfMissing(connection, transaction, catId, veterinarianId, DateTime.Today.AddHours(11).AddMinutes(30).ToString("O"), "Vaccination follow-up", "Awaiting veterinarian review", 150.00);
        InsertVisitIfMissing(connection, transaction, birdId, veterinarianId, DateTime.Today.AddHours(14).ToString("O"), "Nutrition consultation", "Scheduled consultation", 150.00);

        InsertVisitMedicineIfMissing(connection, transaction, visit1Id, vaccineId, 1, 120.00);
        InsertVisitMedicineIfMissing(connection, transaction, visit2Id, antibioticId, 1, 85.50);
        InsertVisitMedicineIfMissing(connection, transaction, todayVisit1Id, vaccineId, 1, 120.00);
        InsertVisitMedicineIfMissing(connection, transaction, todayVisit2Id, antibioticId, 1, 85.50);

        _ = secretaryId;

        transaction.Commit();
    }

    private static int GetRoleId(SqliteConnection connection, SqliteTransaction transaction, string roleName)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "SELECT Id FROM Roles WHERE Name = $name;";
        command.Parameters.AddWithValue("$name", roleName);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private static int GetCategoryId(SqliteConnection connection, SqliteTransaction transaction, string categoryName)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "SELECT Id FROM AnimalCategories WHERE Name = $name;";
        command.Parameters.AddWithValue("$name", categoryName);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private static int UpsertDemoEmployee(SqliteConnection connection, SqliteTransaction transaction, string username, string password, string employeeNumber, string email, string identityNumber, int roleId)
    {
        OperationResult<bool> validation = new EmployeeValidator().ValidateRegistration(
            username,
            password,
            employeeNumber,
            email,
            identityNumber);

        if (!validation.IsSuccess)
        {
            throw new InvalidOperationException($"Demo employee '{username}' is invalid: {validation.ErrorMessage}");
        }

        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT INTO Employees (Username, PasswordHash, EmployeeNumber, Email, IdentityNumber, RoleId)
            VALUES ($username, $passwordHash, $employeeNumber, $email, $identityNumber, $roleId)
            ON CONFLICT(Username) DO UPDATE SET
                PasswordHash = excluded.PasswordHash,
                EmployeeNumber = excluded.EmployeeNumber,
                Email = excluded.Email,
                IdentityNumber = excluded.IdentityNumber,
                RoleId = excluded.RoleId;
            """;
        command.Parameters.AddWithValue("$username", username);
        command.Parameters.AddWithValue("$passwordHash", password);
        command.Parameters.AddWithValue("$employeeNumber", employeeNumber);
        command.Parameters.AddWithValue("$email", email);
        command.Parameters.AddWithValue("$identityNumber", identityNumber);
        command.Parameters.AddWithValue("$roleId", roleId);
        command.ExecuteNonQuery();

        return GetEmployeeIdByUsername(connection, transaction, username);
    }

    private static int InsertCustomerIfMissing(SqliteConnection connection, SqliteTransaction transaction, string fullName, string identityNumber, string phone, string email)
    {
        int? existingId = GetCustomerIdByIdentityOrEmail(connection, transaction, identityNumber, email);
        if (existingId is not null)
        {
            return existingId.Value;
        }

        return InsertCustomer(connection, transaction, fullName, identityNumber, phone, email);
    }

    private static int? GetCustomerIdByIdentityOrEmail(SqliteConnection connection, SqliteTransaction transaction, string identityNumber, string email)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            SELECT Id
            FROM Customers
            WHERE IdentityNumber = $identityNumber OR Email = $email
            LIMIT 1;
            """;
        command.Parameters.AddWithValue("$identityNumber", identityNumber);
        command.Parameters.AddWithValue("$email", email);

        object? id = command.ExecuteScalar();
        return id is null ? null : Convert.ToInt32(id);
    }

    private static int InsertCustomer(SqliteConnection connection, SqliteTransaction transaction, string fullName, string identityNumber, string phone, string email)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT INTO Customers (FullName, IdentityNumber, Phone, Email)
            VALUES ($fullName, $identityNumber, $phone, $email);

            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$fullName", fullName);
        command.Parameters.AddWithValue("$identityNumber", identityNumber);
        command.Parameters.AddWithValue("$phone", phone);
        command.Parameters.AddWithValue("$email", email);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private static int InsertAnimalIfMissing(SqliteConnection connection, SqliteTransaction transaction, string name, string chipNumber, int categoryId, double weightKg, string birthDate, string lastVaccinationDate, int ownerCustomerId)
    {
        int? existingId = GetIdByTextField(connection, transaction, "Animals", "ChipNumber", chipNumber);
        if (existingId is not null)
        {
            return existingId.Value;
        }

        return InsertAnimal(connection, transaction, name, chipNumber, categoryId, weightKg, birthDate, lastVaccinationDate, ownerCustomerId);
    }

    private static int InsertAnimal(SqliteConnection connection, SqliteTransaction transaction, string name, string chipNumber, int categoryId, double weightKg, string birthDate, string lastVaccinationDate, int ownerCustomerId)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT INTO Animals (Name, ChipNumber, CategoryId, WeightKg, BirthDate, LastVaccinationDate, OwnerCustomerId)
            VALUES ($name, $chipNumber, $categoryId, $weightKg, $birthDate, $lastVaccinationDate, $ownerCustomerId);

            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$chipNumber", chipNumber);
        command.Parameters.AddWithValue("$categoryId", categoryId);
        command.Parameters.AddWithValue("$weightKg", weightKg);
        command.Parameters.AddWithValue("$birthDate", birthDate);
        command.Parameters.AddWithValue("$lastVaccinationDate", lastVaccinationDate);
        command.Parameters.AddWithValue("$ownerCustomerId", ownerCustomerId);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private static int InsertMedicineIfMissing(SqliteConnection connection, SqliteTransaction transaction, string name, double price, int quantityInStock)
    {
        int? existingId = GetIdByTextField(connection, transaction, "Medicines", "Name", name);
        if (existingId is not null)
        {
            return existingId.Value;
        }

        return InsertMedicine(connection, transaction, name, price, quantityInStock);
    }

    private static int InsertMedicine(SqliteConnection connection, SqliteTransaction transaction, string name, double price, int quantityInStock)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT INTO Medicines (Name, Price, QuantityInStock)
            VALUES ($name, $price, $quantityInStock);

            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$name", name);
        command.Parameters.AddWithValue("$price", price);
        command.Parameters.AddWithValue("$quantityInStock", quantityInStock);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private static int InsertVisitIfMissing(SqliteConnection connection, SqliteTransaction transaction, int animalId, int veterinarianId, string visitDateTime, string reason, string diagnosis, double baseVisitPrice)
    {
        using SqliteCommand existingCommand = connection.CreateCommand();
        existingCommand.Transaction = transaction;
        existingCommand.CommandText =
            """
            SELECT Id
            FROM Visits
            WHERE AnimalId = $animalId
              AND VisitDateTime = $visitDateTime
            LIMIT 1;
            """;
        existingCommand.Parameters.AddWithValue("$animalId", animalId);
        existingCommand.Parameters.AddWithValue("$visitDateTime", visitDateTime);

        object? existingId = existingCommand.ExecuteScalar();
        if (existingId is not null)
        {
            return Convert.ToInt32(existingId);
        }

        return InsertVisit(connection, transaction, animalId, veterinarianId, visitDateTime, reason, diagnosis, baseVisitPrice);
    }

    private static int InsertVisit(SqliteConnection connection, SqliteTransaction transaction, int animalId, int veterinarianId, string visitDateTime, string reason, string diagnosis, double baseVisitPrice)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT INTO Visits (AnimalId, VeterinarianEmployeeId, VisitDateTime, Reason, Diagnosis, BaseVisitPrice)
            VALUES ($animalId, $veterinarianId, $visitDateTime, $reason, $diagnosis, $baseVisitPrice);

            SELECT last_insert_rowid();
            """;
        command.Parameters.AddWithValue("$animalId", animalId);
        command.Parameters.AddWithValue("$veterinarianId", veterinarianId);
        command.Parameters.AddWithValue("$visitDateTime", visitDateTime);
        command.Parameters.AddWithValue("$reason", reason);
        command.Parameters.AddWithValue("$diagnosis", diagnosis);
        command.Parameters.AddWithValue("$baseVisitPrice", baseVisitPrice);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private static void InsertVisitMedicineIfMissing(SqliteConnection connection, SqliteTransaction transaction, int visitId, int medicineId, int quantity, double unitPrice)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText =
            """
            INSERT OR IGNORE INTO VisitMedicines (VisitId, MedicineId, Quantity, UnitPrice)
            VALUES ($visitId, $medicineId, $quantity, $unitPrice);
            """;
        command.Parameters.AddWithValue("$visitId", visitId);
        command.Parameters.AddWithValue("$medicineId", medicineId);
        command.Parameters.AddWithValue("$quantity", quantity);
        command.Parameters.AddWithValue("$unitPrice", unitPrice);
        command.ExecuteNonQuery();
    }

    private static int GetEmployeeIdByUsername(SqliteConnection connection, SqliteTransaction transaction, string username)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = "SELECT Id FROM Employees WHERE Username = $username;";
        command.Parameters.AddWithValue("$username", username);
        return Convert.ToInt32(command.ExecuteScalar());
    }

    private static int? GetIdByTextField(SqliteConnection connection, SqliteTransaction transaction, string tableName, string fieldName, string value)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = $"SELECT Id FROM {tableName} WHERE {fieldName} = $value LIMIT 1;";
        command.Parameters.AddWithValue("$value", value);

        object? id = command.ExecuteScalar();
        return id is null ? null : Convert.ToInt32(id);
    }
}
