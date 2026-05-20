using ClinicVets.Services;
using ClinicVets.Validators;

namespace ClinicVets.Tests.UnitTests;

public class EmployeeValidatorTests
{
    private readonly EmployeeValidator _validator = new();

    [Fact]
    public void ValidateRegistration_ReturnsSuccess_ForValidEmployeeInput()
    {
        OperationResult<bool> result = _validator.ValidateRegistration("worker1", "Worker#1", "1234", "worker@clinicvets.com", "123456782");

        Assert.True(result.IsSuccess);
    }

    [Theory]
    [InlineData("abc12", "Worker#1", "1234", "worker@clinicvets.com", "123456782", ValidationMessages.InvalidUsernameFormat)]
    [InlineData("worker1", "Worker12", "1234", "worker@clinicvets.com", "123456782", ValidationMessages.InvalidPasswordFormat)]
    [InlineData("worker1", "Worker#1", "123", "worker@clinicvets.com", "123456782", ValidationMessages.InvalidEmployeeNumber)]
    [InlineData("worker1", "Worker#1", "1234", "worker-at-clinicvets.com", "123456782", ValidationMessages.InvalidEmail)]
    [InlineData("worker1", "Worker#1", "1234", "worker@clinicvets.com", "12345678A", ValidationMessages.InvalidIsraeliIdentityNumber)]
    public void ValidateRegistration_ReturnsClearOracle_ForInvalidInput(
        string username,
        string password,
        string employeeNumber,
        string email,
        string identityNumber,
        string expectedMessage)
    {
        OperationResult<bool> result = _validator.ValidateRegistration(username, password, employeeNumber, email, identityNumber);

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedMessage, result.ErrorMessage);
    }

    [Fact]
    public void ValidateLogin_ReturnsRequiredOracle_ForEmptyUsername()
    {
        OperationResult<bool> result = _validator.ValidateLogin("", "Worker#1");

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.UsernameRequired, result.ErrorMessage);
    }
}
