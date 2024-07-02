using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticCars.Generation;

public readonly struct Gene(FloatRange range, float value)
{
    public FloatRange Range { get; } = range;
    public float Value { get; } = value;
}
