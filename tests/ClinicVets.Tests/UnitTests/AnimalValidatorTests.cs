using Xunit;

namespace ClinicVets.Tests.UnitTests;

public class AnimalValidatorTests
{
    [Fact]
    public void AnimalName_WithLettersOnly_ShouldPass()
    {
        string name = "Simba";

        bool result = name.All(char.IsLetter);

        Assert.True(result);
    }

    [Fact]
    public void AnimalName_WithNumbers_ShouldFail()
    {
        string name = "Simba123";

        bool result = name.All(char.IsLetter);

        Assert.False(result);
    }
}