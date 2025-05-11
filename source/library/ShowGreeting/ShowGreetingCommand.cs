// Copyright (c) 2025 Anthony J. Raymond, MIT License

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace PoshGUIExample.ShowGreeting {
    [Cmdlet(VerbsCommon.Show, "Greeting")]
    [OutputType(typeof(void))]
    public class ShowGreetingCommand : PSCmdlet, IDisposable {
        #region Fields

        private ShowGreetingProxy _proxy;

        #endregion Fields

        #region Constructors

        public ShowGreetingCommand () { }

        #endregion Constructors

        #region IDisposable Members

        public void Dispose () {
            if (_proxy != null)
                _proxy.Dispose();

            _proxy = null;

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members

        #region PSCmdlet Members

        protected override void BeginProcessing () {
            _proxy = new ShowGreetingProxy();
        }

        protected override void ProcessRecord () {
            Trace.Assert(_proxy != null);

            _proxy.ShowWindow();
            _proxy.ActivateWindow();
        }

        protected override void EndProcessing () {
            Trace.Assert(_proxy != null);

            _proxy.WaitWindow();

            Exception exception = _proxy.GetLastException();

            if (exception != null)
                ThrowTerminatingError(
                    new ErrorRecord(
                        exception,
                        "PoshGUIExample.GreetingInvocationException",
                        ErrorCategory.OperationStopped,
                        null
                    )
                );

            if (!_proxy.GetDialogResult())
                return;

            Dictionary<string, string> parameters = _proxy.GetParameters();

            Trace.Assert(parameters.Count > 0);

            InvokeCommand.InvokeScript(
                "param ([System.Collections.IDictionary] $Parameters); Send-Greeting @Parameters",
                false,
                PipelineResultTypes.Output | PipelineResultTypes.Warning | PipelineResultTypes.Error,
                null,
                parameters
            );
        }

        protected override void StopProcessing () {
            Trace.Assert(_proxy != null);

            _proxy.CloseWindow();
        }

        #endregion PSCmdlet Members
    }
}
