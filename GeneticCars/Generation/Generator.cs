namespace GeneticCars.Generation;

public class Generator<T>
    where T : Individual, IIndividualFactory<T>
{
    private const float MutationRate = 0.20f;
    private const float MutationSize = 0.30f;

    private static readonly Dictionary<(int, string), int> _namePool = new(2000);

    public void GenerateInitial(World world, T[] individuals, Vector2 position)
    {
        for (int i = 0; i < individuals.Length; i++) {
            var individual = T.CreateRandom(world, position);
            individuals[i] = individual;
        }
    }

    public void Evolve(World world, T[] individuals, Vector2 position)
    {
        Array.Sort(individuals, (a, b) => (-a.Fitness, a.Identity).CompareTo((-b.Fitness, b.Identity)));
        var source = (T[])individuals.Clone();
        int eliteCount = source.Length / 4;
        var genomeHashes = new HashSet<int>();

        var eliteSpan = source.AsSpan(0, eliteCount);
        CloneElite(eliteSpan, individuals.AsSpan(0, eliteCount), genomeHashes, world, position);

        var destination = individuals.AsSpan(eliteCount, eliteCount);
        CreateCrossovers(source, destination, genomeHashes, world, position);

        destination = individuals.AsSpan(2 * eliteCount, eliteCount);
        CreateMutations(eliteSpan, destination, genomeHashes, world, position);

        int dest = 3 * eliteCount;
        while (dest < individuals.Length) {
            var individual = T.CreateRandom(world, position);
            individuals[dest++] = individual;
        }
    }

    private static void CloneElite(Span<T> sourceSpan, Span<T> destinationSpan, HashSet<int> genomeHashes, World world, Vector2 position)
    {
        for (int i = 0; i < sourceSpan.Length; i++) {
            T old = sourceSpan[i];
            var elite = T.Create(Class.Elite, old.Genome, old.Identity, old, null, world, position);
            destinationSpan[i] = elite;
            genomeHashes.Add(elite.GetGenomeHashCode());
        }
    }

    private static void CreateCrossovers(T[] individuals, Span<T> destination, HashSet<int> genomeHashes, World world, Vector2 position)
    {
        int halfPopulation = individuals.Length / 2;
        for (int i = 0; i < destination.Length; i++) {
            T elite = individuals[i];
            T? newIndividual;
            int tries = 0;
            do {
                T other = individuals[Random.Shared.Next(i + 1, halfPopulation)];

                Gene[] genes = [.. elite.Genome];
                for (int g = 0; g < genes.Length; g++) {
                    if (Random.Shared.Next(2) is 0) {
                        genes[g] = other.Genome[g];
                    }
                }

                int generation = Math.Max(elite.Identity.Generation, other.Identity.Generation) + 1;
                Name name = elite.Identity.Name.CombineWith(other.Identity.Name);
                int number = GetNameNumber(generation, name.Display);
                newIndividual = T.Create(Class.Crossed, genes, new Identity(name, generation, number), elite, other,
                    world, position);
            } while (genomeHashes.Contains(newIndividual.GetGenomeHashCode()) && tries++ < 5);
            if (tries >= 5) {
                destination[i] = T.CreateRandom(world, position);
            } else {
                destination[i] = newIndividual;
            }
            genomeHashes.Add(destination[i].GetGenomeHashCode());
        }
    }

    private static int GetNameNumber(int generation, string name)
    {
        if (_namePool.TryGetValue((generation, name), out int number)) {
            number++;
            _namePool[(generation, name)] = number;
            return number;
        }
        _namePool[(generation, name)] = 0;
        return 0;
    }

    private static void CreateMutations(Span<T> eliteSpan, Span<T> destination, HashSet<int> genomeHashes, World world, Vector2 position)
    {
        for (int i = 0; i < eliteSpan.Length; i++) {
            T elite = eliteSpan[i];

            Gene[] genes = [.. elite.Genome];
            T? newIndividual;
            int tries = 0;
            do {
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

                int generation = elite.Identity.Generation + 1;
                int number = GetNameNumber(generation, elite.Identity.Name.Display);
                newIndividual = T.Create(Class.Mutated, genes, new Identity(elite.Identity.Name, generation, number),
                    elite, null, world, position);
            } while (genomeHashes.Contains(newIndividual.GetGenomeHashCode()) && tries++ < 5);
            if (tries >= 5) {
                destination[i] = T.CreateRandom(world, position);
            } else {
                destination[i] = newIndividual;
            }
            genomeHashes.Add(destination[i].GetGenomeHashCode());
        }
    }

    private static void Mutate(Gene[] genes, int i)
    {
        float delta = 2.0f * MutationSize * (Random.Shared.NextSingle() - 0.5f);
        Gene gene = genes[i];
        genes[i] = new Gene(gene.Range, Single.Clamp(gene.Fraction + delta, 0f, 0.99999997f));
    }
}
