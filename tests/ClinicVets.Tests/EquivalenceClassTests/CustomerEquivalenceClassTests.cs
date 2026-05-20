using ClinicVets.Validators;

namespace ClinicVets.Tests.EquivalenceClassTests;

public class CustomerEquivalenceClassTests
{
    [Fact]
    public void Phone_ValidPartition_AcceptsNumericIsraeliStylePhone()
    {
        Assert.True(ValidationRules.IsPhoneValid("0501234567"));
    }

    [Fact]
    public void Phone_InvalidPartition_RejectsLetters()
    {
        Assert.False(ValidationRules.IsPhoneValid("050ABC4567"));
    }

    [Theory]
    [InlineData("dana.levi@gmail.com", true)]
    [InlineData("dana-gmail.com", false)]
    public void Email_ValidAndInvalidPartitions(string email, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsEmailValid(email));
    }

    [Theory]
    [InlineData("Dana Levi", true)]
    [InlineData("דנה לוי", true)]
    [InlineData("Dana1 Levi", false)]
    [InlineData("Dana-Levi", false)]
    public void CustomerName_ValidAndInvalidPartitions(string name, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsEnglishOrHebrewName(name));
    }
}
