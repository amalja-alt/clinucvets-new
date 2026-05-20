using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.FunctionalTests;

public class CustomerServiceFunctionalTests
{
    [Fact]
    public void RegisterCustomer_ForSecretary_Succeeds()
    {
        FakeCustomerRepository customers = new();
        CustomerService service = new(customers, new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Secretary(), "Dana Levi", "123456782", "0501234567", "dana.levi@gmail.com");

        Assert.True(result.IsSuccess);
        Assert.Equal("Dana Levi", result.Value?.FullName);
    }

    [Fact]
    public void RegisterCustomer_WithDuplicateIdentity_Fails()
    {
        FakeCustomerRepository customers = new();
        customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana.levi@gmail.com" });
        CustomerService service = new(customers, new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Secretary(), "Dana Other", "123456782", "0527654321", "dana.other@gmail.com");

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.DuplicateCustomer, result.ErrorMessage);
    }

    [Fact]
    public void SearchCustomer_ByIdentity_Succeeds()
    {
        (CustomerService service, Customer saved) = ServiceWithCustomer();

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), "123456782");

        Assert.True(result.IsSuccess);
        Assert.Equal(saved.Id, result.Value?.Id);
    }

    [Fact]
    public void SearchCustomer_ByPhone_Succeeds()
    {
        (CustomerService service, Customer saved) = ServiceWithCustomer();

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), "0501234567");

        Assert.True(result.IsSuccess);
        Assert.Equal(saved.Id, result.Value?.Id);
    }

    [Fact]
    public void SearchCustomer_NotFound_ReturnsSuccessWithNullValue()
    {
        (CustomerService service, _) = ServiceWithCustomer();

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), "000000000");

        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
        Assert.Equal(string.Empty, result.ErrorMessage);
    }

    private static (CustomerService Service, Customer Saved) ServiceWithCustomer()
    {
        FakeCustomerRepository customers = new();
        Customer saved = customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana.levi@gmail.com" });
        return (new CustomerService(customers, new CustomerValidator()), saved);
    }
}
