namespace GeneticCars.Cars;

public partial class Car
{
    private static readonly SKFont _carFont = SKTypeface.FromFamilyName("Arial").ToFont(0.5f);

    public void Draw(SKCanvas canvas)
    {
        DrawInfo(canvas);
        DrawWheels(canvas);

        var chassisStrokePaint = ColoredStrokePaint;
        DrawChassis(canvas, chassisStrokePaint);
        DrawWheelHubs(canvas, chassisStrokePaint);

        // -- Test tangents ---
        //using var stroke = new SKPaint {
        //    Color = SKColors.Red,
        //    IsStroke = true,
        //    IsAntialias = true
        //};
        //var (t1, t2) = Geometry.ExternalTangentLines(
        //    _wheels[0].Position, WheelRadius(0).Value,
        //    _wheels[1].Position, WheelRadius(1).Value);
        //canvas.DrawLine(t1.P1.X, t1.P1.Y, t1.P2.X, t1.P2.Y, stroke);
        //canvas.DrawLine(t2.P1.X, t2.P1.Y, t2.P2.X, t2.P2.Y, stroke);
    }

    private void DrawInfo(SKCanvas canvas)
    {
        var matrix = canvas.TotalMatrix;
        canvas.Scale(1, -1, 0, _chassis.Position.Y);
        string text = Name + " " + Generation;   // $"{_chassis.LinearVelocity.X:n3}  H = {Health}"
        SKPaint textPaint = IsAlive ? _textPaint : _deadTextPaint;
        canvas.DrawText(text, _chassis.Position.X - 1, _chassis.Position.Y - 3, _carFont, textPaint);
        canvas.SetMatrix(matrix);

        if (IsAlive && Health < Game.MaxCarHealth) {
            ColoredStrokePaint.IsStroke = false;
            canvas.DrawRect(_chassis.Position.X - 1, _chassis.Position.Y + 2.7f, 2f * Health / Game.MaxCarHealth,
                0.15f, ColoredStrokePaint);
            ColoredStrokePaint.IsStroke = true;
            canvas.DrawRect(_chassis.Position.X - 1, _chassis.Position.Y + 2.7f, 2, 0.15f, _infoStrokePaint);
        }
    }

    private void DrawWheels(SKCanvas canvas)
    {
        for (int i = 0; i < WheelCount; i++) {
            var wheelCenter = _wheels[i].Position;
            using var wheelFillPaint = CreateNeutralFillPaint(WheelDensity(i).Fraction);
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, WheelRadius(i).Value, wheelFillPaint);
            canvas.DrawCircle(wheelCenter.X, wheelCenter.Y, WheelRadius(i).Value, NeutralStrokePaint);
        }
    }

    private void DrawChassis(SKCanvas canvas, SKPaint chassisStrokePaint)
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

        var chassisFillPaint = CreateColoredFillPaint(ChassisDensity.Fraction);
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
