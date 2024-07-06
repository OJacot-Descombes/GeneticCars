﻿using System.Xml.Linq;

namespace GeneticCars;

public class Floor
{
    private const int MaxFloorTiles = 200;
    private const float SegmentLength = 1.5f;
    private const float FloorThickness = 0.25f;

    private static readonly SKPaint FloorFillPaint = new() {
        Color = SKColors.OliveDrab,
        IsStroke = false,
        IsAntialias = true
    };
    private static readonly SKPaint GridStrokePaint = new() {
        Color = SKColors.DarkGray,
        IsStroke = true,
        IsAntialias = false,
    };
    private static readonly SKPaint GridTextPaint = new() {
        Color = SKColors.DarkGray,
        IsStroke = false,
        IsAntialias = true
    };

    private static readonly SKFont _gridFont = SKTypeface.FromFamilyName("Arial").ToFont(1f);

    public readonly Vector2[] Vertices = new Vector2[MaxFloorTiles + 1]; // Plus left boundary

    public Floor(Vector2 startPosition)
    {
        Vertices[0] = startPosition + new Vector2(-2, 2);
        Vertices[1] = startPosition;
        for (int i = 2; i < Vertices.Length; i++) {
            float dy = (Random.Shared.NextSingle() * 3f - 1.5f) * 2.5f * i / MaxFloorTiles;
            startPosition += new Vector2(SegmentLength, dy);
            Vertices[i] = startPosition;
        }
    }

    public void AddTo(World world)
    {
        for (int i = 1; i < MaxFloorTiles; i++) {
            world.CreateEdge(Vertices[i - 1], Vertices[i]);
        }
    }

    public void Draw(SKCanvas canvas)
    {
        DrawGrid(canvas);

        var path = new SKPath();
        for (int i = 1; i < MaxFloorTiles; i++) {
            var v0 = Vertices[i - 1];
            var v1 = Vertices[i];
            path.MoveTo(v0.X, v0.Y);
            path.LineTo(v1.X, v1.Y);
            path.LineTo(v1.X, v1.Y - FloorThickness);
            path.LineTo(v0.X, v0.Y - FloorThickness);
            path.Close();
            canvas.DrawPath(path, FloorFillPaint);
            path.Reset();
        }
    }

    private void DrawGrid(SKCanvas canvas)
    {
        const int Spacing = 10;

        int n = (int)Single.Ceiling(Vertices[^1].X / Spacing);
        for (int i = 0; i <= n; i++) {
            int x = i * Spacing;
            var bounds = canvas.LocalClipBounds;
            canvas.DrawLine(x, bounds.Top, x, bounds.Bottom, GridStrokePaint);

            // Bounds Y seems to be inverted, so Botton is actually top.
            float y = bounds.Bottom - 1;
            var matrix = canvas.TotalMatrix;
            canvas.Scale(1, -1, 0, y);
            canvas.DrawText(x.ToString(), x + 0.1f, y, _gridFont, GridTextPaint);
            canvas.SetMatrix(matrix);
        }
    }
}
