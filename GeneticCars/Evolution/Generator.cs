using GeneticCars.Genealogy;

namespace GeneticCars.Evolution;

public class Generator<T>
    where T : Individual, IIndividualFactory<T>
{
    private const float MutationRate = 0.20f;
    private const float MutationSize = 0.30f;
    private const float MaxRadioactivityFitnessDifference = 0.1f;
    private const float KryptoniteLimitSquare = 0.12f;

    private static readonly Dictionary<(int, string), int> _namePool = new(2000);

    public void GenerateInitial(World world, T[] individuals, Vector2 position)
    {
        for (int i = 0; i < individuals.Length; i++) {
            var individual = T.CreateRandom(world, position);
            individuals[i] = individual;
        }
    }

    public void Evolve(World world, Generation<T> generation, Vector2 position, Parameters parameters)
    {
        generation.SaveParameters(parameters);

        var oldPopulation = generation.Population;
        var newPopulation = new T[parameters.PopulationSize];
        generation.Population = newPopulation;

        Array.Sort(oldPopulation, (a, b) => (-a.Fitness, a.Identity).CompareTo((-b.Fitness, b.Identity)));
        int newCreationsDestination;
        var genomeHashes = new HashSet<int>();

        if (parameters.Death.Value) {
            newCreationsDestination =
                Generator<T>.Extinguish(world, oldPopulation, newPopulation, position, parameters, genomeHashes);
        } else {
            int eliteCount = Math.Min(newPopulation.Length / 4, oldPopulation.Length);
            var eliteSpan = oldPopulation.AsSpan(0, eliteCount);
            if (parameters.Radioactivity.Value) {
                IrradiateElite(eliteSpan, newPopulation.AsSpan(0, eliteCount), genomeHashes, world, position);
            } else {
                CloneElite(eliteSpan, newPopulation.AsSpan(0, eliteCount), genomeHashes, world, position);
            }

            var destination = newPopulation.AsSpan(eliteCount, eliteCount);
            CreateCrossovers(oldPopulation, destination, genomeHashes, world, position);

            destination = newPopulation.AsSpan(2 * eliteCount, eliteCount);
            CreateMutations(eliteSpan, destination, genomeHashes, world, position);

            if (parameters.Kryptonite.Value) {
                Span<T> kryptoniteSpan = newPopulation.AsSpan(eliteCount, 2 * eliteCount); // Crossovers and mutations.
                KillSimilarTwins(kryptoniteSpan, world, position);
            }

            newCreationsDestination = 3 * eliteCount;
        }
        while (newCreationsDestination < newPopulation.Length) {
            var individual = T.CreateRandom(world, position);
            newPopulation[newCreationsDestination++] = individual;
        }
    }

    private static int Extinguish(World world, T[] oldPopulation, T[] newPopulation, Vector2 position,
        Parameters parameters, HashSet<int> genomeHashes)
    {
        parameters.Death.Value = false;
        int keepAliveCount = newPopulation.Length / 8;
        var distinctElite = oldPopulation.DistinctBy(i => i.Identity.Name)
            .Take(keepAliveCount)
            .ToArray();
        Generator<T>.CloneElite(distinctElite.AsSpan(), newPopulation.AsSpan(0, distinctElite.Length),
            genomeHashes, world, position);
        return distinctElite.Length;
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

    public static int CountIrradiatable(IEnumerable<float> eliteFitness)
    {
        float lastFitness = Single.MaxValue;
        int irradiatableCount = 0;
        foreach (float fitness in eliteFitness) {
            if (fitness < lastFitness - MaxRadioactivityFitnessDifference) {
                lastFitness = fitness;
            } else {
                irradiatableCount++;
            }
        }
        return irradiatableCount;
    }

    private static void IrradiateElite(Span<T> sourceSpan, Span<T> destinationSpan, HashSet<int> genomeHashes,
        World world, Vector2 position)
    {
        float lastFitness = Single.MaxValue;
        for (int i = 0; i < sourceSpan.Length; i++) {
            T old = sourceSpan[i];
            T elite;
            if (old.Fitness < lastFitness - MaxRadioactivityFitnessDifference) {
                elite = T.Create(Class.Elite, old.Genome, old.Identity, old, null, world, position);
                lastFitness = old.Fitness;
            } else {
                elite = CreateMutation(old, genomeHashes, world, position, irradiated: true);
            }
            destinationSpan[i] = elite;
            genomeHashes.Add(elite.GetGenomeHashCode());
        }
    }

    private static void CreateCrossovers(T[] oldPopulation, Span<T> destination,
        HashSet<int> genomeHashes, World world, Vector2 position)
    {
        int halfPopulation = Math.Max(oldPopulation.Length / 2, 2 * destination.Length);
        for (int i = 0; i < destination.Length; i++) {
            T elite = oldPopulation[i % oldPopulation.Length];
            T? newIndividual;
            int tries = 0;
            do {
                T other = oldPopulation[Random.Shared.Next(i + 1, halfPopulation) % oldPopulation.Length];

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

    private static T CreateMutation(T elite, HashSet<int> genomeHashes, World world, Vector2 position, bool irradiated = false)
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
            newIndividual = T.Create(irradiated ? Class.Radioactive : Class.Mutated, genes, new Identity(elite.Identity.Name, generation, number),
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

    private static bool KillableByKryptonite(Span<T> span, int i) // e.g. 0.05
    {
        for (int j = i - 1; j >= 0; j--) {
            if (span[i].GenomeDistanceSquaredTo(span[j]) < KryptoniteLimitSquare) {
                return true;
            }
        }
        return false;
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
