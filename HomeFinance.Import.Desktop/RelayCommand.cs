using System;
using System.Windows.Input;

namespace HomeFinance.Import.Desktop;

public class RelayCommand : ICommand
{
    private readonly Action<object?> _execute;
    private readonly Func<object?, bool>? _canExecute;
    
    public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
    {
        this._execute = execute;
        this._canExecute = canExecute;
    }

    public void RaiseCanExecuteChanged()
    {
        if (CanExecuteChanged != null)
            CanExecuteChanged(this, EventArgs.Empty);
    }

    public bool CanExecute(object? parameter)
    {
        return this._canExecute == null || this._canExecute(parameter);
    }

    public void Execute(object? parameter)
    {
        this._execute(parameter);
    }

    public event EventHandler? CanExecuteChanged;
}