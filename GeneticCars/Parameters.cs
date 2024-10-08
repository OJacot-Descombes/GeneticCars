﻿using GeneticCars.Properties;
using System.ComponentModel;
using System.Windows.Input;

namespace GeneticCars;

public class Parameters : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public Parameters()
    {
        Radioactivity = new(this);
        Kryptonite = new(this);
        Death = new(this);

        MutationRate = PercentValues[8].Value;
        MutationSize = PercentValues[8].Value;
    }

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

    int _iterations = 4;
    public int Iterations
    {
        get { return _iterations; }
        set {
            if (value != _iterations) {
                _iterations = value;
                OnPropertyChanged(nameof(Iterations));
                OnPropertyChanged(nameof(SpeedDisplay));
            }
        }
    }
    public string SpeedDisplay => (Iterations / 4f).ToString("n2");

    public int Zoom { get; set; } = 30;

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

    public string PlayButtonText => _playing ? "\u23f8 Pause" : "\u23f5 Resume";

    public bool DisplayNames { get; set; }
    public bool DisplayHealthBar { get; set; } = true;
    public bool DisplayFps { get; set; }

    public float MutationRate { get; set; }
    public float MutationSize { get; set; }

    public class FloatDisplay(float value, string text)
    {
        public float Value { get; } = value;
        public string Text { get; } = text;
    }

    public FloatDisplay[] PercentValues { get; } = [
        new(0.010f, "    1%"), new(0.015f, "    1.5%"), new(0.022f, "    2.2%"), new(0.032f, "    3.2%"), new(0.046f, "    4.6%"), new(0.068f, "    6.8%"),
        new(0.100f, "  10%"), new(0.15f, "  15%"), new(0.22f, "  22%"), new(0.32f, "  32%"), new(0.46f, "  46%"), new(0.68f, "  68%"),
        new(1.0f, "100%")];

    public OptionButtonParameter RegenerateFloor { get; } = new(
        Resources.ChangeFloorUp32, Resources.ChangeFloorDn32) { Enabled = true };

    public RadioactivityParameter Radioactivity { get; }
    public KryptoniteParameter Kryptonite { get; }
    public DeathParameter Death { get; }

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

    public class RadioactivityParameter(Parameters parameters)
        : OptionButtonParameter(Resources.RadioactiveUp32, Resources.RadioactiveDn32)
    {
        protected override void SwitchedOn()
        {
            parameters.Death.Value = false;
        }
    }

    public class KryptoniteParameter(Parameters parameters)
        : OptionButtonParameter(Resources.Kryptonite32Up, Resources.Kryptonite32Dn)
    {
        protected override void SwitchedOn()
        {
            parameters.Death.Value = false;
        }
    }

    public class DeathParameter(Parameters parameters)
        : OptionButtonParameter(Resources.DeathUp32, Resources.DeathDn32)
    {
        protected override void SwitchedOn()
        {
            parameters.Radioactivity.Value = false;
            parameters.Kryptonite.Value = false;
        }
    }
}
