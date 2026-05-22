using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.AuthorizationTests;

public class RoleAuthorizationTests
{
    [Fact]
    public void Secretary_CanRegisterCustomer()
    {
        CustomerService service = new(new FakeCustomerRepository(), new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Secretary(), "Dana Levi", "123456782", "0501234567", "dana.levi@gmail.com");

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Veterinarian_CannotRegisterCustomer()
    {
        CustomerService service = new(new FakeCustomerRepository(), new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Veterinarian(), "Dana Levi", "123456782", "0501234567", "dana.levi@gmail.com");

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.SecretaryOnly, result.ErrorMessage);
    }

    [Fact]
    public void Secretary_CanSearchCustomers()
    {
        FakeCustomerRepository customers = new();
        customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana.levi@gmail.com" });
        CustomerService service = new(customers, new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), "123456782");

        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
    }

    [Fact]
    public void Veterinarian_CannotSearchCustomers()
    {
        FakeCustomerRepository customers = new();
        customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana.levi@gmail.com" });
        CustomerService service = new(customers, new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Veterinarian(), "123456782");

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.CustomerManagementSecretaryOnly, result.ErrorMessage);
    }

    [Fact]
    public void Veterinarian_CannotViewCustomerAnimalsThroughCustomerManagement()
    {
        CustomerService service = new(new FakeCustomerRepository(), new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<IReadOnlyList<Animal>> result = service.GetCustomerAnimals(TestEmployees.Veterinarian(), 1);

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.CustomerManagementSecretaryOnly, result.ErrorMessage);
    }

    [Fact]
    public void NullUser_CannotPerformRestrictedCustomerActions()
    {
        CustomerService service = new(new FakeCustomerRepository(), new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer> registerResult = service.RegisterCustomer(null, "Dana Levi", "123456782", "0501234567", "dana.levi@gmail.com");
        OperationResult<Customer?> searchResult = service.SearchByIdentityOrPhone(null, "123456782");

        Assert.False(registerResult.IsSuccess);
        Assert.False(searchResult.IsSuccess);
    }

}
