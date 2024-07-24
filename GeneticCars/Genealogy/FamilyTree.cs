using GeneticCars.Cars;
using GeneticCars.Evolution;

namespace GeneticCars.Genealogy;

public partial class FamilyTree
{
    public List<NodesGeneration> Generations { get; } = new(100);

    public void UpdateScoredGeneration(Generation<Car> carGeneration)
    {
        Node[] updatedPopulation;
        if (Generations.Count > 1) { // Unscored generation is already there
            Node[] ancestors = Generations[^2].Population;
            var indices = new Dictionary<string, int>(ancestors.Length * 3 / 2, StringComparer.Ordinal);
            for (int i = 0; i < ancestors.Length; i++) {
                indices[ancestors[i].Text] = i;
            }
            updatedPopulation = carGeneration.Population
                .OrderByDescending(i => i.Fitness)
                .ThenBy(i => i.Identity)
                .Select(i =>
                    new Node(i, GetIndex(i.Ancestor1, indices), GetIndex(i.Ancestor2, indices), FitnessHandling.Known))
                .ToArray();
        } else {
            updatedPopulation = carGeneration.Population
                .OrderByDescending(i => i.Fitness)
                .ThenBy(i => i.Identity)
                .Select(i => new Node(i, null, null, FitnessHandling.Known))
                .ToArray();
        }
        Generations[^1].Population = updatedPopulation;
    }

    public void AddUnscoredGeneration(Generation<Car> carGeneration)
    {
        Node[] newPopulation;
        if (Generations.Count > 0) {
            Node[] ancestors = Generations[^1].Population;
            var indices = new Dictionary<string, int>(ancestors.Length * 3 / 2, StringComparer.Ordinal);
            for (int i = 0; i < ancestors.Length; i++) {
                indices[ancestors[i].Text] = i;
            }
            newPopulation = carGeneration.Population
                .Select(ind => new Node(ind, GetIndex(ind.Ancestor1, indices), GetIndex(ind.Ancestor2, indices),
                    ind.Class == Class.Elite && !carGeneration.NewFloorGenerated 
                        ? FitnessHandling.Inherit
                        : FitnessHandling.Unknown))
                .ToArray();
        } else {
            newPopulation = carGeneration.Population
                .Select(ind => new Node(ind, null, null, FitnessHandling.Unknown))
                .ToArray();
        }
        Generations.Add(new NodesGeneration(newPopulation, carGeneration));
    }

    private static int? GetIndex(Individual? ancestor, Dictionary<string, int> indices) =>
        ancestor is null ? null : indices.TryGetValue(ancestor.Identity.InfoText, out int index) ? index : null;
}
