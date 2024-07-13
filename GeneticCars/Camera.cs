namespace GeneticCars;

public class Camera
{
    private Vector2 _actualFocus;
    private Vector2 _integralTerm;

    public Vector2 GetFocus(Cars.Car focusedCar, Floor floor)
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
        return _actualFocus;
    }

    public void Reset()
    {
        _actualFocus = Vector2.Zero;
        _integralTerm = Vector2.Zero;
    }
}
