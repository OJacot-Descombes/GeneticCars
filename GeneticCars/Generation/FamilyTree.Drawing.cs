﻿using static GeneticCars.Generation.FamilyTree;
using static System.Net.Mime.MediaTypeNames;

namespace GeneticCars.Generation;

public partial class FamilyTree
{
    private const float ConnectionsStrokeWidth = 1.5f;
    private const float LineHeight = 15f, TextColumnWidth = 125f;
    private const float ConnectionsColumnWidth = 110f, ControlPointDelta = 0.4f * ConnectionsColumnWidth;
    private const float ConnectionYDelta = -3f;
    private const float TopBorder = 30f;
    private const float LeftBorder = 3f;

    private static readonly SKFont _nodeFont = SKTypeface.FromFamilyName("Arial").ToFont(11f);
    private static readonly SKFont _generationFont = SKTypeface.FromFamilyName("Arial").ToFont(15f);

    private static readonly SKPaint _fitnessTextPaint = new() {
        Color = SKColors.Black,
        IsStroke = false,
        IsAntialias = true,
    };

    private const byte StrokeAlpha = 150, FaintStrokeAlpha = 60;

    private static readonly SKPaint _newStrokePaint = new() {
        Color = Individual.NewBaseColor.WithAlpha(StrokeAlpha),
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
    private static readonly SKPaint _crossedStrokePaint = new() {
        Color = Individual.CrossedBaseColor.WithAlpha(StrokeAlpha),
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

    private static readonly SKPaint _newFaintStrokePaint = new() {
        Color = Individual.NewBaseColor.WithAlpha(FaintStrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
        IsAntialias = true,
    };
    private static readonly SKPaint _eliteFaintStrokePaint = new() {
        Color = Individual.EliteBaseColor.WithAlpha(FaintStrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
        IsAntialias = true
    };
    private static readonly SKPaint _crossedFaintStrokePaint = new() {
        Color = Individual.CrossedBaseColor.WithAlpha(FaintStrokeAlpha),
        IsStroke = true,
        StrokeWidth = ConnectionsStrokeWidth,
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
            Class.New => Individual.NewFillPaint,
            Class.Elite => Individual.EliteFillPaint,
            Class.Crossed => Individual.CrossedFillPaint,
            Class.Mutated => Individual.MutatedFillPaint,
            _ => throw new NotImplementedException()
        };

    private static SKPaint GetConnectionPaint(Class @class) =>
    @class switch {
        Class.New => _newStrokePaint,
        Class.Elite => _eliteStrokePaint,
        Class.Crossed => _crossedStrokePaint,
        Class.Mutated => _mutatedStrokePaint,
        _ => throw new NotImplementedException()
    };

    private static SKPaint GetFaintConnectionPaint(Class @class) =>
@class switch {
    Class.New => _newFaintStrokePaint,
    Class.Elite => _eliteFaintStrokePaint,
    Class.Crossed => _crossedFaintStrokePaint,
    Class.Mutated => _mutatedFaintStrokePaint,
    _ => throw new NotImplementedException()
};

    public void Draw(SKCanvas canvas)
    {
        canvas.Clear(SKColors.White);
        float top = canvas.LocalClipBounds.Top + TopBorder;
        float x = canvas.LocalClipBounds.Left + LeftBorder;
        for (int g = 0; g < Generations.Count; g++) {
            float y = top;
            canvas.DrawText(g.ToString(), x, y - 15, _generationFont, Individual.NeutralInfoTextPaint);
            for (int i = 0; i < Generations[g].Length; i++) {
                ref Node node = ref Generations[g][i];

                float textWidth = _nodeFont.MeasureText(node.Text);
                canvas.DrawText(node.Text, x, y, _nodeFont, GetTextPaint(node.Class));

                string text = $" ({node.Fitness:n1})";
                canvas.DrawText(text, x + textWidth, y, _nodeFont, _fitnessTextPaint);
                textWidth += _nodeFont.MeasureText(text) + 1f;

                bool isNewElite = i < Generations[g].Length / 4;
                float lineY = y + ConnectionYDelta;
                if (textWidth < TextColumnWidth) {
                    canvas.DrawLine(x + textWidth, lineY, x + TextColumnWidth, lineY, 
                        isNewElite ? GetConnectionPaint(node.Class) : GetFaintConnectionPaint(node.Class));
                }
                if (node.Ancestor1Index is int ancestorIndex1) {
                    DrawConnection(canvas, ancestorIndex1, x, top, node.Class, lineY, isNewElite);
                }
                if (node.Ancestor2Index is int ancestorIndex2) {
                    DrawConnection(canvas, ancestorIndex2, x, top, node.Class, lineY, isNewElite);
                }
                y += LineHeight;
            }
            x += TextColumnWidth + ConnectionsColumnWidth;
        }
    }

    private static void DrawConnection(SKCanvas canvas, int ancestorIndex, float x, float top, Class @class,
        float lineY, bool isNewElite)
    {
        float ancestorY = top + ancestorIndex * LineHeight + ConnectionYDelta;
        SKPath path = new();
        path.MoveTo(x - ConnectionsColumnWidth, ancestorY);
        path.CubicTo(
            x - ConnectionsColumnWidth + ControlPointDelta, ancestorY,
            x - ControlPointDelta, lineY,
            x, lineY);
        canvas.DrawPath(path, isNewElite ? GetConnectionPaint(@class) : GetFaintConnectionPaint(@class));
    }

    public Size FamilyTreePixelSize
    {
        get {
            int generations = Math.Max(Generations.Count, 1);
            float width = generations * TextColumnWidth + (generations - 1) * ConnectionsColumnWidth + 2 * LeftBorder;
            float height = Generations.Count == 0
                ? 100
                : Generations.Max(g => g.Length) * LineHeight + TopBorder + 3f;
            return new((int)width, (int)height);
        }
    }
}