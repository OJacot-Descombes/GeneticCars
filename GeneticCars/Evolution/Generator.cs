namespace GeneticCars.Evolution;

public class Generator<T>
    where T : Individual, IIndividualFactory<T>
{
    private const float MutationRate = 0.20f;
    private const float MutationSize = 0.30f;
    private const float MaxBoostFitnessDifference = 0.1f;
    private const float KryptoniteLimitSquare = 0.12f;

    private static readonly Dictionary<(int, string), int> _namePool = new(2000);
    private int _keepAliveCount = 1;

    public void GenerateInitial(World world, T[] individuals, Vector2 position)
    {
        for (int i = 0; i < individuals.Length; i++) {
            var individual = T.CreateRandom(world, position);
            individuals[i] = individual;
        }
    }

    public void Evolve(World world, T[] individuals, Vector2 position, Parameters parameters)
    {
        Array.Sort(individuals, (a, b) => (-a.Fitness, a.Identity).CompareTo((-b.Fitness, b.Identity)));
        int newCreationsDestination;
        var genomeHashes = new HashSet<int>();
        if (parameters.Death) {
            newCreationsDestination = Extinguish(world, individuals, position, parameters, genomeHashes);
        } else {
            var source = (T[])individuals.Clone();
            int eliteCount = source.Length / 4;

            var eliteSpan = source.AsSpan(0, eliteCount);
            if (parameters.MutationBoost) {
                BoostElite(eliteSpan, individuals.AsSpan(0, eliteCount), genomeHashes, world, position);
            } else {
                CloneElite(eliteSpan, individuals.AsSpan(0, eliteCount), genomeHashes, world, position);
            }

            var destination = individuals.AsSpan(eliteCount, eliteCount);
            CreateCrossovers(source, destination, genomeHashes, world, position);

            destination = individuals.AsSpan(2 * eliteCount, eliteCount);
            CreateMutations(eliteSpan, destination, genomeHashes, world, position);

            if (parameters.Kryptonite) {
                Span<T> kryptoniteSpan = individuals.AsSpan(eliteCount, 2 * eliteCount); // Crossovers and mutations.
                KillSimilarTwins(kryptoniteSpan, world, position);
            }

            newCreationsDestination = 3 * eliteCount;
        }
        while (newCreationsDestination < individuals.Length) {
            var individual = T.CreateRandom(world, position);
            individuals[newCreationsDestination++] = individual;
        }
    }

    private int Extinguish(World world, T[] individuals, Vector2 position, Parameters parameters, HashSet<int> genomeHashes)
    {
        parameters.Death = false;
        var distinctElite = individuals.DistinctBy(i => i.Identity.Name)
            .Take(_keepAliveCount)
            .ToArray();
        Array.Copy(distinctElite, individuals, distinctElite.Length);
        var eliteSpan = individuals.AsSpan();
        Generator<T>.CloneElite(eliteSpan, eliteSpan, genomeHashes, world, position);
        _keepAliveCount++;
        return distinctElite.Length;
    }

    private static bool KillableByKryptonite(Span<T> span, int i) // e.g. 0.05
    {
        for (int j = i - 1; j >= 0; j--) {
            if (span[i].GenomeDistanceSquaredTo(span[j]) < KryptoniteLimitSquare) {
                return true;
            }
        }
        return false;
    }

    public static bool KillableByKryptoniteCountIsAtLeast(T[] individuals, int minimalCount)
    {
        int eliteCount = individuals.Length / 4;
        Span<T> span = individuals.AsSpan(eliteCount, 2 * eliteCount);
        int count = 0;
        for (int i = 1; i < span.Length; i++) {
            for (int j = i - 1; j >= 0; j--) {
                if (span[i].GenomeDistanceSquaredTo(span[j]) < KryptoniteLimitSquare) {
                    count++;
                    if (count >= minimalCount) {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private static void CloneElite(Span<T> sourceSpan, Span<T> destinationSpan, HashSet<int> genomeHashes,
        World world, Vector2 position)
    {
        for (int i = 0; i < sourceSpan.Length; i++) {
            T old = sourceSpan[i];
            var elite = T.Create(Class.Elite, old.Genome, old.Identity, old, null, world, position);
            destinationSpan[i] = elite;
            genomeHashes.Add(elite.GetGenomeHashCode());
        }
    }

    public static int CountBoostable(IEnumerable<float> eliteFitness)
    {
        float lastFitness = Single.MaxValue;
        int boostableCount = 0;
        foreach (float fitness in eliteFitness) {
            if (fitness < lastFitness - MaxBoostFitnessDifference) {
                lastFitness = fitness;
            } else {
                boostableCount++;
            }
        }
        return boostableCount;
    }

    private static void BoostElite(Span<T> sourceSpan, Span<T> destinationSpan, HashSet<int> genomeHashes,
        World world, Vector2 position)
    {
        float lastFitness = Single.MaxValue;
        for (int i = 0; i < sourceSpan.Length; i++) {
            T old = sourceSpan[i];
            T elite;
            if (old.Fitness < lastFitness - MaxBoostFitnessDifference) {
                elite = T.Create(Class.Elite, old.Genome, old.Identity, old, null, world, position);
                lastFitness = old.Fitness;
            } else {
                elite = CreateMutation(old, genomeHashes, world, position, boosted: true);
            }
            destinationSpan[i] = elite;
            genomeHashes.Add(elite.GetGenomeHashCode());
        }
    }

    private static void CreateCrossovers(T[] individuals, Span<T> destination,
        HashSet<int> genomeHashes, World world, Vector2 position)
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

    private static void CreateMutations(Span<T> eliteSpan, Span<T> destination,
        HashSet<int> genomeHashes, World world, Vector2 position)
    {
        for (int i = 0; i < eliteSpan.Length; i++) {
            T elite = eliteSpan[i];
            destination[i] = CreateMutation(elite, genomeHashes, world, position);
            genomeHashes.Add(destination[i].GetGenomeHashCode());
        }
    }

    private static T CreateMutation(T elite, HashSet<int> genomeHashes, World world, Vector2 position, bool boosted = false)
    {
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
            newIndividual = T.Create(boosted ? Class.Boosted : Class.Mutated, genes, new Identity(elite.Identity.Name, generation, number),
                elite, null, world, position);
        } while (genomeHashes.Contains(newIndividual.GetGenomeHashCode()) && tries++ < 5);
        if (tries >= 5) {
            return T.CreateRandom(world, position);
        } else {
            return newIndividual;
        }
    }

    private static void Mutate(Gene[] genes, int i)
    {
        Gene gene = genes[i];
        float oldFraction = gene.Fraction;
        float fraction;
        do {
            float delta = 2.0f * MutationSize * (Random.Shared.NextSingle() - 0.5f);
            fraction = Single.Clamp(gene.Fraction + delta, 0f, 0.99999997f);
        } while (fraction == oldFraction);
        genes[i] = new Gene(gene.Range, fraction); ;
    }

    private static void KillSimilarTwins(Span<T> kryptoniteSpan, World world, Vector2 position)
    {
        for (int i = 0; i < kryptoniteSpan.Length; i++) {
            if (KillableByKryptonite(kryptoniteSpan, i)) {
                kryptoniteSpan[i] = T.CreateRandom(world, position, Class.Kryptonite);
            }
        }
    }
}
