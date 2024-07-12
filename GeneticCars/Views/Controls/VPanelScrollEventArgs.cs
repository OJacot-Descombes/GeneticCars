namespace GeneticCars.Views.Controls;

public class VPanelScrollEventArgs(int dx, int dy) : EventArgs
{
    public int dx = dx, dy = dy;
}
