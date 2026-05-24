using Xunit;

namespace ClinicVets.Tests.EquivalenceClassTests;

public class AnimalEquivalenceClassTests
{
    [Fact]
    public void Weight_ValidClass_ShouldPass()
    {
        double weight = 20;

        bool result = weight >= 0.1 && weight <= 100;

        Assert.True(result);
    }

    [Fact]
    public void Weight_TooSmallClass_ShouldFail()
    {
        double weight = -5;

        bool result = weight >= 0.1 && weight <= 100;

        Assert.False(result);
    }

    [Fact]
    public void Weight_TooLargeClass_ShouldFail()
    {
        double weight = 500;

        bool result = weight >= 0.1 && weight <= 100;

        Assert.False(result);
    }
}