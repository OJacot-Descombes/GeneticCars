﻿using GeneticCars.Cars;
using SkiaSharp.Views.Desktop;

namespace GeneticCars;

public class SpawnTestGame
{
    public const float Gravity = 9.81f;
    public const float Zoom = 50;
    public const int Fps = 120;
    public const int MaxCarHealth = Fps * 8;
    public const int CarCount = 1;

    private readonly Vector2 _spawnPosition = new(-4, 5);
    private Car _car = null!;
    private readonly Floor _floor = new(new Vector2(-4.9f, 2f));

#pragma warning disable CS0067 // The event is never used
    public event EventHandler? FamilyTreeChanged;
#pragma warning restore CS0067

    public Parameters Parameters { get; } = new();

    public Size FamilyTreePixelSize { get; } = new();

    public void DrawSimulation(SKPaintGLSurfaceEventArgs e)
    {
        SKCanvas canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.White);
        var focus = new Vector2(0, 2.6f);
        canvas.Translate(Math.Min(200, -Zoom * focus.X + e.Info.Width - 200), Zoom * focus.Y + e.Info.Height - 80);
        canvas.Scale(Zoom, -Zoom);

        _floor.Draw(canvas, Parameters);
        _car.Draw(canvas, new Parameters());
    }

#pragma warning disable CA1822, IDE0060 // Mark members as static, remove unused parameter
    public void DrawFamilyTree(SKPaintGLSurfaceEventArgs e, SKRect viewBox)
    {
    }
#pragma warning restore CA1822, IDE0060 // Mark members as static

    public async void Run(SKGLControl control)
    {
        var iterations = new SolverIterations {
            PositionIterations = 4,
            VelocityIterations = 4,
            TOIPositionIterations = 4,
            TOIVelocityIterations = 4
        };
        while (true) {
            var world = CreateWorld();
            _floor.AddTo(world);
            _car = Car.CreateRandom(world, _spawnPosition);

            for (int frame = 0; frame < 150; frame++) {
                await Task.Delay(1);
                control.Refresh();
                world.Step(1f / Fps, ref iterations);
                if (frame < 15) {
                    _car.Body.AngularVelocity = 0;
                }
            }
            await Task.Delay(500);
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
