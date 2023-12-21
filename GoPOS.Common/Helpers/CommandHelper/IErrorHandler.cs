using System;

namespace GoPOS.Helpers.CommandHelper;

public interface IErrorHandler
{
    void HandleError(Exception ex);
}
