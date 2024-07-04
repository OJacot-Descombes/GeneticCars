using GeneticCars.Cars;
using SkiaSharp.Views.Desktop;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneticCars;

public class Game
{
    public const float Gravity = 9.81f;
    public const int Fps = 30;

    private Car[]? _cars;

    public void Draw(SKPaintGLSurfaceEventArgs e)
    {
        SKCanvas canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);
        canvas.Translate(0, e.Info.Height);
        canvas.Scale(30, -30);
        foreach (var car in _cars) {
            car?.Draw(canvas);
        }
    }

    public async void Run(SKGLControl control)
    {
        var world = new World {
            Gravity = new Vector2(0, -Gravity),
            Enabled = true
        };
        const int N = 20;
        _cars = new Car[N];
        for (int i = 0; i < N; i++) {
            _cars[i] = Car.CreateRandom(0, world, new Vector2(10, 10));
            _cars[i].IsElite = true;
        }

        world.CreateRectangle(100, 5, 1, new Vector2(-10, -2));

        var iterations = new SolverIterations {
            PositionIterations = 4,
            VelocityIterations = 4,
            TOIPositionIterations = 4,
            TOIVelocityIterations = 4
        };
        while (true) {
            await Task.Delay(1);
            control.Refresh();
            world.Step(1f / Fps, ref iterations);
        }
    }
}
