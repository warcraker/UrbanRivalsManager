using Autofac;
using System;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Warcraker.UrbanRivals.URManager.ViewModels;

namespace Warcraker.UrbanRivals.URManager.View.ExceptionHandler
{
    public partial class ExceptionHandlerWindow
    {
        private readonly ILogger<ExceptionHandlerWindow> _log;
        private readonly ExceptionHandlerVmBase _vm;
        
        public ExceptionHandlerWindow(Exception exception)
        {
            InitializeComponent();
            using (ILifetimeScope scope = AutofacContainer.INSTANCE.BeginLifetimeScope())
            {
                _log = scope.Resolve<ILogger<ExceptionHandlerWindow>>();
                TypedParameter parameter = TypedParameter.From(exception);
                _vm = AutofacContainer.INSTANCE.Resolve<ExceptionHandlerVmBase>(parameter);
            }

            _log.LogInformation("Called application exception handler. Crash log:{NewLine}{CrashLog}", Environment.NewLine, _vm.CrashLog);
            
            DataContext = _vm;
            Closing += OnWindowIsClosed;
        }

        private void OnWindowIsClosed(object sender, CancelEventArgs args)
        {
            if (_vm.ShouldSendCrashReport)
            {
                _log.LogInformation("Sending crash report...");
                _vm.SendCrashReport(); 
            }
            else
            {
                _log.LogInformation("Crash report will not be sent");
            }
        }
    }
}
