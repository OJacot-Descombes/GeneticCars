namespace GeneticCars;

public class FpsMeter
{
    private static readonly SKPaint _fpsPaint = new() {
        Color = SKColors.Red,
        IsStroke = false,
        IsAntialias = true
    };
    private static readonly SKFont _fpsFont = SKTypeface.FromFamilyName("Arial").ToFont(0.5f);

    private int _lastFps, _updates;

    private DateTime _lastUpdateTime = DateTime.UtcNow;

    public void Update(SKCanvas canvas)
    {
        _updates++;
        if (_updates >= 15) {
            var currentDateTime = DateTime.UtcNow;
            var elapsed = currentDateTime - _lastUpdateTime;
            _lastUpdateTime = currentDateTime;
            int fps = (int)Math.Round(_updates / elapsed.TotalSeconds);
            _lastFps = fps;
            _updates = 0;
        }

        float y = canvas.LocalClipBounds.Bottom - 1.5f;
        canvas.Scale(1, -1, 0, y);
        canvas.DrawText($"FPS: {_lastFps}", canvas.LocalClipBounds.Left + 0.1f, y, _fpsFont, _fpsPaint);
    }
}
