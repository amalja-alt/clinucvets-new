using Xunit;

namespace ClinicVets.Tests.DecisionTableTests;

public class AnimalDecisionTableTests
{
    [Theory]
    [InlineData(true, true, true)]
    [InlineData(true, false, false)]
    [InlineData(false, true, false)]
    [InlineData(false, false, false)]
    public void AddAnimalDecisionTable(
        bool validName,
        bool validWeight,
        bool expected)
    {
        bool result = validName && validWeight;

        Assert.Equal(expected, result);
    }
}