namespace GeneticCars.Generation;

public readonly record struct Identity(Name Name, int Generation, int Number) : IComparable, IComparable<Identity>
{
    public string InfoText { get; } = Generation == 0 && Number == 0
        ? $"{Name.Display} 0"
        : $"{Name.Display} {Generation}.{Number}";

    public override string ToString() => InfoText;

    int IComparable.CompareTo(object? obj)
    {
        if (obj is Identity id) {
            return CompareTo(id);
        }
        throw new ArgumentException(nameof(obj) + " is not an " + nameof(Identity));
    }

    public int CompareTo(Identity other)
    {
        int result = Name.CompareTo(other.Name);
        if (result != 0)
            return result;
        result = Generation.CompareTo(other.Generation);
        if (result != 0)
            return result;
        return Number.CompareTo(other.Number);
    }
}
