using Microsoft.Data.Sqlite;
namespace ClinicVets.Data;
// in this class we init the database  ( sql )
// add a defult data to the structure 
// so wwe can work with it to the demo and the tests
public class ClinicDatabaseInitializer(string connectionString)
{
    // the function :
    // this is the intilization function 
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
        SeedAnimalCategories(connection);
    }

    // to make a connection with the sql tables and create the tables if they not exist
    private static void EnableForeignKeys(SqliteConnection connection)
    {
        using SqliteCommand command = connection.CreateCommand();
        command.CommandText = "PRAGMA foreign_keys = ON;";
        command.ExecuteNonQuery();
    }
    // make the roles table with the fields : id and name
    // id for the role and the name secretary or veterinarian
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
    // create the worker (employee) table 
    // with the details that we need to log in 
    // name , password , employee number , email , identity number and the role ( here we save the id that connect to the role table) 
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

    // create the costomer table with the details that we need to save about the customer
    // name , identity number , phone and email
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

    // create the animal category table with the details that we need to save about the category
    // name of the category ( dog , cat , bird and reptile) 
    // the is is for the connection with the animal table 
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

    // create the animal table with the details that we need to save about the animal
    // name , chip number , category id ( connection with the category table) , weight , birth date , 
    // last vaccination date and the owner costomer id ( connection with the costomer table)
    // here we have a connection with the animal category table and the costomer table
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

    // create the medicine table with the details that we need to save about the medicine
    // name , price and quantity in stock
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

    // create the visit table with the details that we need to save about the visit
    // animal id ( connection with the animal table) , veterinarian employee id , visit date and time , 
    // reason for the visit , diagnosis and the base price for the visit
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

    // make the roles table we have just 2 roles of the worker 
    private static void SeedRoles(SqliteConnection connection)
    {
        insertroles(connection, "Veterinarian");
        insertroles(connection, "Secretary");
    }

    private static void insertroles(SqliteConnection connection, string roleName)
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

    // make the animal category table we have just 4 categories for the animals
    private static void SeedAnimalCategories(SqliteConnection connection)
    {
        InsertAnimalC(connection, "Dog");
        InsertAnimalC(connection, "Cat");
        InsertAnimalC(connection, "Reptile");
        InsertAnimalC(connection, "Bird");
    }
    // private function to add the category to the category table if it not exist
    // we have a 4 categories for the animals : dog , cat , reptile and bird - from the assignmnet 
    // we cant delete this function 
    private static void InsertAnimalC(SqliteConnection connection, string categoryName)
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
}
