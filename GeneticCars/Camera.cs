using SkiaSharp.Views.Desktop;

namespace GeneticCars;

public class Camera
{
    private Vector2 _actualFocus;
    private Vector2 _integralTerm;

    public Vector2 GetViewBoxFocus(Cars.Car focusedCar, Floor floor, Parameters parameters, SKPaintGLSurfaceEventArgs e)
    {
        const float fp = 0.05f; // Proportional factor.
        const float fi = 0.002f; // Integral factor.
        const float maxDeltaV = 1f;

        float x = focusedCar.Body.Position.X;
        var desiredFocus = new Vector2(x, floor.AltitudeAt(x));
        Vector2 deltaFocus = desiredFocus - _actualFocus;

        var deltaV = new Vector2(
            Single.Clamp(fp * deltaFocus.X, -maxDeltaV, maxDeltaV),
            Single.Clamp(fp * deltaFocus.Y, -maxDeltaV, maxDeltaV));

        bool isIntegralDecreasingX = _integralTerm.X * deltaFocus.X < 0;
        bool isIntegralDecreasingY = _integralTerm.Y * deltaFocus.Y < 0;
        _integralTerm += fi * deltaFocus;

        // Avoid overshooting
        if (isIntegralDecreasingX) {
            _integralTerm.X = 0;
        }
        if (isIntegralDecreasingY) {
            _integralTerm.Y = 0;
        }

        _actualFocus += deltaV + _integralTerm;

        // Calculate view box translation
        float rightBorder = e.Info.Width / 7 + 50;
        const float MinLeftBorder = 200;
        float dx = Math.Min(MinLeftBorder, -parameters.Zoom * _actualFocus.X + e.Info.Width - rightBorder);

        const float MinTop = 100;
        float dy = parameters.Zoom * _actualFocus.Y + e.Info.Height - (e.Info.Height - MinTop) / 3;

        return new Vector2(dx, dy);
    }

    public void Reset()
    {
        _actualFocus = Vector2.Zero;
        _integralTerm = Vector2.Zero;
    }
}
