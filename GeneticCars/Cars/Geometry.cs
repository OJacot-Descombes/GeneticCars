using static System.MathF;


namespace GeneticCars.Cars;

public static class Geometry
{
    public static (LineSegment T1, LineSegment T2) ExternalTangentLines(Vector2 c1, float r1, Vector2 c2, float r2)
    {
        if (r1 > r2) {
            (r1, r2) = (r2, r1);
            (c1, c2) = (c2, c1);
        }

        Vector2 cDiff = c2 - c1;
        if (cDiff.Length() + r1 < r2 + 0.01f) {
            return ((c1, c2), (c1, c2));
        }
        float gamma = -Atan2(cDiff.Y, cDiff.X);
        float beta = Asin(r2 - r1 / Sqrt(cDiff.X * cDiff.X + cDiff.Y * cDiff.Y));
        float alpha = gamma - beta;
        float sinAlpha = Sin(alpha);
        float cosAlpha = Cos(alpha);
        var p1 = new Vector2(c1.X + r1 * sinAlpha, c1.Y + r1 * cosAlpha);
        var p2 = new Vector2(c2.X + r2 * sinAlpha, c2.Y + r2 * cosAlpha);

        beta = -beta;
        alpha = gamma - beta;
        sinAlpha = Sin(alpha);
        cosAlpha = Cos(alpha);
        var p3 = new Vector2(c1.X - r1 * sinAlpha, c1.Y - r1 * cosAlpha);
        var p4 = new Vector2(c2.X - r2 * sinAlpha, c2.Y - r2 * cosAlpha);

        return ((p1, p2), (p3, p4));
    }
}