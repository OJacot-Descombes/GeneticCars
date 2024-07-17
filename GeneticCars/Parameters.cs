using System.ComponentModel;
using System.Windows.Input;

namespace GeneticCars;

public class Parameters : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public int PopulationSize { get; } = 40;

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

    public bool ChangingFloor { get; set; }

    public bool DisplayFps { get; set; }

    private bool _mutationBoost;
    public bool MutationBoost
    {
        get { return _mutationBoost; }
        set {
            if (value != _mutationBoost) {
                _mutationBoost = value;
                OnPropertyChanged(nameof(MutationBoost));
                if (value) {
                    MutationBoostImage = Properties.Resources.RadioactiveDn32;
                    Death = false;
                } else {
                    MutationBoostImage = Properties.Resources.RadioactiveUp32;
                }
            }
        }
    }

    private bool _mutationBoostEnabled;
    public bool MutationBoostEnabled
    {
        get { return _mutationBoostEnabled; }
        set {
            if (value != _mutationBoostEnabled) {
                _mutationBoostEnabled = value;
                OnPropertyChanged(nameof(MutationBoostEnabled));
            }
        }
    }

    private Bitmap _mutationBoostImage = Properties.Resources.RadioactiveUp32;
    public Bitmap MutationBoostImage
    {
        get { return _mutationBoostImage; }
        set {
            if (value != _mutationBoostImage) {
                _mutationBoostImage = value;
                OnPropertyChanged(nameof(MutationBoostImage));
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
                    MutationBoost = false;
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

    private void OnPropertyChanged(string propertyName)
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
