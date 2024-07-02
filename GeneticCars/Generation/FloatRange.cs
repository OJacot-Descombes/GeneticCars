using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticCars.Generation;

public sealed class FloatRange(float min, float range)
{
    public float Min { get; } = min;
    public float Range { get; } = range;

    public float RandomValue() => Min + Random.Shared.NextSingle() * Range;
}
