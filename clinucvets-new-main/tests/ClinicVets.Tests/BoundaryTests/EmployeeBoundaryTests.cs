using ClinicVets.Validators;

namespace ClinicVets.Tests.BoundaryTests;

public class EmployeeBoundaryTests
{
    [Theory]
    [InlineData("abc12", false)]
    [InlineData("abcd12", true)]
    [InlineData("abc12DEF", true)]
    [InlineData("abc12DEFG", false)]
    public void UsernameLength_BoundaryValues_5_6_8_9(string username, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsUsernameValid(username));
    }

    [Theory]
    [InlineData("Abcde1!", false)]
    [InlineData("Abcdef1!", true)]
    [InlineData("Abcdef12#$", true)]
    [InlineData("Abcdef12#$A", false)]
    public void PasswordLength_BoundaryValues_7_8_10_11(string password, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsPasswordValid(password));
    }

    [Theory]
    [InlineData("123", false)]
    [InlineData("1234", true)]
    [InlineData("12345", false)]
    public void EmployeeIdLength_BoundaryValues_3_4_5(string employeeId, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsEmployeeNumberValid(employeeId));
    }

    [Theory]
    [InlineData("12345678", false)]
    [InlineData("123456789", true)]
    [InlineData("1234567890", false)]
    public void IsraeliIdLength_BoundaryValues_8_9_10(string identityNumber, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsIdentityNumberValid(identityNumber));
    }
}
