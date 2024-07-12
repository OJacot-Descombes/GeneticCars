using System.ComponentModel;
using System.Windows.Input;

namespace GeneticCars;

public class Parameters : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    bool _playing = true;
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

    private class RelayCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly Action _commandAction;
        private readonly Func<bool>? _canExecuteCommandAction;

        public RelayCommand(Action commandAction, Func<bool>? canExecuteCommandAction = null)
        {
            _commandAction = commandAction;
            _canExecuteCommandAction = canExecuteCommandAction;
        }

        bool ICommand.CanExecute(object? parameter)
            => _canExecuteCommandAction is null || _canExecuteCommandAction.Invoke();

        void ICommand.Execute(object? parameter)
            => _commandAction.Invoke();

        /// <summary>
        ///  Triggers sending a notification, that the command availability has changed.
        /// </summary>
        public void NotifyCanExecuteChanged()
            => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
