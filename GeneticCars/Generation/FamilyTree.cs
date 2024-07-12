namespace GeneticCars.Generation;

public partial class FamilyTree
{
    public List<Node[]> Generations { get; } = new(100);

    public readonly struct Node(Individual individual, int? ancestor1Index, int? ancestor2Index)
    {
        public readonly Class Class = individual.Class;
        public readonly float Fitness = individual.Fitness;
        public readonly string Text = individual.Identity.InfoText;
        public readonly int? Ancestor1Index = ancestor1Index;
        public readonly int? Ancestor2Index = ancestor2Index;

        public override string ToString() => $"{Text} ({Fitness}) {Class}, ancestors [{Ancestor1Index}, {Ancestor2Index}]";
    }

    public void AddGeneration(IEnumerable<Individual> individuals)
    {
        Node[] newGeneration;
        if (Generations.Count > 0) {
            Node[] ancestors = Generations[^1];
            var indices = new Dictionary<string, int>(ancestors.Length * 3 / 2, StringComparer.Ordinal);
            for (int i = 0; i < ancestors.Length; i++) {
                indices[ancestors[i].Text] = i;
            }
            newGeneration = individuals
                .OrderByDescending(i => i.Fitness)
                .Select(i => new Node(i, GetIndex(i.Ancestor1, indices), GetIndex(i.Ancestor2, indices)))
                .ToArray();
        } else {
            newGeneration = individuals
                .OrderByDescending(i => i.Fitness)
                .Select(i => new Node(i, null, null))
                .ToArray();
        }
        Generations.Add(newGeneration);
    }

    private int? GetIndex(Individual? ancestor, Dictionary<string, int> indices) =>
        ancestor is null ? null : indices.TryGetValue(ancestor.Identity.InfoText, out int index) ? index : null;
}
