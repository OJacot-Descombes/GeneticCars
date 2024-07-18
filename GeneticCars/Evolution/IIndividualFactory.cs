namespace GeneticCars.Evolution;

public interface IIndividualFactory<T>
    where T : Individual
{
    public static abstract T Create(Class @class, Gene[] genes, Identity identity,
        Individual? ancestor1, Individual? ancestor2,
        World world, Vector2 position);

    public static abstract T CreateRandom(World world, Vector2 position, Class @class = Class.New);
}
