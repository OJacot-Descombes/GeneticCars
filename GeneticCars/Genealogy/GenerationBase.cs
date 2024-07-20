namespace GeneticCars.Genealogy;

public abstract class GenerationBase
{
    public bool DeathApplied { get; set; }
    public bool KryptoniteApplied { get; set; }
    public bool NewFloorGenerated { get; set; }

    public bool RadioactivityApplied { get; set; }

    public void SaveParameters(Parameters parameters)
    {
        RadioactivityApplied = parameters.Radioactivity;
        KryptoniteApplied = parameters.Kryptonite;
        DeathApplied = parameters.Death;
        NewFloorGenerated = parameters.RegenerateFloor;
    }
}