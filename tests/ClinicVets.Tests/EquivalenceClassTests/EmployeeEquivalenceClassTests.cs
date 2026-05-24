using ClinicVets.Validators;

namespace ClinicVets.Tests.EquivalenceClassTests;

public class EmployeeEquivalenceClassTests
{
    [Fact]
    public void Username_ValidPartition_AcceptsEnglishLettersWithUpToTwoDigits()
    {
        Assert.True(ValidationRules.IsUsernameValid("worker1"));
    }

    [Fact]
    public void Username_InvalidPartition_RejectsTooManyDigits()
    {
        Assert.False(ValidationRules.IsUsernameValid("abc123D"));
    }

    [Fact]
    public void Username_InvalidPartition_RejectsInvalidCharacters()
    {
        Assert.False(ValidationRules.IsUsernameValid("abc_def"));
    }

    [Fact]
    public void Password_ValidPartition_AcceptsRequiredCharacterGroups()
    {
        Assert.True(ValidationRules.IsPasswordValid("Worker#1"));
    }

    [Theory]
    [InlineData("1234567!")]
    [InlineData("Worker#!")]
    [InlineData("Worker12")]
    public void Password_InvalidPartitions_RejectMissingRequiredGroups(string password)
    {
        Assert.False(ValidationRules.IsPasswordValid(password));
    }
}
