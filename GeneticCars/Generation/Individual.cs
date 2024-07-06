namespace GeneticCars.Generation;

public abstract partial class Individual(Class @class, Gene[] genome, int generation, string name)
{
    public Gene[] Genome { get; } = genome;

    public int Generation { get; } = generation;

    public string Name { get; } = name;

    public Class Class { get; } = @class;

    public int Health { get; set; }

    public bool IsAlive => Health > 0;

    public abstract float Fitness { get; }

    public string CombineNames(Individual other) => Name[..(Name.Length / 2)] + other.Name[..(Name.Length / 2)];

    public static string GenerateName(int len)
    {
        // From Waqar Khan https://stackoverflow.com/a/49922533/880990
        string[] consonants = ["b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "w", "x", "z"];
        string[] vowels = ["a", "e", "i", "o", "u", "y"];
        string Name = "";
        Name += consonants[Random.Shared.Next(consonants.Length)].ToUpper();
        Name += vowels[Random.Shared.Next(vowels.Length)];
        int b = 2; //b tells how many times a new letter has been added. It's 2 right now because the first two letters are already in the name.
        while (b < len) {
            Name += consonants[Random.Shared.Next(consonants.Length)];
            b++;
            Name += vowels[Random.Shared.Next(vowels.Length)];
            b++;
        }
        return Name;
    }

    public override int GetHashCode()
    {
        int hash = 17;
        foreach (Gene gene in Genome) {
            unchecked {
                hash = hash * 31 + gene.Fraction.GetHashCode();
            }
        }
        return hash;
    }
}
