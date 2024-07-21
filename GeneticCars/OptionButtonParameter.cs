using System.ComponentModel;

namespace GeneticCars;

public class OptionButtonParameter(Bitmap buttonUpImage, Bitmap buttonDownImage) : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    private bool _value;
    public bool Value
    {
        get { return _value; }
        set {
            if (value != _value) {
                _value = value;
                OnPropertyChanged(nameof(Value));
                if (value) {
                    Image = buttonDownImage;
                    SwitchedOn();
                } else {
                    Image = buttonUpImage;
                }
            }
        }
    }

    private bool _Enabled;
    public bool Enabled
    {
        get { return _Enabled; }
        set {
            if (value != _Enabled) {
                _Enabled = value;
                OnPropertyChanged(nameof(Enabled));
            }
        }
    }

#pragma warning disable CS9124 // Parameter is captured into the state of the enclosing type and its value is also used to initialize a field, property, or event.
    private Bitmap _image = buttonUpImage;
#pragma warning restore CS9124
    public Bitmap Image
    {
        get { return _image; }
        set {
            if (value != _image) {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
    }

    protected virtual void SwitchedOn()
    {
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
