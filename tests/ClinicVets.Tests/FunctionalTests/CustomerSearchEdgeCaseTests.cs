using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.FunctionalTests;

public class CustomerSearchEdgeCaseTests
{
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("not-a-phone-or-id")]
    public void SearchCustomer_WithBlankOrNonNumericText_ReturnsSuccessWithNullValue(string searchText)
    {
        FakeCustomerRepository customers = new();
        customers.Add(new Customer { FullName = "Dana Levi", IdentityNumber = "123456782", Phone = "0501234567", Email = "dana.levi@gmail.com" });
        CustomerService service = new(customers, new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<Customer?> result = service.SearchByIdentityOrPhone(TestEmployees.Secretary(), searchText);

        Assert.True(result.IsSuccess);
        Assert.Null(result.Value);
    }
}
