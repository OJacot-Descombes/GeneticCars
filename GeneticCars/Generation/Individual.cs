namespace GeneticCars.Generation;

public abstract partial class Individual(Class @class, Gene[] genome, Identity identity,
    Individual? ancestor1, Individual? ancestor2)
{
    public Class Class { get; } = @class;

    public Gene[] Genome { get; } = genome;

    public Individual? Ancestor1 { get; } = ancestor1;
    public Individual? Ancestor2 { get; } = ancestor2;

    public Identity Identity { get; } = identity;

    private int _health;
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

    public override string ToString() => $"{Identity} {Class} ({Ancestor1}, {Ancestor2})";
}
