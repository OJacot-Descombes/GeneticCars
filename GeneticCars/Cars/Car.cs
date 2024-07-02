using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneticCars.Generation;

namespace GeneticCars.Cars;

public class Car(Gene[] genes, int generation, string name) : Individual(genes, generation, name)
{
    public const int WheelCount = 2;
    public const int ChassisCount = 8;

    private static readonly FloatRange WheelRadiusRange = new(0.2f, 0.5f);
    private static readonly FloatRange WheelDensityRange = new(40f, 100f);
    private static readonly FloatRange ChassisDensityRange = new(30f, 300f);
    private static readonly FloatRange ChassisAxisRange = new(0.1f, 1.1f);

    public float WheelRadius(int index) => Genes[index].Value;
    public float WheelDensity(int index) => Genes[2 + index].Value;
    public float ChassisDensity => Genes[4].Value;
    public float ChassisAxis(int index) => Genes[5 + index].Value;

    public static Car CreateRandom(int generation, string name)
    {
        Gene[] genes = [
            new(WheelRadiusRange, WheelRadiusRange.RandomValue()),
            new(WheelRadiusRange, WheelRadiusRange.RandomValue()),
            new(WheelDensityRange, WheelDensityRange.RandomValue()),
            new(WheelDensityRange, WheelDensityRange.RandomValue()),
            new(ChassisDensityRange, ChassisDensityRange.RandomValue()),
            new(ChassisAxisRange, ChassisAxisRange.RandomValue()),
            new(ChassisAxisRange, ChassisAxisRange.RandomValue()),
            new(ChassisAxisRange, ChassisAxisRange.RandomValue()),
            new(ChassisAxisRange, ChassisAxisRange.RandomValue()),
            new(ChassisAxisRange, ChassisAxisRange.RandomValue()),
            new(ChassisAxisRange, ChassisAxisRange.RandomValue()),
            new(ChassisAxisRange, ChassisAxisRange.RandomValue()),
            new(ChassisAxisRange, ChassisAxisRange.RandomValue()),
        ];
        return new Car(genes, generation, name);
    }
}
