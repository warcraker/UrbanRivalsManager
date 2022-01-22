using System;
using System.Windows;
using Warcraker.UrbanRivals.URManager.View.ExceptionHandler;

namespace Warcraker.UrbanRivals.URManager
{
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender,
            System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                var window = new ExceptionHandlerWindow(e.Exception);
                window.ShowDialog();
                e.Handled = true;
            }
            finally
            {
                Current.Shutdown();
            }

            // TODO: Press Close application
            // TODO: Press Continue
            // TODO: Close window
            // TODO: Send email
        }
    }
}