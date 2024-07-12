using SkiaSharp.Views.Desktop;

namespace GeneticCars;

public partial class MainForm : Form
{
    private readonly Game _game = new();
    //private readonly SpawnTestGame _game = new();

    public MainForm()
    {
        InitializeComponent();
        _game.FamilyTreeChanged += Game_FamilyTreeChanged;
        parametersBindingSource.DataSource = _game.Parameters;
    }

    private void Game_FamilyTreeChanged(object? sender, EventArgs e)
    {
        AdjustFamilyTreeControlSize();
        flowLayoutPanel1.AutoScrollPosition = new Point(familyTreeSKGLControl.Size.Width, -flowLayoutPanel1.AutoScrollPosition.Y);
        familyTreeSKGLControl.Invalidate();
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        _game.Run(simulationSKGLControl);
    }

    private void SimulationSKGLControl_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        _game.DrawSimulation(e);
    }

    private void FamilyTreeSKGLControl_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        _game.DrawFamilyTree(e);
    }

    private void FlowLayoutPanel_Resize(object sender, EventArgs e)
    {
        AdjustFamilyTreeControlSize();
    }

    private void AdjustFamilyTreeControlSize()
    {
        Size familyTreeSize = _game.FamilyTreePixelSize;
        familyTreeSKGLControl.Size = new Size(familyTreeSize.Width, familyTreeSize.Height);
    }
}
