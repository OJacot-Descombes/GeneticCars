namespace GeneticCars.Genealogy;

public abstract class GenerationBase
{
    public bool DeathApplied { get; set; }
    public bool KryptoniteApplied { get; set; }
    public bool NewFloorGenerated { get; set; }

    public bool RadioactivityApplied { get; set; }

    public string? MutationText { get; set; }

    public void SaveParameters(Parameters parameters)
    {
        RadioactivityApplied = parameters.Radioactivity.Value;
        KryptoniteApplied = parameters.Kryptonite.Value;
        DeathApplied = parameters.Death.Value;
        NewFloorGenerated = parameters.RegenerateFloor.Value;

        MutationText = $"R/S {MutText(parameters.MutationRate)} / {MutText(parameters.MutationSize)}";

        string MutText(float value) => parameters.PercentValues.First(p => p.Value == value).Text.TrimStart();
    }
}