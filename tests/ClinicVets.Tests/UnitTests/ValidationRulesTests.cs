using ClinicVets.Validators;

namespace ClinicVets.Tests.UnitTests;

public class ValidationRulesTests
{
    [Theory]
    [InlineData("abcDEF")]
    [InlineData("abc12DE")]
    [InlineData("AB12cdEF")]
    public void IsUsernameValid_AcceptsValidUsernames(string username)
    {
        Assert.True(ValidationRules.IsUsernameValid(username));
    }

    [Theory]
    [InlineData("abc12")]
    [InlineData("abcDEF123")]
    [InlineData("abc123D")]
    [InlineData("abc_def")]
    [InlineData("אבג123")]
    public void IsUsernameValid_RejectsInvalidUsernames(string username)
    {
        Assert.False(ValidationRules.IsUsernameValid(username));
    }

    [Theory]
    [InlineData("Abcdef1!")]
    [InlineData("Abcdef12#$")]
    public void IsPasswordValid_AcceptsValidPasswords(string password)
    {
        Assert.True(ValidationRules.IsPasswordValid(password));
    }

    [Theory]
    [InlineData("Abcdef1")]
    [InlineData("Abcdefgh1!1")]
    [InlineData("Abcdef12")]
    [InlineData("Abcdefg!")]
    [InlineData("1234567!")]
    [InlineData("Abcdef1@")]
    public void IsPasswordValid_RejectsInvalidPasswords(string password)
    {
        Assert.False(ValidationRules.IsPasswordValid(password));
    }

    [Theory]
    [InlineData("1234", true)]
    [InlineData("123", false)]
    [InlineData("12345", false)]
    [InlineData("12A4", false)]
    public void IsEmployeeNumberValid_ChecksFourDigitFormat(string employeeNumber, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsEmployeeNumberValid(employeeNumber));
    }

    [Theory]
    [InlineData("123456782", true)]
    [InlineData("123456789", true)]
    [InlineData("12345678", false)]
    [InlineData("1234567820", false)]
    [InlineData("12345A782", false)]
    public void IsIdentityNumberValid_ChecksNineDigitNumericFormat(string identityNumber, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsIdentityNumberValid(identityNumber));
    }

    [Theory]
    [InlineData("person@gmail.com", true)]
    [InlineData("person.name@mail.co.il", true)]
    [InlineData("persongmail.com", false)]
    [InlineData("person@", false)]
    [InlineData("person example@gmail.com", false)]
    public void IsEmailValid_ChecksEmailFormat(string email, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsEmailValid(email));
    }

    [Theory]
    [InlineData("0501234567", true)]
    [InlineData("021234567", true)]
    [InlineData("501234567", false)]
    [InlineData("050123456", true)]
    [InlineData("05012345678", false)]
    [InlineData("050ABC4567", false)]
    public void IsPhoneValid_ChecksPhoneFormat(string phone, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsPhoneValid(phone));
    }
}
