using ClinicVets.Validators;

namespace ClinicVets.Tests.BoundaryTests;

public class CustomerBoundaryTests
{
    [Theory]
    [InlineData("12345678", false)]
    [InlineData("123456789", true)]
    [InlineData("1234567890", false)]
    public void IsraeliIdLength_BoundaryValues_8_9_10(string identityNumber, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsIdentityNumberValid(identityNumber));
    }

    [Theory]
    [InlineData("05012345", false)]
    [InlineData("050123456", true)]
    [InlineData("0501234567", true)]
    [InlineData("05012345678", false)]
    public void PhoneLength_BoundaryValues_8_9_10_11(string phone, bool expected)
    {
        Assert.Equal(expected, ValidationRules.IsPhoneValid(phone));
    }
}
