using GeneticCars.Cars;
using GeneticCars.Generation;
using SkiaSharp.Views.Desktop;

namespace GeneticCars;

public class Game
{
    public const float Gravity = 9.81f;
    public const float Zoom = 30;
    public const int Fps = 30;
    public const int MaxCarHealth = Fps * 8;
    public const int CarCount = 40;
    public const bool DisplayFps = true;

    public static readonly LabelPlacer LabelPlacer = new();

    public event EventHandler? FamilyTreeChanged;

    private static readonly SKPaint _fpsPaint = new() {
        Color = SKColors.Red,
        IsStroke = false,
        IsAntialias = true
    };
    private static readonly SKFont _fpsFont = SKTypeface.FromFamilyName("Arial").ToFont(0.5f);

    private readonly Car[] _cars = new Car[CarCount];
    private Floor _floor = new(new Vector2(-4.9f, 2f));
    private readonly Vector2 _spawnPosition = new(-4, 5);

    private readonly Camera _camera = new();
    private readonly Generator<Car> _generator = new();
    private readonly FamilyTree _familyTree = new();

    private bool _running = true;
    private Car? _lastFocusedCar = null;
    private int _nDraw, _lastFps;
    private DateTime _lastDrawTime = DateTime.UtcNow;

    public Size FamilyTreePixelSize => _familyTree.FamilyTreePixelSize;

    public Parameters Parameters { get; } = new();

    public void DrawSimulation(SKPaintGLSurfaceEventArgs e)
    {
        SKCanvas canvas = e.Surface.Canvas;
        if (_nDraw++ % 30 == 0) {
            Array.Sort(_cars, (a, b) => (a.Fitness, a.Identity).CompareTo((b.Fitness, b.Identity)));
        }
        foreach (Car car in _cars) {
            // We have to this without changing the order, because otherwise the dead car's labels will always
            // display at the bottom, which makes them drop when their car dies.
            car.CalculateNextInfoPosition(canvas);
        }
        Car focusedCar = GetFocusedCar();
        if (focusedCar is null) {
            _running = false;
            if (_lastFocusedCar is null) {
                return;
            }
        }
        Vector2 focus = _camera.GetFocus(focusedCar ?? _lastFocusedCar!, _floor);
        _lastFocusedCar = focusedCar;

        canvas.Clear(SKColors.White);
        canvas.Translate(Math.Min(200, -Zoom * focus.X + e.Info.Width - 200), Zoom * focus.Y + e.Info.Height - 80);
        canvas.Scale(Zoom, -Zoom);

        _floor.Draw(canvas);
        // Draw dead cars fist, so that they remain in the background.
        for (int i = _cars.Length - 1; i >= 0; i--) {
            var car = _cars[i];
            if (!car.IsAlive) {
                car.Draw(canvas);
            }
        }
        for (int i = _cars.Length - 1; i >= 0; i--) {
            var car = _cars[i];
            if (car.IsAlive) {
                car.Draw(canvas);
            }
        }
        LabelPlacer.Reset();

        if (DisplayFps) {
            var currentDateTime = DateTime.UtcNow;
            var elapsed = currentDateTime - _lastDrawTime;
            _lastDrawTime = currentDateTime;

            if (_nDraw % 15 == 0) {
                int fps = (int)(1_000_000.0 / (elapsed.TotalMicroseconds));
                _lastFps = fps;
            }

            float y = canvas.LocalClipBounds.Bottom - 1.5f;
            canvas.Scale(1, -1, 0, y);
            canvas.DrawText($"fps: {_lastFps}", canvas.LocalClipBounds.Left + 0.1f, y, _fpsFont, _fpsPaint);
        }
    }

    public void DrawFamilyTree(SKPaintGLSurfaceEventArgs e)
    {
        _familyTree.Draw(e.Surface.Canvas);
    }

    private Car GetFocusedCar()
    {
        return _cars
            .Where(c => c.IsAlive)
            .MaxBy(c => c.Fitness)!;
    }

    public async void Run(SKGLControl control)
    {
        var world = CreateWorld();
        _generator.GenerateInitial(world, _cars, _spawnPosition);
        _floor.AddTo(world);

        var iterations = new SolverIterations {
            PositionIterations = 4,
            VelocityIterations = 4,
            TOIPositionIterations = 4,
            TOIVelocityIterations = 4
        };
        while (true) {
            int frame = 1;
            while (_running) {
                await Task.Delay(1);
                control.Refresh();
                if (Parameters.Playing) {
                    world.Step(1f / Fps, ref iterations);
                    foreach (Car car in _cars) {
                        float velocity = car.Body.LinearVelocity.X;
                        if (frame > 100 && velocity < 0.05f && velocity > -1.0f) {
                            car.Health--;
                        }
                    }
                    frame++;
                }
            }
            _familyTree.AddGeneration(_cars);
            FamilyTreeChanged?.Invoke(this, EventArgs.Empty);
            await Task.Delay(500);
            _camera.Reset();
            world = CreateWorld();
            if (Parameters.ChangingFloor) {
                _floor = new(new Vector2(-4.9f, 2f));
            }
            _floor.AddTo(world);
            _generator.Evolve(world, _cars, _spawnPosition);
            _running = true;
        }
    }

    private static World CreateWorld()
    {
        return new World {
            Gravity = new Vector2(0, -Gravity),
            Enabled = true
        };
    }
}
