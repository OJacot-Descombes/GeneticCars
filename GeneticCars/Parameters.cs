using System.ComponentModel;
using System.Windows.Input;

namespace GeneticCars;

public class Parameters : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

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
