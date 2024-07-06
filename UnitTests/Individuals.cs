using GeneticCars.Cars;
using nkast.Aether.Physics2D.Dynamics;

namespace UnitTests;

[TestClass]
public class Individuals
{
    [TestMethod]
    public void IndexRange()
    {
        // Arrange
        var world = new World();
        var car = Car.CreateRandom(0, world, default);
        var sut = new Car(default, car.Genome, 0, "Kapiguko", world, default);
        var other = new Car(default, car.Genome, 0, "Binesoty", world, default);

        // Act
        string result = sut.CombineNames(other);

        // Assert
        Assert.AreEqual("KapiBine", result);
    }
}
