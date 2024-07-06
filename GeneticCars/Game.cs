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
    public const int CarCount = 100;

    private readonly Car[] _cars = new Car[CarCount];
    private readonly Floor _floor = new(new Vector2(-4.9f, 2f));
    private readonly Camera _camera = new();
    private readonly Generator<Car> _generator = new();

    private bool _running = true;
    private Car? _lastFocusedCar = null;

    public void Draw(SKPaintGLSurfaceEventArgs e)
    {
        Car focusedCar = GetFocusedCar();
        if (focusedCar is null) {
            _running = false;
            if (_lastFocusedCar is null) {
                return;
            }
        }
        Vector2 focus = _camera.GetFocus(focusedCar ?? _lastFocusedCar!, _floor);
        _lastFocusedCar = focusedCar;

        SKCanvas canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);
        canvas.Translate(Math.Min(200, -Zoom * focus.X + e.Info.Width - 100), Zoom * focus.Y + e.Info.Height - 80);
        canvas.Scale(Zoom, -Zoom);

        _floor.Draw(canvas);
        foreach (Car car in _cars.OrderBy(c => c.IsAlive ? 1 : 0)) {
            car.Draw(canvas);
        }
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
        var spawnPosition = new Vector2(0, 15);
        _generator.GenerateInitial(world, _cars, spawnPosition);
        _floor.AddTo(world);

        var iterations = new SolverIterations {
            PositionIterations = 4,
            VelocityIterations = 4,
            TOIPositionIterations = 4,
            TOIVelocityIterations = 4
        };
        while (true) {
            while (_running) {
                await Task.Delay(1);
                control.Refresh();
                world.Step(1f / Fps, ref iterations);
                foreach (Car car in _cars) {
                    float velocity = car.Body.LinearVelocity.X;
                    if (velocity < 0.05f && velocity > -0.5f) {
                        car.Health--;
                    }
                }
            }
            await Task.Delay(500);
            world = CreateWorld();
            _floor.AddTo(world);
            _generator.Evolve(world, _cars, spawnPosition);
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
