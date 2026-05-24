using Xunit;

namespace ClinicVets.Tests.BoundaryTests;

public class AnimalBoundaryTests
{
    [Theory]
    [InlineData(0.09, false)]
    [InlineData(0.1, true)]
    [InlineData(100, true)]
    [InlineData(100.1, false)]
    public void WeightBoundaryTest(double weight, bool expected)
    {
        bool result = weight >= 0.1 && weight <= 100;

        Assert.Equal(expected, result);
    }
}
[Theory]
[InlineData(1999, false)]
[InlineData(2000, true)]
[InlineData(2025, true)]
public void BirthYearBoundaryTest(int year, bool expected)
{
    bool result = year >= 2000 && year <= DateTime.Now.Year;

    Assert.Equal(expected, result);
}