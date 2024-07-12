using SkiaSharp.Views.Desktop;

namespace GeneticCars;

public partial class MainForm : Form
{
    private readonly Game _game = new();
    //private readonly SpawnTestGame _game = new();
    private int _left;

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
        UpdateVisibleBounds();
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
        _game.DrawFamilyTree(e, _left, _left + flowLayoutPanel1.Width);
    }

    private void FlowLayoutPanel_Resize(object sender, EventArgs e)
    {
        AdjustFamilyTreeControlSize();
        UpdateVisibleBounds();
    }

    private void AdjustFamilyTreeControlSize()
    {
        Size familyTreeSize = _game.FamilyTreePixelSize;
        familyTreeSKGLControl.Size = new Size(familyTreeSize.Width, familyTreeSize.Height);
    }

    private void UpdateVisibleBounds()
    {
        int newLeft = -familyTreeSKGLControl.Left;
        if (newLeft != _left) {
            _left = newLeft;
            familyTreeSKGLControl.Invalidate();
        }
    }

    private void FlowLayoutPanel1_Scroll(object sender, ScrollEventArgs e)
    {
        UpdateVisibleBounds();
    }
}
