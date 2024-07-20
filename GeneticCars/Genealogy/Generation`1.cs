namespace GeneticCars.Genealogy;

public class Generation<T>(T[] population) : GenerationBase
{
    public T[] Population { get; set; } = population;
}