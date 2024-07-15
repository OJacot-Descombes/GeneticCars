using GeneticCars.Cars;
using GeneticCars.Generation;
using SkiaSharp.Views.Desktop;

namespace GeneticCars;

public class Game
{
    public const float Gravity = 9.81f;
    public const float Zoom = 30;
    public const int AssumedFps = 30;
    public const int MaxCarHealth = AssumedFps * 8;
    public const int CarCount = 40;

    public static readonly LabelPlacer LabelPlacer = new();

    public event EventHandler? FamilyTreeChanged;

    private readonly Car[] _cars = new Car[CarCount];
    private Floor _floor = new(new Vector2(-4.9f, 2f));
    private readonly Vector2 _spawnPosition = new(-4, 5);

    private readonly Camera _camera = new();
    private readonly Generator<Car> _generator = new();
    private readonly FamilyTree _familyTree = new();
    private readonly FpsMeter _fpsMeter = new();

    private bool _running = true;
    private Car? _lastFocusedCar = null;
    private int _nDraw;

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
            car.CalculateNextInfoPosition();
        }
        Car focusedCar = GetFocusedCar();
        if (focusedCar is null) {
            _running = false;
            if (_lastFocusedCar is null) {
                return;
            }
        }
        Vector2 focus = _camera.GetViewBoxFocus(focusedCar ?? _lastFocusedCar!, _floor, e);
        _lastFocusedCar = focusedCar;

        canvas.Clear(SKColors.White);
        canvas.Translate(focus.X, focus.Y);
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

        if (Parameters.DisplayFps) {
            _fpsMeter.Update(canvas);
        }
    }

    public void DrawFamilyTree(SKPaintGLSurfaceEventArgs e, SKRect viewBox)
    {
        _familyTree.Draw(e.Surface.Canvas, viewBox);
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
        _familyTree.AddUnscoredGeneration(_cars);
        FamilyTreeChanged?.Invoke(this, EventArgs.Empty);
        _floor.AddTo(world);
        Parameters.MutationBoostEnabled = false;
        Parameters.MutationBoost = false;

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
                    world.Step(1f / AssumedFps, ref iterations);
                    foreach (Car car in _cars) {
                        float velocity = car.Body.LinearVelocity.X;
                        if (frame > 100 && velocity < 0.18f && velocity > -1.0f) {
                            car.Health--;
                        }
                    }
                    frame++;
                }
            }
            _familyTree.UpdateScoredGeneration(_cars);
            await Task.Delay(500);
            _camera.Reset();
            world = CreateWorld();
            if (Parameters.ChangingFloor) {
                _floor = new(new Vector2(-4.9f, 2f));
            }
            _floor.AddTo(world);
            _generator.Evolve(world, _cars, _spawnPosition, Parameters.MutationBoost);
            _familyTree.AddUnscoredGeneration(_cars);
            FamilyTree.Node[] lastScoredGeneration = _familyTree.Generations[^2];
            Parameters.MutationBoostEnabled = _familyTree.Generations.Count > 5 &&
                Generator<Car>.CountBoostable(
                    lastScoredGeneration
                    .Take(lastScoredGeneration.Length / 4)
                    .Select(n => n.Fitness ?? 0f)) > 2;
            if (!Parameters.MutationBoostEnabled) {
                Parameters.MutationBoost = false;
            }
            FamilyTreeChanged?.Invoke(this, EventArgs.Empty);
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
