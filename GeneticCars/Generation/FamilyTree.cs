namespace GeneticCars.Generation;

public partial class FamilyTree
{
    public List<Node[]> Generations { get; } = new(100);

    public readonly struct Node(Individual individual, int? ancestor1Index, int? ancestor2Index, bool fitnessKnown)
    {
        public Class Class { get; init; } = individual.Class;
        public float? Fitness { get; init; } = fitnessKnown ? individual.Fitness : null;
        public string Text { get; init; } = individual.Identity.InfoText;
        public int? Ancestor1Index { get; init; } = ancestor1Index;
        public int? Ancestor2Index { get; init; } = ancestor2Index;

        public override string ToString() => $"{Text} ({Fitness}) {Class}, ancestors [{Ancestor1Index}, {Ancestor2Index}]";
    }

    public void UpdateScoredGeneration(IEnumerable<Individual> individuals)
    {
        Node[] newGeneration;
        if (Generations.Count > 1) { // Unscored generation is already there
            Node[] ancestors = Generations[^2];
            var indices = new Dictionary<string, int>(ancestors.Length * 3 / 2, StringComparer.Ordinal);
            for (int i = 0; i < ancestors.Length; i++) {
                indices[ancestors[i].Text] = i;
            }
            newGeneration = individuals
                .OrderByDescending(i => i.Fitness)
                .ThenBy(i => i.Identity)
                .Select(i => new Node(i, GetIndex(i.Ancestor1, indices), GetIndex(i.Ancestor2, indices), true))
                .ToArray();
        } else {
            newGeneration = individuals
                .OrderByDescending(i => i.Fitness)
                .ThenBy(i => i.Identity)
                .Select(i => new Node(i, null, null, true))
                .ToArray();
        }
        Generations[^1] = newGeneration;
    }

    public void AddUnscoredGeneration(IEnumerable<Individual> individuals)
    {
        Node[] newGeneration;
        if (Generations.Count > 0) {
            Node[] ancestors = Generations[^1];
            var indices = new Dictionary<string, int>(ancestors.Length * 3 / 2, StringComparer.Ordinal);
            for (int i = 0; i < ancestors.Length; i++) {
                indices[ancestors[i].Text] = i;
            }
            newGeneration = individuals
                .Select(ind => new Node(ind, GetIndex(ind.Ancestor1, indices), GetIndex(ind.Ancestor2, indices), false))
                .ToArray();
        } else {
            newGeneration = individuals
                .Select(ind => new Node(ind, null, null, false))
                .ToArray();
        }
        Generations.Add(newGeneration);
    }

    private static int? GetIndex(Individual? ancestor, Dictionary<string, int> indices) =>
        ancestor is null ? null : indices.TryGetValue(ancestor.Identity.InfoText, out int index) ? index : null;
}
