using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;

namespace ClinicVets.Tests.DecisionTableTests;

public class LoginDecisionTableTests
{
    [Theory]
    [InlineData("", "Secret#1", ValidationMessages.UsernameRequired)]
    [InlineData("secret1", "", ValidationMessages.PasswordRequired)]
    [InlineData("bad", "Secret#1", ValidationMessages.InvalidUsernameFormat)]
    [InlineData("missing1", "Secret#1", ValidationMessages.WrongCredentials)]
    [InlineData("secret1", "Wrong#1", ValidationMessages.WrongCredentials)]
    public void LoginDecisionRows_InvalidOrMissingInputs_ReturnExpectedFailure(
        string username,
        string password,
        string expectedMessage)
    {
        AuthService service = new(RepositoryWithLoginUser(), new EmployeeValidator());

        AuthenticationResult result = service.Login(username, password);

        Assert.False(result.IsSuccess);
        Assert.Equal(expectedMessage, result.ErrorMessage);
        Assert.Null(service.CurrentUser);
    }

    [Fact]
    public void LoginDecisionRow_ValidUserAndPassword_ReturnsSuccess()
    {
        AuthService service = new(RepositoryWithLoginUser(), new EmployeeValidator());

        AuthenticationResult result = service.Login("secret1", "Secret#1");

        Assert.True(result.IsSuccess);
        Assert.Equal("secret1", service.CurrentUser?.Username);
    }

    private static FakeEmployeeRepository RepositoryWithLoginUser()
    {
        FakeEmployeeRepository employees = new();
        employees.Seed(new Employee
        {
            Id = 1,
            Username = "secret1",
            PasswordHash = "Secret#1",
            EmployeeNumber = "9002",
            Email = "secretary@clinicvets.com",
            IdentityNumber = "100000009",
            Role = StaffRole.Secretary
        });
        return employees;
    }
}
