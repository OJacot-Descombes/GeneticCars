namespace GeneticCars.Generation;

public abstract partial class Individual(Class @class, Gene[] genome, int generation, Name name)
{
    public Gene[] Genome { get; } = genome;

    public int Generation { get; } = generation;

    public Name Name { get; } = name;

    public Class Class { get; } = @class;

    int _health;
    public int Health
    {
        get { return _health; }
        set {
            bool wasAlive = IsAlive;
            _health = value;
            if (wasAlive && !IsAlive) {
                Dying();
            }
        }
    }

    public bool IsAlive => Health > 0;

    public abstract float Fitness { get; }

    protected virtual void Dying()
    {
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
