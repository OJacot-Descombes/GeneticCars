using System;

namespace GeneticCars.Generation;

public abstract partial class Individual
{
    private static readonly SKColor _neutralFillColor = SKColors.White;
    private static readonly SKColor _neutralBaseColor = SKColors.Black;
    private static readonly SKColor _newBaseColor = SKColors.ForestGreen;
    private static readonly SKColor _eliteBaseColor = new(0, 115, 153);
    private static readonly SKColor _crossedBaseColor = SKColors.MediumVioletRed;
    private static readonly SKColor _mutatedBaseColor = SKColors.Chocolate;
    private static readonly SKColor _deadBaseColor = SKColors.LightPink;

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
        Color = _newBaseColor,
        IsStroke = true,
        IsAntialias = true
    };
    protected static readonly SKPaint _newFillPaint = new() {
        Color = _newBaseColor,
        IsStroke = false,
        IsAntialias = false
    };
    protected static readonly SKPaint _newInfoTextPaint = new() {
        Color = DimInfoTextColor(_newBaseColor),
        IsStroke = false,
        IsAntialias = true
    };

    protected static readonly SKPaint _eliteStrokePaint = new() {
        Color = _eliteBaseColor,
        IsStroke = true,
        IsAntialias = true
    };
    protected static readonly SKPaint _eliteFillPaint = new() {
        Color = _eliteBaseColor,
        IsStroke = false,
        IsAntialias = false
    };
    protected static readonly SKPaint _eliteInfoTextPaint = new() {
        Color = DimInfoTextColor(_eliteBaseColor),
        IsStroke = false,
        IsAntialias = true
    };

    protected static readonly SKPaint _crossedStrokePaint = new() {
        Color = _crossedBaseColor,
        IsStroke = true,
        IsAntialias = true
    };
    protected static readonly SKPaint _crossedFillPaint = new() {
        Color = _crossedBaseColor,
        IsStroke = false,
        IsAntialias = false
    };
    protected static readonly SKPaint _crossedInfoTextPaint = new() {
        Color = DimInfoTextColor(_crossedBaseColor),
        IsStroke = false,
        IsAntialias = true
    };

    protected static readonly SKPaint _mutatedStrokePaint = new() {
        Color = _mutatedBaseColor,
        IsStroke = true,
        IsAntialias = true
    };
    protected static readonly SKPaint _mutatedFillPaint = new() {
        Color = _mutatedBaseColor,
        IsStroke = false,
        IsAntialias = false
    };
    protected static readonly SKPaint _mutatedInfoTextPaint = new() {
        Color = DimInfoTextColor(_mutatedBaseColor),
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


    protected static readonly SKPaint _infoTextPaint = new() {
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
            Class.New => _newStrokePaint,
            Class.Elite => _eliteStrokePaint,
            Class.Crossed => _crossedStrokePaint,
            Class.Mutated => _mutatedStrokePaint,
            _ => throw new NotImplementedException()
        }
        : _deadStrokePaint;

    protected SKPaint ColoredFillPaint => IsAlive
        ? Class switch {
            Class.New => _newFillPaint,
            Class.Elite => _eliteFillPaint,
            Class.Crossed => _crossedFillPaint,
            Class.Mutated => _mutatedFillPaint,
            _ => throw new NotImplementedException()
        }
        : _deadFillPaint;

    protected SKPaint InfoTextPaint => IsAlive
    ? Class switch {
        Class.New => _newInfoTextPaint,
        Class.Elite => _eliteInfoTextPaint,
        Class.Crossed => _crossedInfoTextPaint,
        Class.Mutated => _mutatedInfoTextPaint,
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
                Class.New => _newBaseColor,
                Class.Elite => _eliteBaseColor,
                Class.Crossed => _crossedBaseColor,
                Class.Mutated => _mutatedBaseColor,
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
