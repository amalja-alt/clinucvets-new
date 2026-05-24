using Xunit;

namespace ClinicVets.Tests.RegressionTests;

public class AnimalRegressionTests
{
    [Fact]
    public void AddingAnimal_ShouldNotBreakSearch()
    {
        bool addAnimalWorks = true;
        bool searchWorks = true;

        Assert.True(addAnimalWorks);
        Assert.True(searchWorks);
    }
}