using GeneticCars.Evolution;
using System.ComponentModel;

namespace GeneticCars.Genealogy;

public partial class FamilyTree
{
    private const float ConnectionsStrokeWidth = 1.5f;
    private const float LineHeight = 15f, TextColumnWidth = 135f;
    private const float ConnectionsColumnWidth = 120f, ControlPointDelta = 0.4f * ConnectionsColumnWidth;
    private const float ConnectionYDelta = -4.0f;
    private const float TopBorder = 30f;
    private const float HorizontalBorder = 3f;

    private static readonly SKFont _nodeFont = SKTypeface.FromFamilyName("Arial").ToFont(11f);
    private static readonly SKFont _generationFont = SKTypeface.FromFamilyName("Arial").ToFont(15f);

    private static readonly SKPaint _fitnessTextPaint = new() {
        Color = SKColors.Black,
        IsStroke = false,
        IsAntialias = true,
    };

    private static readonly SKPaint _selectionMarkerPaint = new() {
        Color = SKColors.Blue,
        IsStroke = true,
        IsAntialias = true,
        PathEffect = SKPathEffect.CreateDash([2f, 2f], 0)
    };

    private static readonly SKPaint _newChampionBackgroundPaint = new() {
        Color = SKColors.Yellow,
        IsStroke = false,
        IsAntialias = true,
    };

    private static readonly SKPaint _radioactiveMarkerFillPaint = new() {
        Color = SKColors.Black,
        IsStroke = false,
        IsAntialias = false,
    };
    private static readonly SKPaint _radioactiveMarkerStrokePaint = new() {
        Color = new SKColor(241, 225, 42), // From RadioactiveUp32.png
        StrokeWidth = 1.6f,
        IsStroke = true,
        IsAntialias = true,
    };

    private static readonly SKPaint _kryptoniteMarkerFillPaint = new() {
        Color = SKColors.LightGreen,
        IsStroke = false,
        IsAntialias = false,
    };
    private static readonly SKPaint _kryptoniteMarkerStrokePaint = new() {
        Color = SKColors.Black,
        StrokeWidth = 0.8f,
        IsStroke = true,
        IsAntialias = true,
    };

    private static readonly SKPaint _floorLetterPaint = new() {
        Color = SKColors.White,
        IsStroke = false,
        IsAntialias = false,
    };
    private static readonly SKPaint _radioactivityLetterPaint = new() {
        Color = new SKColor(217, 195, 0),
        IsStroke = false,
        IsAntialias = false,
    };
    private static readonly SKPaint _kryptoniteLetterPaint = new() {
        Color = new SKColor(0, 189, 0),
        IsStroke = false,
        IsAntialias = false,
    };
    private static readonly SKPaint _deathLetterPaint = new() {
        Color = new SKColor(204, 57, 86),
        IsStroke = false,
        IsAntialias = false,
    };

    private const byte StrokeAlpha = 150, FaintStrokeAlpha = 60;

    private static readonly SKPaint _newStrokePaint = new() {
        Color = Individual.NewBaseColor.WithAlpha(StrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
        IsAntialias = true,
    };
    private static readonly SKPaint _newStrongStrokePaint = new() {
        Color = Individual.NewBaseColor.WithAlpha(StrokeAlpha),
        IsStroke = true,
        StrokeWidth = 2 * ConnectionsStrokeWidth,
        IsAntialias = true,
    };
    private static readonly SKPaint _newFaintStrokePaint = new() {
        Color = Individual.NewBaseColor.WithAlpha(FaintStrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
        IsAntialias = true,
    };

    private static readonly SKPaint _eliteStrokePaint = new() {
        Color = Individual.EliteBaseColor.WithAlpha(StrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
        IsAntialias = true
    };
    private static readonly SKPaint _eliteStrongStrokePaint = new() {
        Color = Individual.EliteBaseColor.WithAlpha(StrokeAlpha),
        IsStroke = true,
        StrokeWidth = 2 * ConnectionsStrokeWidth,
        IsAntialias = true
    };
    private static readonly SKPaint _eliteFaintStrokePaint = new() {
        Color = Individual.EliteBaseColor.WithAlpha(FaintStrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
        IsAntialias = true
    };

    private static readonly SKPaint _crossedStrokePaint = new() {
        Color = Individual.CrossedBaseColor.WithAlpha(StrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
        IsAntialias = true
    };
    private static readonly SKPaint _crossedStrongStrokePaint = new() {
        Color = Individual.CrossedBaseColor.WithAlpha(StrokeAlpha),
        IsStroke = true,
        StrokeWidth = 2 * ConnectionsStrokeWidth,
        IsAntialias = true
    };
    private static readonly SKPaint _crossedFaintStrokePaint = new() {
        Color = Individual.CrossedBaseColor.WithAlpha(FaintStrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
        IsAntialias = true
    };

    private static readonly SKPaint _mutatedStrokePaint = new() {
        Color = Individual.MutatedBaseColor.WithAlpha(StrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
        IsAntialias = true
    };
    private static readonly SKPaint _mutatedStrongStrokePaint = new() {
        Color = Individual.MutatedBaseColor.WithAlpha(StrokeAlpha),
        IsStroke = true,
        StrokeWidth = 2 * ConnectionsStrokeWidth,
        IsAntialias = true
    };
    private static readonly SKPaint _mutatedFaintStrokePaint = new() {
        Color = Individual.MutatedBaseColor.WithAlpha(FaintStrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
        IsAntialias = true
    };


    private static SKPaint GetTextPaint(Class @class) =>
        @class switch {
            Class.New or Class.Kryptonite => Individual.NewFillPaint,
            Class.Elite => Individual.EliteFillPaint,
            Class.Crossed => Individual.CrossedFillPaint,
            Class.Mutated or Class.Radioactive => Individual.MutatedFillPaint,
            _ => throw new NotImplementedException()
        };

    private static SKPaint GetConnectionPaint(Class @class, bool isRelated, bool isElite) =>
    (@class, isRelated, isElite) switch {
        (Class.New or Class.Kryptonite, true, _) => _newStrongStrokePaint,
        (Class.New or Class.Kryptonite, false, false) => _newFaintStrokePaint,
        (Class.New or Class.Kryptonite, false, true) => _newStrokePaint,

        (Class.Elite, true, _) => _eliteStrongStrokePaint,
        (Class.Elite, false, false) => _eliteFaintStrokePaint,
        (Class.Elite, false, true) => _eliteStrokePaint,

        (Class.Crossed, true, _) => _crossedStrongStrokePaint,
        (Class.Crossed, false, false) => _crossedFaintStrokePaint,
        (Class.Crossed, false, true) => _crossedStrokePaint,

        (Class.Mutated or Class.Radioactive, true, _) => _mutatedStrongStrokePaint,
        (Class.Mutated or Class.Radioactive, false, false) => _mutatedFaintStrokePaint,
        (Class.Mutated or Class.Radioactive, false, true) => _mutatedStrokePaint,

        _ => throw new NotImplementedException()
    };

    private float _zoom = 1.0f;

    public bool ZoomBy(int delta)
    {
        float zoom = Single.Clamp(_zoom * Single.Pow(1.08f, delta / 120f), 0.5f, 2.0f);
        if (zoom != _zoom) {
            _zoom = zoom;
            return true;
        }
        return false;
    }

    public void Draw(SKCanvas canvas, SKRect viewBox)
    {
        canvas.Clear(SKColors.White);
        canvas.Scale(_zoom, _zoom);
        float top = canvas.LocalClipBounds.Top + TopBorder - viewBox.Top / _zoom;
        float x = canvas.LocalClipBounds.Left + HorizontalBorder - viewBox.Left / _zoom;
        for (int g = 0; g < Generations.Count; g++) {
            if (x + TextColumnWidth > 0 && x < viewBox.Width / _zoom + ConnectionsColumnWidth) {
                float y = top;

                DrawColumnHeader(canvas, x, g, y);
                Node[] population = Generations[g].Population;
                for (int i = 0; i < population.Length; i++) {
                    if (_selected == (g, i, true)) {
                        canvas.DrawRect(x - 1, y - LineHeight + 4.5f, TextColumnWidth, LineHeight, _selectionMarkerPaint);
                    }


                    ref Node node = ref population[i];

                    string text = node.Text + " ";
                    float textWidth = _nodeFont.MeasureText(text);
                    canvas.DrawText(text, x, y, _nodeFont, GetTextPaint(node.Class));

                    text = $"({node.Fitness?.ToString("n1") ?? "?"})";
                    float w = _nodeFont.MeasureText(text, out SKRect bounds);
                    if (i == 0 && g > 0 && Generations[g - 1].Population[0].Fitness < node.Fitness) {
                        bounds.Offset(x + textWidth + 3, y - 2);
                        bounds.Size = new(bounds.Width - 6f, bounds.Height + 2);
                        canvas.DrawRect(bounds, _newChampionBackgroundPaint);
                    }
                    canvas.DrawText(text, x + textWidth, y, _nodeFont, _fitnessTextPaint);
                    textWidth += w + 1f;

                    bool isNewElite = i < population.Length / 4;
                    float lineY = y + ConnectionYDelta;
                    if (textWidth < TextColumnWidth) {
                        canvas.DrawLine(x + textWidth, lineY, x + TextColumnWidth, lineY,
                            GetConnectionPaint(node.Class, IsRelatedToSelection(g, i, null), isNewElite));
                    }
                    if (node.Ancestor1Index is int ancestorIndex1) {
                        DrawConnection(canvas, ancestorIndex1, x, top, node.Class, lineY, IsRelatedToSelection(g, i, 1), isNewElite);
                        if (node.Class is Class.Radioactive) {
                            canvas.DrawCircle(x - 3, lineY, 2.4f, _radioactiveMarkerFillPaint);
                            canvas.DrawCircle(x - 3, lineY, 2.4f, _radioactiveMarkerStrokePaint);
                        }
                    }
                    if (node.Ancestor2Index is int ancestorIndex2) {
                        DrawConnection(canvas, ancestorIndex2, x, top, node.Class, lineY, IsRelatedToSelection(g, i, 2), isNewElite);
                    }
                    if (node.Class == Class.Kryptonite) {
                        canvas.DrawCircle(x - 3, lineY, 2.4f, _kryptoniteMarkerFillPaint);
                        canvas.DrawCircle(x - 3, lineY, 2.4f, _kryptoniteMarkerStrokePaint);
                    }
                    y += LineHeight;
                }
            }
            x += TextColumnWidth + ConnectionsColumnWidth;
        }
    }

    private void DrawColumnHeader(SKCanvas canvas, float x, int g, float y)
    {
        NodesGeneration generation = Generations[g];

        string numberText = g.ToString();
        canvas.DrawText(numberText, x, y - 15, _generationFont, Individual.NeutralInfoTextPaint);

        float letterX = x + (int)_generationFont.MeasureText(numberText) + 10;
        if (generation.NewFloorGenerated) {
            canvas.DrawRect(letterX - 1, y - 28, 12, 15, Floor.FloorFillPaint);
            canvas.DrawText("F", letterX, y - 15, _generationFont, _floorLetterPaint);
            letterX += 15;
        }
        if (generation.RadioactivityApplied) {
            canvas.DrawText("R", letterX, y - 15, _generationFont, _radioactivityLetterPaint);
            letterX += 15;
        }
        if (generation.KryptoniteApplied) {
            canvas.DrawText("K", letterX, y - 15, _generationFont, _kryptoniteLetterPaint);
            letterX += 15;
        }
        if (generation.DeathApplied) {
            canvas.DrawText("D", letterX, y - 15, _generationFont, _deathLetterPaint);
        }
    }

    private static void DrawConnection(SKCanvas canvas, int ancestorIndex, float x, float top, Class @class,
        float lineY, bool isRelated, bool isNewElite)
    {
        float ancestorY = top + ancestorIndex * LineHeight + ConnectionYDelta;
        SKPath path = new();
        path.MoveTo(x - ConnectionsColumnWidth, ancestorY);
        path.CubicTo(
            x - ConnectionsColumnWidth + ControlPointDelta, ancestorY,
            x - ControlPointDelta, lineY,
            x - 1, lineY);
        canvas.DrawPath(path, GetConnectionPaint(@class, isRelated, isNewElite));
    }

    public Size FamilyTreePixelSize
    {
        get {
            int generations = Math.Max(Generations.Count, 1);
            float width = generations * TextColumnWidth + (generations - 1) * ConnectionsColumnWidth + 2 * HorizontalBorder;
            float height = Generations.Count == 0
                ? 100
                : Generations.Max(g => g.Population.Length) * LineHeight + TopBorder + 3f;
            return new((int)(_zoom * width), (int)(_zoom * height));
        }
    }
}
