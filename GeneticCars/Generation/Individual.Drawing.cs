namespace GeneticCars.Generation;

public abstract partial class Individual
{
    public static readonly SKColor NewBaseColor = SKColors.ForestGreen;
    public static readonly SKColor EliteBaseColor = new(0, 115, 153);
    public static readonly SKColor CrossedBaseColor = SKColors.MediumVioletRed;
    public static readonly SKColor MutatedBaseColor = SKColors.Chocolate;

    private static readonly SKColor _deadBaseColor = SKColors.LightPink;
    private static readonly SKColor _neutralFillColor = SKColors.White;
    private static readonly SKColor _neutralBaseColor = SKColors.Black;

    protected static readonly SKPaint _neutralStrokePaint = new() {
        Color = _neutralBaseColor,
        IsStroke = true,
        IsAntialias = true
    };
    protected static readonly SKPaint _neutralFillPaint = new() {
        Color = _neutralFillColor,
        IsStroke = false,
        IsAntialias = false
    };

    protected static readonly SKPaint _newStrokePaint = new() {
        Color = NewBaseColor,
        IsStroke = true,
        IsAntialias = true
    };
    public static readonly SKPaint NewFillPaint = new() {
        Color = NewBaseColor,
        IsStroke = false,
        IsAntialias = false
    };
    protected static readonly SKPaint _newInfoTextPaint = new() {
        Color = DimInfoTextColor(NewBaseColor),
        IsStroke = false,
        IsAntialias = true
    };

    protected static readonly SKPaint _eliteStrokePaint = new() {
        Color = EliteBaseColor,
        IsStroke = true,
        IsAntialias = true
    };
    public static readonly SKPaint EliteFillPaint = new() {
        Color = EliteBaseColor,
        IsStroke = false,
        IsAntialias = false
    };
    protected static readonly SKPaint _eliteInfoTextPaint = new() {
        Color = DimInfoTextColor(EliteBaseColor),
        IsStroke = false,
        IsAntialias = true
    };

    protected static readonly SKPaint _crossedStrokePaint = new() {
        Color = CrossedBaseColor,
        IsStroke = true,
        IsAntialias = true
    };
    public static readonly SKPaint CrossedFillPaint = new() {
        Color = CrossedBaseColor,
        IsStroke = false,
        IsAntialias = false
    };
    protected static readonly SKPaint _crossedInfoTextPaint = new() {
        Color = DimInfoTextColor(CrossedBaseColor),
        IsStroke = false,
        IsAntialias = true
    };

    protected static readonly SKPaint _mutatedStrokePaint = new() {
        Color = MutatedBaseColor,
        IsStroke = true,
        IsAntialias = true
    };
    public static readonly SKPaint MutatedFillPaint = new() {
        Color = MutatedBaseColor,
        IsStroke = false,
        IsAntialias = false
    };
    protected static readonly SKPaint _mutatedInfoTextPaint = new() {
        Color = DimInfoTextColor(MutatedBaseColor),
        IsStroke = false,
        IsAntialias = true
    };

    protected static readonly SKPaint _deadStrokePaint = new() {
        Color = _deadBaseColor,
        IsStroke = true,
        IsAntialias = true
    };
    protected static readonly SKPaint _deadFillPaint = new() {
        Color = _deadBaseColor,
        IsStroke = false,
        IsAntialias = false
    };

    public static readonly SKPaint NeutralInfoTextPaint = new() {
        Color = SKColors.Gray,
        IsStroke = false,
        IsAntialias = true
    };

    protected static readonly SKPaint _infoStrokePaint = new() {
        Color = SKColors.Gray,
        IsStroke = true,
        IsAntialias = false
    };

    protected static readonly SKPaint _deadTextPaint = new() {
        Color = _deadBaseColor,
        IsStroke = false,
        IsAntialias = true
    };

    protected SKPaint NeutralStrokePaint => IsAlive ? _neutralStrokePaint : _deadStrokePaint;

    protected SKPaint ColoredStrokePaint => IsAlive
        ? Class switch {
            Class.New or Class.Kryptonite => _newStrokePaint,
            Class.Elite => _eliteStrokePaint,
            Class.Crossed => _crossedStrokePaint,
            Class.Mutated or Class.Boosted => _mutatedStrokePaint,
            _ => throw new NotImplementedException()
        }
        : _deadStrokePaint;

    protected SKPaint ColoredFillPaint => IsAlive
        ? Class switch {
            Class.New or Class.Kryptonite => NewFillPaint,
            Class.Elite => EliteFillPaint,
            Class.Crossed => CrossedFillPaint,
            Class.Mutated or Class.Boosted => MutatedFillPaint,
            _ => throw new NotImplementedException()
        }
        : _deadFillPaint;

    protected SKPaint ColoredInfoTextPaint => IsAlive
    ? Class switch {
        Class.New or Class.Kryptonite => _newInfoTextPaint,
        Class.Elite => _eliteInfoTextPaint,
        Class.Crossed => _crossedInfoTextPaint,
        Class.Mutated or Class.Boosted => _mutatedInfoTextPaint,
        _ => throw new NotImplementedException()
    }
    : _deadFillPaint;

    protected SKPaint CreateNeutralFillPaint(float density)
    {
        return IsAlive
            ? CreateFillPaint(density, _neutralBaseColor)
            : CreateFillPaint(density, _deadBaseColor);
    }

    protected SKPaint CreateColoredFillPaint(float density)
    {
        SKColor baseColor = IsAlive
            ? Class switch {
                Class.New or Class.Kryptonite => NewBaseColor,
                Class.Elite => EliteBaseColor,
                Class.Crossed => CrossedBaseColor,
                Class.Mutated or Class.Boosted => MutatedBaseColor,
                _ => throw new NotImplementedException()
            }
            : _deadBaseColor;
        return CreateFillPaint(density, baseColor);
    }

    private static SKPaint CreateFillPaint(float density, SKColor baseColor)
    {
        const float Min = 0.4f, Max = 0.9f;
        const float Range = Max - Min;

        baseColor.ToHsl(out float h, out float s, out float v);
        float vRange = 100f - v;
        var color = SKColor.FromHsl(h, s, v + vRange * Min + (vRange * Range * (1f - density)));
        return new SKPaint {
            Color = color,
            IsStroke = false,
            IsAntialias = false
        };
    }

    private static SKColor DimInfoTextColor(SKColor baseColor)
    {
        baseColor.ToHsl(out float h, out float s, out float v);
        return SKColor.FromHsl(h, 0.5f * s, (v + 50f) / 1.5f);
    }
}
