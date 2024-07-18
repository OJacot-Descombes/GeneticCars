using GeneticCars.Evolution;

namespace UnitTests;

[TestClass]
public class Names
{
    [TestMethod]
    public void CombineSimpleNames()
    {
        // Arrange
        var sut = new Name("Kapiguko");
        var other = new Name("Binesoty");

        // Act
        Name result = sut.CombineWith(other);

        // Assert
        Assert.AreEqual("KapiBine", result.Raw, "Raw");
        Assert.AreEqual("KapiBine", result.Display, "Display");
    }

    [TestMethod]
    public void CreateComplexName()
    {
        // Arrange
        string raw = $"{Name.UpperShSurrogate}api{Name.ChSurrogate}u{Name.ThSurrogate}o";
        var sut = new Name(raw);

        // Assert
        Assert.AreEqual(raw, sut.Raw, "Raw");
        Assert.AreEqual("Shapichutho", sut.Display, "Display");
    }

    [TestMethod]
    public void CombineComplexNames()
    {
        // Arrange
        string raw = $"{Name.UpperShSurrogate}a{Name.ThSurrogate}iguro";
        var sut = new Name(raw);
        var other = new Name("Binesoty");

        // Act
        Name result = sut.CombineWith(other);

        // Assert
        Assert.AreEqual($"{Name.UpperShSurrogate}a{Name.ThSurrogate}iBine", result.Raw, "Raw");
        Assert.AreEqual("ShathiBine", result.Display, "Display");
    }
}
