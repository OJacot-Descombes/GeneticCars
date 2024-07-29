using SkiaSharp.Views.Desktop;

namespace GeneticCars;

public partial class MainForm : Form
{
#if false    
    private readonly SpawnTestGame _game = new();
#else
    private readonly Game _game = new();
#endif

    public MainForm()
    {
        InitializeComponent();
        _game.FamilyTreeChanged += Game_FamilyTreeChanged;
        parametersBindingSource.DataSource = _game.Parameters;
    }

    private void Game_FamilyTreeChanged(object? sender, EventArgs e)
    {
        familyTreeVPanel.VirtualAreaSize = _game.FamilyTreePixelSize;
        familyTreeVPanel.ScrollOffset = new Point(_game.FamilyTreePixelSize.Width, familyTreeVPanel.ScrollOffset.Y);
        familyTreeVPanel.skGLControl.Invalidate();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        _game.Run(simulationSKGLControl);
    }

    private void SimulationSKGLControl_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        _game.DrawSimulation(e);
    }

    private void FamilyTreeVPanel_PaintSurface(SKPaintGLSurfaceEventArgs e, SKRect viewBox)
    {
        _game.DrawFamilyTree(e, viewBox);
    }
}
