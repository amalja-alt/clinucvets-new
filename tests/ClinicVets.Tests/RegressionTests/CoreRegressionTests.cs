using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.RegressionTests;

public class CoreRegressionTests
{
    [Fact]
    public void Login_CoreFlow_StillWorks()
    {
        FakeEmployeeRepository employees = new();
        employees.Seed(new Employee { Id = 1, Username = "secret1", PasswordHash = "Secret#1", EmployeeNumber = "9002", Email = "secretary@clinicvets.com", IdentityNumber = "100000009", Role = StaffRole.Secretary });
        AuthService service = new(employees, new EmployeeValidator());

        AuthenticationResult result = service.Login("secret1", "Secret#1");

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void EmployeeRegistrationValidation_CoreFlow_StillWorks()
    {
        EmployeeValidator validator = new();

        OperationResult<bool> result = validator.ValidateRegistration("worker1", "Worker#1", "1234", "worker@clinicvets.com", "123456782");

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void CustomerRegistration_CoreFlow_StillWorks()
    {
        CustomerService service = new(new FakeCustomerRepository(), new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Secretary(), "Dana Levi", "123456782", "0501234567", "dana@example.com");

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void CustomerSearch_CoreFlow_StillWorks()
    {
        FakeCustomerRepository customers = new();
        Customer saved = customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana@example.com" });
        CustomerService service = new(customers, new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), saved.Phone);

        Assert.True(result.IsSuccess);
        Assert.Equal(saved.Id, result.Value?.Id);
    }

    [Fact]
    public void AnimalRegistration_CoreFlow_StillWorks()
    {
        FakeCustomerRepository customers = new();
        Customer owner = customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana@example.com" });
        AnimalService service = new(new FakeAnimalRepository(), customers, new AnimalValidator());

        OperationResult<Animal> result = service.AddAnimal("Buddy", "DOG-1001", AnimalType.Dog, 18.5m, new DateOnly(2021, 4, 12), DateOnly.FromDateTime(DateTime.Today), owner.Id);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void VisitService_CoreFlow_StillWorks()
    {
        FakeAnimalRepository animals = new();
        animals.Seed(new Animal { Id = 1, Name = "Buddy", ChipNumber = "DOG-1001", Type = AnimalType.Dog, WeightKg = 18.5m, BirthDate = new DateOnly(2021, 4, 12), LastVaccinationDate = DateOnly.FromDateTime(DateTime.Today), OwnerCustomerId = 1 });
        VisitService service = new(new FakeVisitRepository(), animals, new FakeMedicineRepository(), new VisitValidator());

        OperationResult<Visit> result = service.OpenVisit(TestEmployees.Veterinarian(), 1, "Annual checkup", "Healthy", []);

        Assert.True(result.IsSuccess);
    }
}
