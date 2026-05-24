using ClinicVets.Services;
using ClinicVets.Validators;

namespace ClinicVets.Tests.UnitTests;

public class CustomerValidatorTests
{
    private readonly CustomerValidator _validator = new();

    [Fact]
    public void ValidateCustomer_ReturnsSuccess_ForValidCustomerInput()
    {
        OperationResult<bool> result = _validator.ValidateCustomer("Dana Levi", "123456782", "0501234567", "dana.levi@gmail.com");

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("Dana1 Levi", "123456782", "0501234567", "dana.levi@gmail.com", ValidationMessages.InvalidFullName)]
    [InlineData("Dana Levi", "12345678A", "0501234567", "dana.levi@gmail.com", ValidationMessages.InvalidIsraeliIdentityNumber)]
    [InlineData("Dana Levi", "123456782", "501234567", "dana.levi@gmail.com", ValidationMessages.InvalidPhone)]
    [InlineData("Dana Levi", "123456782", "0501234567", "dana-gmail.com", ValidationMessages.InvalidEmail)]
    public void ValidateCustomer_ReturnsClearOracle_ForInvalidInput(
        string fullName,
        string identityNumber,
        string phone,
        string email,
        string expectedMessage)
    {
        OperationResult<bool> result = _validator.ValidateCustomer(fullName, identityNumber, phone, email);

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedMessage, result.ErrorMessage);
    }
}
