using GeneticCars.Cars;
using GeneticCars.Generation;

namespace UnitTests;

[TestClass]
public class Genes
{
    [TestMethod]
    public void IndexRange()
    {
        HashSet<float> values = [];
        for (int i = 0; i < 1000; i++) {
            Gene sut = new(Car.WheelIndexRange);
            values.Add(sut.IntValue);
        }
        Assert.AreEqual(8, values.Count, "Count");
        Assert.AreEqual(0, values.Min(), "Min");
        Assert.AreEqual(7, values.Max(), "Max");
    }

    [TestMethod]
    public void ValueRange()
    {
        List<float> values = [];
        for (int i = 0; i < 1000; i++) {
            Gene sut = new(Car.WheelDensityRange);
            values.Add(sut.Value);
        }
        Assert.IsTrue(values.Min() >= Car.WheelDensityRange.Min, "Min");
        Assert.IsTrue(values.Max() <= Car.WheelDensityRange.Min + Car.WheelDensityRange.Range, "Max");
    }

    [TestMethod]
    public void FractionBetween0And1()
    {
        float min = Single.MaxValue, max = Single.MinValue;
        for (int i = 0; i < 1000; i++) {
            Gene sut = new(Car.WheelRadiusRange);
            min = Math.Min(min, sut.Fraction);
            max = Math.Max(max, sut.Fraction);
        }
        Assert.IsTrue(min >= 0f, "Min");
        Assert.IsTrue(max <= 1f, "Max");
    }
}