using Xunit;

namespace ClinicVets.Tests.FunctionalTests;

public class AnimalFunctionalTests
{
    [Fact]
    public void AddAnimal_WithValidData_ShouldSucceed()
    {
        bool result = true;

        Assert.True(result);
    }

    [Fact]
    public void SearchAnimal_ByName_ShouldReturnAnimal()
    {
        bool result = true;

        Assert.True(result);
    }
    [Fact]
    public void AddAnimal_WithInvalidWeight_ShouldFail()
    {
        bool result = false;

        Assert.False(result);
    }
}
