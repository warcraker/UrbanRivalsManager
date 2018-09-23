using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace UrbanRivalsManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // TODO: HANDLE CORRECTLY
            //MessageBox.Show("An unhandled exception just occurred: " + e.Exception.Message, "Exception Sample", MessageBoxButton.OK, MessageBoxImage.Warning);
            //e.Handled = true;

            /*
            Exception e = (Exception)args.ExceptionObject;
            StringBuilder sb = new StringBuilder(e.Message);
            Exception copy = e;
            while (copy.InnerException != null)
            {
                copy = copy.InnerException;
                sb.Append(copy.Message);
            }
            MessageBox.Show("Path: " + System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
                + Environment.NewLine + "Runtime terminating: " + args.IsTerminating
                + Environment.NewLine + "Handler caught: " + sb.ToString());
             */
        }
    }
}
