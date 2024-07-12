namespace GeneticCars.Views.Controls;

internal class NoFocusButton : Button {
    public NoFocusButton()
        : base()
    {
        this.SetStyle(ControlStyles.Selectable, false);
    }
}
