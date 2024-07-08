using SkiaSharp.Views.Desktop;

namespace GeneticCars;

public partial class MainForm : Form
{
    private readonly Game _game = new();
    //private readonly SpawnTestGame _game = new();

    public MainForm()
    {
        InitializeComponent();
    }

    private void SkGLControl_PaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
    {
        _game.Draw(e);
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        _game.Run(skGLControl);
    }
}
