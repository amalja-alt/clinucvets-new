using ClinicVets.Models;
using ClinicVets.Repositories;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.EndToEndTests;

public class AssignmentFlowEndToEndTests
{
    [Fact]
    public void SecretaryFlow_RegisterEmployeeLoginRegisterCustomerSearchAndViewLinkedAnimals_UsesRealSqlite()
    {
        using SqliteTestDatabase database = new();
        database.Initialize();

        EmployeeRepository employees = new(database.ConnectionString);
        CustomerRepository customers = new(database.ConnectionString);
        AnimalRepository animals = new(database.ConnectionString);
        EmployeeService employeeService = new(employees, new EmployeeValidator());
        AuthService authService = new(employees, new EmployeeValidator());
        CustomerService customerService = new(customers, animals, new CustomerValidator());

        OperationResult<Employee> employeeResult = employeeService.RegisterEmployee(
            "secret1",
            "Secret#1",
            "9002",
            "secretary@clinicvets.com",
            "100000009",
            StaffRole.Secretary);
        AuthenticationResult loginResult = authService.Login("secret1", "Secret#1");
        OperationResult<Customer> customerResult = customerService.RegisterCustomer(
            authService.CurrentUser,
            "Dana Levi",
            "123456782",
            "0501234567",
            "dana.levi@gmail.com");
        Animal linkedAnimal = animals.Add(new Animal
        {
            Name = "Luna",
            ChipNumber = "CAT-2001",
            Type = AnimalType.Cat,
            WeightKg = 4.3m,
            BirthDate = new DateOnly(2022, 8, 5),
            LastVaccinationDate = new DateOnly(2025, 5, 1),
            OwnerCustomerId = customerResult.Value!.Id
        });

        OperationResult<Customer?> searchResult = customerService.SearchByIdentityOrPhone(authService.CurrentUser, "050-123-4567");
        OperationResult<IReadOnlyList<Animal>> animalsResult = customerService.GetCustomerAnimals(authService.CurrentUser, customerResult.Value.Id);

        Assert.True(employeeResult.IsSuccess);
        Assert.True(loginResult.IsSuccess);
        Assert.True(customerResult.IsSuccess);
        Assert.Equal(customerResult.Value.Id, searchResult.Value?.Id);
        Animal animal = Assert.Single(animalsResult.Value!);
        Assert.Equal(linkedAnimal.Id, animal.Id);
    }
}
