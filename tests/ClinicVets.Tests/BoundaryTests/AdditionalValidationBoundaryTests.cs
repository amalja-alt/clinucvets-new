using ClinicVets.Validators;

namespace ClinicVets.Tests.BoundaryTests;

public class AdditionalValidationBoundaryTests
{
    [Theory]
    [InlineData("abcd12", true)]
    [InlineData("abc123", false)]
    public void UsernameDigitCount_BoundaryValues_2And3Digits(string username, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsUsernameValid(username));
    }

    [Theory]
    [InlineData("Abc12345!", true)]
    [InlineData("Abc12345#", true)]
    [InlineData("Abc12345$", true)]
    [InlineData("Abc12345@", false)]
    public void PasswordSpecialCharacter_EquivalenceClasses(string password, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsPasswordValid(password));
    }

    [Theory]
    [InlineData("", false)]
    [InlineData("   ", false)]
    [InlineData("Dana  Levi", false)]
    [InlineData(" Dana Levi ", true)]
    public void CustomerName_WhitespaceEdgeCases(string name, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsEnglishOrHebrewName(name));
    }
}
