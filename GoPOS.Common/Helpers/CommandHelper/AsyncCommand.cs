using System;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace GoPOS.Helpers.CommandHelper;

public class AsyncCommand<T> : IAsyncCommand<T>
{
    private readonly Func<T, bool> _canExecute;
    private readonly IErrorHandler _errorHandler;
    private readonly Func<T, Task> _execute;

    private bool _isExecuting;

    public AsyncCommand(Func<T, Task> execute, Func<T, bool>? canExecute = null, IErrorHandler? errorHandler = null)
    {
        _execute = execute;
        _canExecute = canExecute!;
        _errorHandler = errorHandler!;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(T parameter)
    {
        return !_isExecuting && (_canExecute?.Invoke(parameter) ?? true);
    }

    public async Task ExecuteAsync(T parameter)
    {
        if (CanExecute(parameter))
        {
            try
            {
                _isExecuting = true;
                await _execute(parameter);
            }
            finally
            {
                _isExecuting = false;
            }
        }

        RaiseCanExecuteChanged();
    }

    bool ICommand.CanExecute(object? parameter)
    {
        return CanExecute((T)parameter!);
    }

    void ICommand.Execute(object? parameter)
    {
        ExecuteAsync((T)parameter!).FireAndForgetSafeAsync(_errorHandler, BeforeExecute, AfterExecute, (T)parameter!);
    }

    private static void AfterExecute(object obj)
    {
        if (obj is Button btn)
        {
            btn.IsEnabled = true;
        }
    }

    private static void BeforeExecute(object obj)
    {
        if (obj is Button btn)
        {
            btn.IsEnabled = false;
        }
    }

    private void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}
