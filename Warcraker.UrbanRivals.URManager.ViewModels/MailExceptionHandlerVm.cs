using System;
using Microsoft.Extensions.Logging;

namespace Warcraker.UrbanRivals.URManager.ViewModels
{
    public class MailExceptionHandlerVm : ExceptionHandlerVmBase
    {
        private ILogger<MailExceptionHandlerVm> _log;
        public MailExceptionHandlerVm(ILogger<MailExceptionHandlerVm> log, Exception exception)
            : base(exception)
        {
            _log = log;
            _log.LogInformation("Unhandled exception: {CrashLog}", CrashLog);
            ShouldSendCrashReport = true;
        }

        public override void SendCrashReport()
        {
            throw new NotImplementedException(nameof(SendCrashReport)); // TODO MailExceptionHandlerVm.SendCrashReport
        }
    }
}