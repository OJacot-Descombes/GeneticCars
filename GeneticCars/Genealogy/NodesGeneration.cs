using static GeneticCars.Genealogy.FamilyTree;

namespace GeneticCars.Genealogy;

public class NodesGeneration : Generation<Node>
{
    public NodesGeneration(Node[] population, GenerationBase template) : base(population)
    {
        RadioactivityApplied = template.RadioactivityApplied;
        KryptoniteApplied = template.KryptoniteApplied;
        DeathApplied = template.DeathApplied;
        NewFloorGenerated = template.NewFloorGenerated;
    }
}
