﻿namespace GeneticCars.Generation;

public interface IIndividualFactory<T>
    where T : Individual
{
    public static abstract T Create(Class @class, Gene[] genes, int generation, Name name, World world, Vector2 position);

    public static abstract T CreateRandom(World world, Vector2 position);
}
