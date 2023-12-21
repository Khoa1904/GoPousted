using System;
using System.Threading.Tasks;

namespace GoPOS.Helpers.CommandHelper;

public static class TaskUtilities
{
    public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler? handler = null, Action<object>? beforeExecute = null, Action<object>? afterExecute = null, object? obj = null)
    {
        try
        {
            beforeExecute?.Invoke(obj!);
            await task;
            afterExecute?.Invoke(obj!);
        }
        catch (Exception ex)
        {
            handler?.HandleError(ex);
        }
    }
}
