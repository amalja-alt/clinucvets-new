using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Tests.TestSupport;
using Microsoft.Data.Sqlite;

namespace ClinicVets.Tests.IntegrationTests;

public class RepositorySqliteIntegrationTests
{
    [Fact]
    public void EmployeeRepository_AddThenFindByUsername_PersistsEmployeeWithRole()
    {
        using SqliteTestDatabase database = InitializedDatabase();
        EmployeeRepository employees = new(database.ConnectionString);

        Employee saved = employees.Add(new Employee
        {
            Username = "worker1",
            PasswordHash = "Worker#1",
            EmployeeNumber = "1234",
            Email = "worker@clinicvets.com",
            IdentityNumber = "123456782",
            Role = StaffRole.Secretary
        });

        Employee? found = employees.FindByUsername("worker1");

        Assert.NotEqual(0, saved.Id);
        Assert.NotNull(found);
        Assert.Equal(saved.Id, found!.Id);
        Assert.Equal(StaffRole.Secretary, found.Role);
        Assert.True(employees.ExistsByRegistrationFields("newusr", "1234", "new@clinicvets.com", "100000009"));
    }

    [Theory]
    [InlineData("worker1", "5678", "other@clinicvets.com", "100000009")]
    [InlineData("other1", "1234", "other@clinicvets.com", "100000009")]
    [InlineData("other1", "5678", "worker@clinicvets.com", "100000009")]
    [InlineData("other1", "5678", "other@clinicvets.com", "123456782")]
    public void EmployeeRepository_ExistsByRegistrationFields_DetectsEachUniqueField(
        string username,
        string employeeNumber,
        string email,
        string identityNumber)
    {
        using SqliteTestDatabase database = InitializedDatabase();
        EmployeeRepository employees = new(database.ConnectionString);
        employees.Add(new Employee
        {
            Username = "worker1",
            PasswordHash = "Worker#1",
            EmployeeNumber = "1234",
            Email = "worker@clinicvets.com",
            IdentityNumber = "123456782",
            Role = StaffRole.Secretary
        });

        Assert.True(employees.ExistsByRegistrationFields(username, employeeNumber, email, identityNumber));
    }

    [Fact]
    public void CustomerRepository_AddThenSearchByIdentityAndPhone_PersistsCustomer()
    {
        using SqliteTestDatabase database = InitializedDatabase();
        CustomerRepository customers = new(database.ConnectionString);

        Customer saved = customers.Add(new Customer
        {
            FullName = "Dana Levi",
            IdentityNumber = "123456782",
            Phone = "0501234567",
            Email = "dana.levi@gmail.com"
        });

        Customer? byIdentity = customers.FindByIdentityOrPhone("123456782");
        Customer? byPhone = customers.FindByIdentityOrPhone("0501234567");

        Assert.NotEqual(0, saved.Id);
        Assert.Equal(saved.Id, byIdentity?.Id);
        Assert.Equal(saved.Id, byPhone?.Id);
        Assert.True(customers.ExistsByIdentityNumber("123456782"));
    }

    [Fact]
    public void CustomerRepository_DuplicateEmail_IsRejectedBySqliteUniqueConstraint()
    {
        using SqliteTestDatabase database = InitializedDatabase();
        CustomerRepository customers = new(database.ConnectionString);
        customers.Add(new Customer
        {
            FullName = "Dana Levi",
            IdentityNumber = "123456782",
            Phone = "0501234567",
            Email = "same.email@gmail.com"
        });

        Assert.Throws<SqliteException>(() => customers.Add(new Customer
        {
            FullName = "Dana Other",
            IdentityNumber = "234567899",
            Phone = "0527654321",
            Email = "same.email@gmail.com"
        }));
    }

    [Fact]
    public void AnimalRepository_FindByOwnerCustomerId_ReturnsOnlyLinkedAnimalsFromSqlite()
    {
        using SqliteTestDatabase database = InitializedDatabase();
        CustomerRepository customers = new(database.ConnectionString);
        AnimalRepository animals = new(database.ConnectionString);
        Customer owner = customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana.levi@gmail.com" });
        Customer otherOwner = customers.Add(new Customer { FullName = "Noam Cohen", IdentityNumber = "234567899", Phone = "0527654321", Email = "noam.cohen@gmail.com" });
        Animal linked = animals.Add(NewAnimal("Luna", "CAT-2001", AnimalType.Cat, owner.Id));
        animals.Add(NewAnimal("Buddy", "DOG-1001", AnimalType.Dog, otherOwner.Id));

        IReadOnlyList<Animal> result = animals.FindByOwnerCustomerId(owner.Id);

        Animal animal = Assert.Single(result);
        Assert.Equal(linked.Id, animal.Id);
        Assert.Equal(owner.Id, animal.OwnerCustomerId);
    }

    private static SqliteTestDatabase InitializedDatabase()
    {
        SqliteTestDatabase database = new();
        database.Initialize();
        return database;
    }

    private static Animal NewAnimal(string name, string chipNumber, AnimalType type, int ownerCustomerId)
    {
        return new Animal
        {
            Name = name,
            ChipNumber = chipNumber,
            Type = type,
            WeightKg = 4.3m,
            BirthDate = new DateOnly(2022, 8, 5),
            LastVaccinationDate = new DateOnly(2025, 5, 1),
            OwnerCustomerId = ownerCustomerId
        };
    }
}
