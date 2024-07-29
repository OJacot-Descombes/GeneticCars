using GeneticCars.Cars;
using GeneticCars.Evolution;
using GeneticCars.Genealogy;
using SkiaSharp.Views.Desktop;

namespace GeneticCars;

public class Game
{
    public const float Gravity = 9.81f;
    public const int MaxCarHealth = 30/*base fps*/ * 8 * 4/*base physics iterations*/;
    private const int GracePeriodInIterations = 400;
    public static readonly LabelPlacer LabelPlacer = new();

    public event EventHandler? FamilyTreeChanged;

    private readonly Generation<Car> _carGeneration;
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
        _carGeneration = new Generation<Car>(new Car[Parameters.PopulationSize]);
    }

    public Size FamilyTreePixelSize => _familyTree.FamilyTreePixelSize;

    public Parameters Parameters { get; } = new();

    public void DrawSimulation(SKPaintGLSurfaceEventArgs e)
    {
        SKCanvas canvas = e.Surface.Canvas;
        if (_nDraw++ % 30 == 0) {
            Array.Sort(_carGeneration.Population, (a, b) => (a.Fitness, a.Identity).CompareTo((b.Fitness, b.Identity)));
        }
        foreach (Car car in _carGeneration.Population) {
            // We have to this without changing the order, because otherwise the dead car's labels will always
            // display at the bottom, which makes them drop when their car dies.
            car.CalculateNextInfoPosition(Parameters);
        }
        Car focusedCar = GetFocusedCar();
        if (focusedCar is null) {
            _running = false;
            if (_lastFocusedCar is null) {
                return;
            }
        }
        Vector2 focus = _camera.GetViewBoxFocus(focusedCar ?? _lastFocusedCar!, _floor, Parameters, e);
        _lastFocusedCar = focusedCar;

        canvas.Clear(SKColors.White);
        canvas.Translate(focus.X, focus.Y);
        canvas.Scale(Parameters.Zoom, -Parameters.Zoom);

        _floor.Draw(canvas, Parameters);
        // Draw dead cars fist, so that they remain in the background.
        for (int i = _carGeneration.Population.Length - 1; i >= 0; i--) {
            var car = _carGeneration.Population[i];
            if (!car.IsAlive) {
                car.Draw(canvas, Parameters);
            }
        }
        for (int i = _carGeneration.Population.Length - 1; i >= 0; i--) {
            var car = _carGeneration.Population[i];
            if (car.IsAlive) {
                car.Draw(canvas, Parameters);
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
        return _carGeneration.Population
            .Where(c => c.IsAlive)
            .MaxBy(c => c.Fitness)!;
    }

    public async void Run(SKGLControl skGLControl)
    {
        var world = CreateWorld();
        _generator.GenerateInitial(world, _carGeneration.Population, _spawnPosition);
        var solverIterations = new SolverIterations {
            PositionIterations = 4,
            VelocityIterations = 4,
            TOIPositionIterations = 4,
            TOIVelocityIterations = 4
        };

        while (true) {
            _floor.AddTo(world);
            _familyTree.AddUnscoredGeneration(_carGeneration);
            FamilyTreeChanged?.Invoke(this, EventArgs.Empty);
            int iterationCount = 0;
            _running = true;
            while (_running) {
                await Task.Delay(1); // Process the message loop.
                skGLControl.Refresh();
                if (Parameters.Playing) {
                    for (int i = 0; i < Parameters.Iterations; i++) {
                        world.Step(1f / 120f, ref solverIterations);
                    }
                    foreach (Car car in _carGeneration.Population) {
                        float x = car.Body.Position.X;
                        float speed = (x - car.LastPositionX) * 30f / Parameters.Iterations;
                        if (iterationCount > GracePeriodInIterations && speed < 0.2f && speed > -1.1f) {
                            car.Health -= Parameters.Iterations;
                        }
                        // Because of bug in physics engine LinearVelocity.X can be >0 when position does not or only
                        // barely change. In rare cases, apparently non-moving car's health did not decrease.
                        car.LastPositionX = x;
                    }
                    iterationCount += Parameters.Iterations;
                }
            }
            _familyTree.UpdateScoredGeneration(_carGeneration);
            await Task.Delay(500);
            _camera.Reset();
            world = CreateWorld();
            if (Parameters.RegenerateFloor.Value) {
                _floor = new(new Vector2(-4.9f, 2f));
            }
            if (Parameters.Death.Value) {
                _lastDeathGeneration = _familyTree.Generations.Count;
            }
            _generator.Evolve(world, _carGeneration, _spawnPosition, Parameters);
            FamilyTree.Node[] lastScoredGeneration = _familyTree.Generations[^1].Population;

            EnableDisableRadioactivity(lastScoredGeneration);
            EnableDisableKryptonite();
            Parameters.Death.Enabled = _familyTree.Generations.Count >= 10 &&
                _familyTree.Generations.Count > _lastDeathGeneration + 2;
        }
    }

    private void EnableDisableRadioactivity(FamilyTree.Node[] lastScoredGeneration)
    {
        Parameters.Radioactivity.Enabled = !Parameters.Death.Value && _familyTree.Generations.Count >= 5 &&
            Generator<Car>.CountIrradiatable(
                lastScoredGeneration
                .Take(lastScoredGeneration.Length / 4)
                .Select(n => n.Fitness ?? 0f)) > Parameters.PopulationSize / 20;
        if (!Parameters.Radioactivity.Enabled) {
            Parameters.Radioactivity.Value = false;
        }
    }

    private void EnableDisableKryptonite()
    {
        Parameters.Kryptonite.Enabled = !Parameters.Death.Value && _familyTree.Generations.Count >= 8 &&
            Generator<Car>.KillableByKryptoniteCountIsAtLeast(
                _carGeneration.Population, Math.Max(1, Parameters.PopulationSize / 10));
        if (!Parameters.Kryptonite.Enabled) {
            Parameters.Kryptonite.Value = false;
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
