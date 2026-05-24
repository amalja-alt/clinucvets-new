using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.AuthorizationTests;

public class CustomerAuthorizationEdgeTests
{
    [Fact]
    public void NullUser_CannotViewCustomerAnimalsThroughCustomerManagement()
    {
        CustomerService service = new(new FakeCustomerRepository(), new FakeAnimalRepository(), new CustomerValidator());

        OperationResult<IReadOnlyList<Animal>> result = service.GetCustomerAnimals(null, 1);

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.CustomerManagementSecretaryOnly, result.ErrorMessage);
    }
}
