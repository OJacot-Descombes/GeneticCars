namespace GeneticCars.Cars;

public class LabelPlacer
{
    private const float Granularity = 0.05f;
    private const float InvGranularity = 1f / Granularity;

    private float _lowerBound = -5;
    private float _upperBound = +10;
    private float[]? _rightMost;

    public float GetVerticalDisplacement(SKRect rect)
    {
        float lower = rect.Top;
        float upper = rect.Bottom;
        EnsureCapacity(lower, upper);

        int i0 = Index(lower);
        int i1 = Index(upper);
        int di = i1 - i0;
        int old_i0 = i0;
        bool found;
        do {
            found = true;
            for (int i = Math.Min(i1, _rightMost!.Length - 1); i >= i0; i--) {
                if (rect.Left < _rightMost![i]) {
                    found = false;
                    i0 = i + 1;
                    i1 = i0 + di;
                }
            }
        } while (!found);
        for (int i = i0; i <= Math.Min(i1, _rightMost!.Length - 1); i++) {
            _rightMost[i] = Math.Max(_rightMost[i], rect.Right);
        }
        return (i0 - old_i0) * Granularity;
    }

    public void Reset()
    {
        if (_rightMost is not null) {
            Array.Fill(_rightMost, -1000f);
        }
    }

    private int Index(float value) => (int)((value - _lowerBound) * InvGranularity);

    private void EnsureCapacity(float lowerValue, float upperValue)
    {
        float newLowerBound = _lowerBound;
        bool changed = false;
        while (newLowerBound > lowerValue) {
            newLowerBound -= 5;
            changed = true;
        }

        float newUpperBound = _upperBound;
        while (newUpperBound < upperValue + 10) {
            newUpperBound += 5;
            changed = true;
        }

        if (_rightMost is null) {
            _lowerBound = newLowerBound;
            _upperBound = newUpperBound;
            _rightMost = new float[Index(newUpperBound) - Index(newLowerBound) + 1];
            Array.Fill(_rightMost, -1000f);
        } else if (changed) {
            int i0 = Index(newLowerBound); // Negative if new lower bound.
            int i1 = Index(newUpperBound);
            float[] newArray = new float[i1 - i0 + 1];
            Array.Fill(_rightMost, -1000);
            Array.Copy(_rightMost, 0, newArray, -i0, _rightMost.Length);
            _rightMost = newArray;
            _lowerBound = newLowerBound;
            _upperBound = newUpperBound;
        }
    }
}
