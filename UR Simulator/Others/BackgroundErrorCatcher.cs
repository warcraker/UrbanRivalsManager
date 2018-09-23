using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UrbanRivalsManager.Utils
{
    internal class BindingErrorTraceListener : DefaultTraceListener
    {
        private static BindingErrorTraceListener _Listener;
        private StringBuilder _Message = new StringBuilder();

        private BindingErrorTraceListener() { }

        public static void SetTrace()
        {
            SetTrace(SourceLevels.Error, TraceOptions.None);
        }
        public static void SetTrace(SourceLevels level, TraceOptions options)
        {
            if (_Listener == null)
            {
                _Listener = new BindingErrorTraceListener();
                PresentationTraceSources.DataBindingSource.Listeners.Add(_Listener);
            }

            _Listener.TraceOutputOptions = options;
            PresentationTraceSources.DataBindingSource.Switch.Level = level;
        }
        public static void CloseTrace()
        {
            if (_Listener == null)
                return;

            _Listener.Flush();
            _Listener.Close();
            PresentationTraceSources.DataBindingSource.Listeners.Remove(_Listener);
            _Listener = null;
        }

        public override void Write(string message)
        {
            _Message.Append(message);
        }
        public override void WriteLine(string message)
        {
            _Message.Append(message);

            var final = _Message.ToString();
            _Message.Length = 0;

            DisplayError(final);
        }

        private void DisplayError(string message)
        {
            #if DEBUG
            MessageBox.Show(message, "Binding Error", MessageBoxButton.OK, MessageBoxImage.Error);
            #else
            // TODO: Launch Error log instead of MessageBox
            MessageBox.Show(message, "Binding Error", MessageBoxButton.OK, MessageBoxImage.Error);
            #endif
        }
    }

    internal static class BackgroundErrorCatcher
    {
        public static void Init()
        {
            SubscribeExceptionHandler();
            SetBindingErrorTraceListener();
        }

        [ConditionalAttribute("RELEASE")]
        private static void SubscribeExceptionHandler()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(ExceptionHandler);
        }

        private static void ExceptionHandler(object sender, UnhandledExceptionEventArgs args)
        {
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

            // TODO: Log and force close
        }

        private static void SetBindingErrorTraceListener()
        {
            BindingErrorTraceListener.SetTrace();
        }
    }
}
