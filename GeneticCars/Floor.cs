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

    public readonly Vector2[] Vertices = new Vector2[MaxFloorTiles];

    public Floor(Vector2 startPosition)
    {
        Vertices[0] = startPosition;
        for (int i = 1; i < MaxFloorTiles; i++) {
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
}
