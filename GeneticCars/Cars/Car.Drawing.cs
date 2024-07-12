namespace GeneticCars.Cars;

public partial class Car
{
    private static readonly SKFont _carFont = SKTypeface.FromFamilyName("Arial").ToFont(0.5f);

    private SKRect _rect;
    private float _lastLabelX;
    private float _nextInfoX;
    private float _integralX;
    private float _lastLabelY;
    private float _nextInfoY;

    public void Draw(SKCanvas canvas)
    {
        var chassisFillPaint = CreateColoredFillPaint(ChassisDensity.Fraction);
        var chassisStrokePaint = ColoredStrokePaint;

        DrawInfo(canvas);
        DrawWheels(canvas, chassisFillPaint, chassisStrokePaint);
        DrawChassis(canvas, chassisFillPaint, chassisStrokePaint);
        DrawWheelHubs(canvas, chassisStrokePaint);
    }

    public void CalculateNextInfoPosition()
    {
        // Make smooth horizontal label movements
        float deltaX = 0.1f * (_chassis.Position.X - _lastLabelX);

        bool isIntegralDecreasing = _integralX * deltaX < 0;
        _integralX += 0.02f * deltaX;

        // Avoid overshooting
        if (isIntegralDecreasing) {
            _integralX = 0;
        }

        _nextInfoX = _lastLabelX + deltaX + _integralX;
        _lastLabelX = _nextInfoX;

        _nextInfoY = _chassis.Position.Y;
        _rect = GetInfoRect(Identity.InfoText);
        float displacement = Game.LabelPlacer.GetVerticalDisplacement(_rect);
        _nextInfoY += displacement;

        // Make smooth vertical label movements
        _nextInfoY = _lastLabelY + 0.1f * (_nextInfoY - _lastLabelY);
        _lastLabelY = _nextInfoY;
    }

    private void DrawInfo(SKCanvas canvas)
    {
        float max = canvas.LocalClipBounds.Bottom - 4.5f;
        if (_nextInfoY > max) {
            _lastLabelY = max;
        } else {
            var matrix = canvas.TotalMatrix;

            canvas.Scale(1, -1, 0, _nextInfoY);
            float xText = _nextInfoX - 1;
            float yText = _nextInfoY - 3;
            canvas.DrawText(Identity.InfoText, xText, yText, _carFont, ColoredInfoTextPaint);
            canvas.SetMatrix(matrix);

            yText = _nextInfoY + 3.4f;
            if (IsAlive && Health < Game.MaxCarHealth) {
                canvas.DrawRect(xText, yText - 0.7f, 2f * Health / Game.MaxCarHealth,
                    0.15f, ColoredFillPaint);
                canvas.DrawRect(xText, yText - 0.7f, 2, 0.15f, _infoStrokePaint);
            }
        }
    }

    public SKRect GetInfoRect(string text)
    {
        float xText = _nextInfoX - 1;
        float yText = _chassis.Position.Y + 2.7f;
        float width = _carFont.MeasureText(text);

        float left = xText - 0.1f;
        float top, right, bottom;
        if (IsAlive && Health < Game.MaxCarHealth) {
            top = yText - 0.05f;
            right = left + Math.Max(2f, width) + 0.2f;
            bottom = top + 0.75f;
        } else {
            top = yText + 0.15f;
            right = left + width + 0.2f;
            bottom = top + 0.55f;
        }
        return new SKRect(left, top, right, bottom);
    }

    private void DrawWheels(SKCanvas canvas, SKPaint chassisFillPaint, SKPaint chassisStrokePaint)
    {
        for (int i = 0; i < WheelCount; i++) {
            Body body = _wheels[i];
            var wheelCenter = body.Position;
            using var wheelFillPaint = CreateNeutralFillPaint(WheelDensity(i).Fraction);
            float radius = WheelRadius(i).Value;
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, radius, wheelFillPaint);
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, radius, NeutralStrokePaint);

            // Draw marker on wheel to make rotation observable.
            float angle = body.Rotation;
            float x = wheelCenter.X + MathF.Cos(angle) * 0.6f * radius;
            float y = wheelCenter.Y + MathF.Sin(angle) * 0.6f * radius;
            canvas.DrawCircle(x, y, 0.25f * radius, chassisFillPaint);
            canvas.DrawCircle(x, y, 0.25f * radius, chassisStrokePaint);
        }
    }

    private void DrawChassis(SKCanvas canvas, SKPaint chassisFillPaint, SKPaint chassisStrokePaint)
    {
        var matrix = canvas.TotalMatrix;
        canvas.RotateRadians(_chassis.Rotation, _chassis.Position.X, _chassis.Position.Y);
        var path = new SKPath();
        Vector2 vector = _chassisVertices[0] + _chassis.Position;
        path.MoveTo(vector.X, vector.Y);
        for (int i = 1; i < _chassisVertices.Count; i++) {
            vector = _chassisVertices[i] + _chassis.Position;
            path.LineTo(vector.X, vector.Y);
        }
        path.Close();

        canvas.DrawPath(path, chassisFillPaint);
        canvas.DrawPath(path, chassisStrokePaint);
        foreach (Vector2 v in _chassisVertices) {
            var v1 = v + _chassis.Position;
            canvas.DrawLine(_chassis.Position.X, _chassis.Position.Y, v1.X, v1.Y, chassisStrokePaint);
        }
        canvas.SetMatrix(matrix);
    }

    private void DrawWheelHubs(SKCanvas canvas, SKPaint chassisStrokePaint)
    {
        for (int i = 0; i < WheelCount; i++) {
            var wheelCenter = _wheels[i].Position;
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, 0.07f, _neutralFillPaint);
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, 0.07f, chassisStrokePaint);
        }
    }
}
