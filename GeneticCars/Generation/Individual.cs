using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticCars.Generation;

public abstract class Individual(Gene[] genes, int generation, string name)
{
    public Gene[] Genes { get; } = genes;

    public int Generation { get; } = generation;

    public string Name { get; } = name;
}
