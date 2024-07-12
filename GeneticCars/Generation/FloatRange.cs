namespace GeneticCars.Generation;

public sealed class FloatRange(float min, float range)
{
    public float Min { get; } = min;
    public float Range { get; } = range;

    public override string ToString() => $"[{Min}..{Min + Range}]";
}
