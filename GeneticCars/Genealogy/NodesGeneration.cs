using static GeneticCars.Genealogy.FamilyTree;

namespace GeneticCars.Genealogy;

public class NodesGeneration : Generation<Node>
{
    internal (int g, int i) _cashedSelection;
    internal readonly HashSet<int> _relatedNodesCash = [];
    internal readonly HashSet<int> _relatedAncestorsCash = [];

    public NodesGeneration(Node[] population, GenerationBase template) : base(population)
    {
        RadioactivityApplied = template.RadioactivityApplied;
        KryptoniteApplied = template.KryptoniteApplied;
        DeathApplied = template.DeathApplied;
        NewFloorGenerated = template.NewFloorGenerated;
        MutationText = template.MutationText;
    }
}
