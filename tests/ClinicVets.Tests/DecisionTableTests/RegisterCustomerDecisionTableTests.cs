using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.DecisionTableTests;

public class RegisterCustomerDecisionTableTests
{
    [Fact]
    public void Secretary_ValidData_NoDuplicate_ReturnsSuccess()
    {
        CustomerService service = new(new FakeCustomerRepository(), new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Secretary(), "Dana Levi", "123456782", "0501234567", "dana@example.com");

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void Veterinarian_ValidData_NoDuplicate_ReturnsAuthorizationFailure()
    {
        CustomerService service = new(new FakeCustomerRepository(), new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Veterinarian(), "Dana Levi", "123456782", "0501234567", "dana@example.com");

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.SecretaryOnly, result.ErrorMessage);
    }

    [Theory]
    [InlineData("Dana1 Levi", "123456782", "0501234567", "dana@example.com", ValidationMessages.InvalidFullName)]
    [InlineData("Dana Levi", "12345678A", "0501234567", "dana@example.com", ValidationMessages.InvalidIsraeliIdentityNumber)]
    [InlineData("Dana Levi", "123456782", "501234567", "dana@example.com", ValidationMessages.InvalidPhone)]
    [InlineData("Dana Levi", "123456782", "0501234567", "dana-example.com", ValidationMessages.InvalidEmail)]
    public void Secretary_InvalidCustomerData_ReturnsValidationFailure(
        string fullName,
        string identityNumber,
        string phone,
        string email,
        string expectedMessage)
    {
        CustomerService service = new(new FakeCustomerRepository(), new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Secretary(), fullName, identityNumber, phone, email);

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedMessage, result.ErrorMessage);
    }

    [Fact]
    public void Secretary_ValidData_DuplicateIdentity_ReturnsDuplicateFailure()
    {
        FakeCustomerRepository customers = new();
        customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana@example.com" });
        CustomerService service = new(customers, new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer> result = service.RegisterCustomer(TestEmployees.Secretary(), "Dana Other", "123456782", "0527654321", "other@example.com");

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.DuplicateCustomer, result.ErrorMessage);
    }
}
