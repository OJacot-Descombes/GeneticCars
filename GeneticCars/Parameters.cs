using System.ComponentModel;
using System.Windows.Input;

namespace GeneticCars;

public class Parameters : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    int _populationSize = 40;
    public int PopulationSize
    {
        get {
            return _populationSize;
        }
        set {
            value = 4 * (value / 4);
            if (value != _populationSize) {
                _populationSize = value;
                OnPropertyChanged(nameof(PopulationSize));
            }
        }
    }

    private bool _playing = true;
    public bool Playing
    {
        get { return _playing; }
        set {
            _playing = value;
            OnPropertyChanged(nameof(Playing));
            OnPropertyChanged(nameof(PlayButtonText));
        }
    }

    public string PlayButtonText => _playing ? "Pause" : "Resume";

    bool _regenerateFloor;
    public bool RegenerateFloor
    {
        get { return _regenerateFloor; }
        set {
            if (value != _regenerateFloor) {
                _regenerateFloor = value;
                OnPropertyChanged(nameof(RegenerateFloor));
                if (value) {
                    RegenerateFloorImage = Properties.Resources.ChangeFloorDn32;
                } else {
                    RegenerateFloorImage = Properties.Resources.ChangeFloorUp32;
                }
            }
        }
    }

    private Bitmap _regenerateFloorImage = Properties.Resources.ChangeFloorUp32;
    public Bitmap RegenerateFloorImage
    {
        get { return _regenerateFloorImage; }
        set {
            if (value != _regenerateFloorImage) {
                _regenerateFloorImage = value;
                OnPropertyChanged(nameof(RegenerateFloorImage));
            }
        }
    }


    public bool DisplayFps { get; set; }

    private bool _radioactivity;
    public bool Radioactivity
    {
        get { return _radioactivity; }
        set {
            if (value != _radioactivity) {
                _radioactivity = value;
                OnPropertyChanged(nameof(Radioactivity));
                if (value) {
                    RadioactivityImage = Properties.Resources.RadioactiveDn32;
                    Death = false;
                } else {
                    RadioactivityImage = Properties.Resources.RadioactiveUp32;
                }
            }
        }
    }

    private bool _radioactivityEnabled;
    public bool RadioactivityEnabled
    {
        get { return _radioactivityEnabled; }
        set {
            if (value != _radioactivityEnabled) {
                _radioactivityEnabled = value;
                OnPropertyChanged(nameof(RadioactivityEnabled));
            }
        }
    }

    private Bitmap _radioactivityImage = Properties.Resources.RadioactiveUp32;
    public Bitmap RadioactivityImage
    {
        get { return _radioactivityImage; }
        set {
            if (value != _radioactivityImage) {
                _radioactivityImage = value;
                OnPropertyChanged(nameof(RadioactivityImage));
            }
        }
    }

    private bool _kryptonite;
    public bool Kryptonite
    {
        get { return _kryptonite; }
        set {
            if (value != _kryptonite) {
                _kryptonite = value;
                OnPropertyChanged(nameof(Kryptonite));
                if (value) {
                    KryptoniteImage = Properties.Resources.Kryptonite32Dn;
                    Death = false;
                } else {
                    KryptoniteImage = Properties.Resources.Kryptonite32Up;
                }
            }
        }
    }

    private bool _kryptoniteEnabled;
    public bool KryptoniteEnabled
    {
        get { return _kryptoniteEnabled; }
        set {
            if (value != _kryptoniteEnabled) {
                _kryptoniteEnabled = value;
                OnPropertyChanged(nameof(KryptoniteEnabled));
            }
        }
    }

    private Bitmap _kryptoniteImage = Properties.Resources.Kryptonite32Up;
    public Bitmap KryptoniteImage
    {
        get { return _kryptoniteImage; }
        set {
            if (value != _kryptoniteImage) {
                _kryptoniteImage = value;
                OnPropertyChanged(nameof(KryptoniteImage));
            }
        }
    }

    private bool _death;
    public bool Death
    {
        get { return _death; }
        set {
            if (value != _death) {
                _death = value;
                OnPropertyChanged(nameof(Death));
                if (value) {
                    DeathImage = Properties.Resources.DeathDn32;
                    Radioactivity = false;
                    Kryptonite = false;
                } else {
                    DeathImage = Properties.Resources.DeathUp32;
                }
            }
        }
    }

    private bool _deathEnabled;
    public bool DeathEnabled
    {
        get { return _deathEnabled; }
        set {
            if (value != _deathEnabled) {
                _deathEnabled = value;
                OnPropertyChanged(nameof(DeathEnabled));
            }
        }
    }

    private Bitmap _deathImage = Properties.Resources.DeathUp32;
    public Bitmap DeathImage
    {
        get { return _deathImage; }
        set {
            if (value != _deathImage) {
                _deathImage = value;
                OnPropertyChanged(nameof(DeathImage));
            }
        }
    }

    private ICommand? _playCommand;
    public ICommand PlayCommand => _playCommand ??= new RelayCommand(() => { Playing = !Playing; });

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    private class RelayCommand(Action commandAction, Func<bool>? canExecuteCommand = null) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        bool ICommand.CanExecute(object? parameter) => canExecuteCommand is null || canExecuteCommand();

        void ICommand.Execute(object? parameter) => commandAction();

        /// <summary>
        ///  Triggers sending a notification, that the command availability has changed.
        /// </summary>
        public void NotifyCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
