using GeneticCars.Evolution;

namespace GeneticCars.Genealogy;

public readonly struct Node(Individual individual, int? ancestor1Index, int? ancestor2Index,
    FitnessHandling fitnessHandling)
{
    public Class Class { get; init; } = individual.Class;
    public float? Fitness { get; init; } = fitnessHandling switch {
        FitnessHandling.Known => individual.Fitness,
        FitnessHandling.Inherit => individual.Ancestor1?.Fitness,
        _ => null
    };
    public string Text { get; init; } = individual.Identity.InfoText;
    public int? Ancestor1Index { get; init; } = ancestor1Index;
    public int? Ancestor2Index { get; init; } = ancestor2Index;

    public override string ToString() => $"{Text} ({Fitness}) {Class}, ancestors [{Ancestor1Index}, {Ancestor2Index}]";
}
