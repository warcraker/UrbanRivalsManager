using System;
using System.Text;

namespace Warcraker.UrbanRivals.URManager.ViewModels
{
    public abstract class ExceptionHandlerVmBase
    {
        public string CrashLog { get; }
        public Exception Exception { get; }
        public bool ShouldSendCrashReport { get; set; }

        public ExceptionHandlerVmBase(Exception exception)
        {
            CrashLog = FormatExceptionsStack(exception);
            Exception = exception;
        }

        public abstract void SendCrashReport();
        
        private static string FormatExceptionsStack(Exception exception)
        {
            var result = new StringBuilder();
            Exception current = exception;
            do
            {
                result.AppendLine(exception.Message);
                result.AppendLine(exception.StackTrace);
                current = current.InnerException;
            } while (current != null);

            return result.ToString();
        }
    }
}