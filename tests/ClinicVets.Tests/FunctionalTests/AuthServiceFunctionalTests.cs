using ClinicVets.Models;
using ClinicVets.Services;
using ClinicVets.Tests.TestSupport;
using ClinicVets.Validators;
namespace ClinicVets.Tests.FunctionalTests;

public class AuthServiceFunctionalTests
{
    [Fact]
    public void ValidLogin_Succeeds()
    {
        FakeEmployeeRepository employees = RepositoryWithLoginUser();
        AuthService service = new(employees, new EmployeeValidator());

        AuthenticationResult result = service.Login("secret1", "Secret#1");

        Assert.True(result.IsSuccess);
        Assert.Equal("secret1", result.LoggedInUser?.Username);
        Assert.Equal("secret1", service.CurrentUser?.Username);
    }

    [Fact]
    public void WrongPassword_Fails()
    {
        AuthService service = new(RepositoryWithLoginUser(), new EmployeeValidator());

        AuthenticationResult result = service.Login("secret1", "Wrong#1");

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.WrongCredentials, result.ErrorMessage);
    }

    [Fact]
    public void UnknownUsername_Fails()
    {
        AuthService service = new(RepositoryWithLoginUser(), new EmployeeValidator());

        AuthenticationResult result = service.Login("missing1", "Secret#1");

        Assert.False(result.IsSuccess);
        Assert.Equal(ValidationMessages.WrongCredentials, result.ErrorMessage);
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
