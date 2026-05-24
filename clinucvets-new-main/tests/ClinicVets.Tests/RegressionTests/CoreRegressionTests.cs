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

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Secretary(), "Dana Levi", "123456782", "0501234567", "dana.levi@gmail.com");

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void CustomerSearch_CoreFlow_StillWorks()
    {
        FakeCustomerRepository customers = new();
        Customer saved = customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana.levi@gmail.com" });
        CustomerService service = new(customers, new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), saved.Phone);

        Assert.True(result.IsSuccess);
        Assert.Equal(saved.Id, result.Value?.Id);
    }

}
