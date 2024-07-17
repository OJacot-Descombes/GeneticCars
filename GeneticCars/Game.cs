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

    public static readonly LabelPlacer LabelPlacer = new();

    public event EventHandler? FamilyTreeChanged;

    private readonly Car[] _cars;
    private Floor _floor = new(new Vector2(-4.9f, 2f));
    private readonly Vector2 _spawnPosition = new(-4, 5);

    private readonly Camera _camera = new();
    private readonly Generator<Car> _generator = new();
    private readonly FamilyTree _familyTree = new();
    private readonly FpsMeter _fpsMeter = new();

    private bool _running;
    private Car? _lastFocusedCar = null;
    private int _nDraw, _lastDeathGeneration;

    public Game()
    {
        _cars = new Car[Parameters.PopulationSize];
    }

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

    public async void Run(SKGLControl skGLControl)
    {
        Parameters.MutationBoostEnabled = false;
        Parameters.MutationBoost = false;
        var world = CreateWorld();
        _generator.GenerateInitial(world, _cars, _spawnPosition);

        var solverIterations = new SolverIterations {
            PositionIterations = 4,
            VelocityIterations = 4,
            TOIPositionIterations = 4,
            TOIVelocityIterations = 4
        };
        while (true) {
            _floor.AddTo(world);
            _familyTree.AddUnscoredGeneration(_cars);
            FamilyTreeChanged?.Invoke(this, EventArgs.Empty);
            int frame = 1;
            _running = true;
            while (_running) {
                await Task.Delay(1); // Process the message loop.
                skGLControl.Refresh();
                if (Parameters.Playing) {
                    world.Step(1f / AssumedFps, ref solverIterations);
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
            if (Parameters.Death) {
                _lastDeathGeneration = _familyTree.Generations.Count;
            }
            _generator.Evolve(world, _cars, _spawnPosition, Parameters);
            FamilyTree.Node[] lastScoredGeneration = _familyTree.Generations[^1];

            EnableDisableMutationBoost(lastScoredGeneration);
            EnableDisableKryptonite();
            Parameters.DeathEnabled = _familyTree.Generations.Count > 10 && _familyTree.Generations.Count > _lastDeathGeneration + 2;
        }
    }

    private void EnableDisableMutationBoost(FamilyTree.Node[] lastScoredGeneration)
    {
        Parameters.MutationBoostEnabled = !Parameters.Death && _familyTree.Generations.Count > 5 &&
            Generator<Car>.CountBoostable(
                lastScoredGeneration
                .Take(lastScoredGeneration.Length / 4)
                .Select(n => n.Fitness ?? 0f)) > Parameters.PopulationSize / 20;
        if (!Parameters.MutationBoostEnabled) {
            Parameters.MutationBoost = false;
        }
    }

    private void EnableDisableKryptonite()
    {
        Parameters.KryptoniteEnabled = !Parameters.Death && _familyTree.Generations.Count > 8 &&
            Generator<Car>.KillableByKryptoniteCountIsAtLeast(_cars, Math.Max(1, Parameters.PopulationSize / 10));
        if (!Parameters.KryptoniteEnabled) {
            Parameters.Kryptonite = false;
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
