using GeneticCars.Evolution;
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
        legendNewLabel.ForeColor = Individual.NewBaseColor.ToDrawingColor();
        legendEliteLabel.ForeColor = Individual.EliteBaseColor.ToDrawingColor();
        legendCrossLabel.ForeColor = Individual.CrossedBaseColor.ToDrawingColor();
        legendMutationLabel.ForeColor = Individual.MutatedBaseColor.ToDrawingColor();

        _game.FamilyTreeChanged += Game_FamilyTreeChanged;
        parametersBindingSource.DataSource = _game.Parameters;
        MouseWheel += MainForm_MouseWheel;
        familyTreeVPanel.skGLControl.MouseMove += SkGLControl_MouseMove;
        familyTreeVPanel.skGLControl.MouseClick += SkGLControl_MouseClick;
        familyTreeVPanel.skGLControl.MouseLeave += SkGLControl_MouseLeave;
    }

    private void SkGLControl_MouseLeave(object? sender, EventArgs e)
    {
        if (_game.FamilyTree.DeselectNode()) {
            familyTreeVPanel.skGLControl.Invalidate();
        }
    }

    private void SkGLControl_MouseClick(object? sender, MouseEventArgs e)
    {
        if (_game.FamilyTree.SelectNode(
            e.X + familyTreeVPanel.ScrollOffset.X, e.Y + familyTreeVPanel.ScrollOffset.Y, sticky: true)) {
            familyTreeVPanel.skGLControl.Invalidate();
        }
    }

    private void SkGLControl_MouseMove(object? sender, MouseEventArgs e)
    {
        if (_game.FamilyTree.SelectNode(
            e.X + familyTreeVPanel.ScrollOffset.X, e.Y + familyTreeVPanel.ScrollOffset.Y, sticky: false)) {
            familyTreeVPanel.skGLControl.Invalidate();
        }
    }

    private void MainForm_MouseWheel(object? sender, MouseEventArgs e)
    {
        if (ModifierKeys == Keys.Control) {
            Point screenPos = PointToScreen(e.Location);
            Rectangle screenRect = familyTreeVPanel.RectangleToScreen(familyTreeVPanel.Bounds);
            if (screenRect.Contains(screenPos) && _game.FamilyTree.ZoomBy(e.Delta)) {
                familyTreeVPanel.VirtualAreaSize = _game.FamilyTreePixelSize;
                familyTreeVPanel.skGLControl.Invalidate();
            }
        }
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
