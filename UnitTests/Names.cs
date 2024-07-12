using GeneticCars.Generation;

namespace UnitTests;

[TestClass]
public class Names
{
    [TestMethod]
    public void CombineSimpleNames()
    {
        // Arrange
        var sut = new Name("Kapiguko", "Kapiguko", null);
        var other = new Name("Binesoty", "Binesoty", null);

        // Act
        Name result = sut.CombineWith(other);

        // Assert
        Assert.AreEqual("KapiBine", result.Raw, "Raw");
        Assert.AreEqual("KapiBine", result.Display, "Display");
        Assert.AreEqual("BineKapi", result.Reverse, "Reverse");
    }

    [TestMethod]
    public void CreateComplexName()
    {
        // Arrange
        string raw = $"{Name.UpperShSurrogate}api{Name.ChSurrogate}u{Name.ThSurrogate}o";
        var sut = new Name(raw, Name.ToDisplay(raw), null);

        // Assert
        Assert.AreEqual(raw, sut.Raw, "Raw");
        Assert.AreEqual("Shapichutho", sut.Display, "Display");
    }

    [TestMethod]
    public void CombineComplexNames()
    {
        // Arrange
        string raw = $"{Name.UpperShSurrogate}a{Name.ThSurrogate}iguro";
        var sut = new Name(raw, Name.ToDisplay(raw), null);
        var other = new Name("Binesoty", "Binesoty", null);

        // Act
        Name result = sut.CombineWith(other);

        // Assert
        Assert.AreEqual($"{Name.UpperShSurrogate}a{Name.ThSurrogate}iBine", result.Raw, "Raw");
        Assert.AreEqual("ShathiBine", result.Display, "Display");
        Assert.AreEqual($"Bine{Name.UpperShSurrogate}a{Name.ThSurrogate}i", result.Reverse, "Reverse");
    }
}
