namespace GeneticCars.Generation;

public readonly struct Gene(FloatRange range, float fraction)
{
    public Gene(FloatRange range) : this(range, Random.Shared.NextSingle())
    {
    }

    public FloatRange Range { get; } = range;
    public float Value => Range.Min + Fraction * Range.Range;

    // Truncated Value as int
    public int IntValue => (int)Value;

    /// <summary>
    /// Float in range [0, 1] where 0 corresponds to Value = Min and 1 to Value = Min + Range.
    /// </summary>
    public float Fraction { get; } = fraction;
}
