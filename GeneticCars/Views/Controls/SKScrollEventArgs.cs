namespace GeneticCars.Views.Controls;

public class SKScrollEventArgs(int dx, int dy) : EventArgs
{
    public int dx = dx, dy = dy;
}
