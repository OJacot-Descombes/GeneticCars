namespace GeneticCars.Generation;

public class Generator<T>
    where T : Individual, IIndividualFactory<T>
{
    private const float MutationRate = 0.20f;
    private const float MutationSize = 0.30f;

    public void GenerateInitial(World world, T[] individuals, Vector2 position)
    {
        for (int i = 0; i < individuals.Length; i++) {
            var individual = T.CreateRandom(world, position);
            individuals[i] = individual;
        }
    }

    public void Evolve(World world, T[] individuals, Vector2 position)
    {
        Array.Sort(individuals, (a, b) => -a.Fitness.CompareTo(b.Fitness));
        int eliteCount = individuals.Length / 4;

        var eliteSpan = individuals.AsSpan(0, eliteCount);
        CloneElite(eliteSpan, world, position);

        var destination = individuals.AsSpan(eliteCount, eliteCount);
        CreateCrossovers(individuals, destination, world, position);

        destination = individuals.AsSpan(2 * eliteCount, eliteCount);
        CreateMutations(eliteSpan, destination, world, position);

        int dest = 3 * eliteCount;
        while (dest < individuals.Length) {
            var individual = T.CreateRandom(world, position);
            individuals[dest++] = individual;
        }

        // Ensure uniqueness
        var hashCodes = new HashSet<int>();
        for (int i = 0; i < individuals.Length; i++) {
            T individual = individuals[i];
            int hc = individual.GetHashCode();
            while (hashCodes.Contains(hc)) {
                individual = T.CreateRandom(world, position);
                individuals[i] = individual;
                hc = individual.GetHashCode();
            }
            hashCodes.Add(hc);
        }
    }

    private static void CloneElite(Span<T> eliteSpan, World world, Vector2 position)
    {
        for (int i = 0; i < eliteSpan.Length; i++) {
            T old = eliteSpan[i];
            var elite = T.Create(Class.Elite, old.Genome, old.Generation, old.Name, world, position);
            eliteSpan[i] = elite;
        }
    }

    private static void CreateCrossovers(T[] individuals, Span<T> destination, World world, Vector2 position)
    {
        for (int i = 0; i < destination.Length; i++) {
            T elite = individuals[i];
            T other = Array.Find(individuals, o => o.Name != elite.Name) ?? individuals[i + 1];

            Gene[] genes = [.. elite.Genome];
            for (int g = 0; g < genes.Length; g++) {
                if (Random.Shared.Next(2) is 0) {
                    genes[g] = other.Genome[g];
                }
            }

            var newIndividual = T.Create(Class.Crossed, genes,
                Math.Max(elite.Generation, other.Generation) + 1,
                elite.Name.CombineWith(other.Name), world, position);
            destination[i] = newIndividual;
        }
    }

    private static void CreateMutations(Span<T> eliteSpan, Span<T> destination, World world, Vector2 position)
    {
        for (int i = 0; i < eliteSpan.Length; i++) {
            T elite = eliteSpan[i];

            Gene[] genes = [.. elite.Genome];
            int mutations = 0;
            for (int g = 0; g < genes.Length; g++) {
                if (Random.Shared.NextSingle() < MutationRate) {
                    Mutate(genes, g);
                    mutations++;
                }
            }
            if (mutations == 0) {
                Mutate(genes, Random.Shared.Next(genes.Length));
            }

            var newIndividual = T.Create(Class.Mutated, genes, elite.Generation + 1, elite.Name, world, position);
            destination[i] = newIndividual;
        }
    }

    private static void Mutate(Gene[] genes, int i)
    {
        float delta = 2.0f * MutationSize * (Random.Shared.NextSingle() - 0.5f);
        Gene gene = genes[i];
        genes[i] = new Gene(gene.Range, Single.Clamp(gene.Fraction + delta, 0f, 0.99999997f));
    }
}
