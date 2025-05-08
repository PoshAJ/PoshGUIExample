// Copyright (c) 2025 Anthony J. Raymond, MIT License (see manifest for details)

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace PoshGUIExample.ShowGreeting {
    [Cmdlet(VerbsCommon.Show, "Greeting")]
    [OutputType(typeof(void))]
    public class ShowGreetingCommand : PSCmdlet, IDisposable {
        #region Properties

        private ShowGreetingProxy _proxy;

        #endregion Properties

        #region Constructors

        public ShowGreetingCommand () { }

        #endregion Constructors

        #region PSCmdlet Methods

        protected override void BeginProcessing () {
            _proxy = new ShowGreetingProxy();
        }

        protected override void ProcessRecord () {
            _proxy.ShowWindow();
            _proxy.ActivateWindow();
        }

        protected override void EndProcessing () {
            _proxy.WaitWindow();

            Exception exception = _proxy.GetLastException();

            if (exception != null) {
                ThrowTerminatingError(
                    new ErrorRecord(
                        exception,
                        "PoshGUIExample.GreetingInvocationException",
                        ErrorCategory.OperationStopped,
                        null
                    )
                );
            }

            Dictionary<string, string> parameters = _proxy.GetParameters();

            if (!parameters.TryGetValue("InputObject", out string inputObject) || string.IsNullOrEmpty(inputObject)) {
                WriteError(
                    new ErrorRecord(
                        new GreetingArgumentException("Cannot bind argument to parameter 'InputObject' because it is an empty string."),
                        "PoshGUIExample.GreetingArgumentException",
                        ErrorCategory.InvalidArgument,
                        null
                    )
                );

                return;
            }

            InvokeCommand.InvokeScript(
                "param ([System.Collections.IDictionary] $Parameters); Send-Greeting @Parameters",
                false,
                PipelineResultTypes.Output | PipelineResultTypes.Warning | PipelineResultTypes.Error,
                null,
                parameters
            );
        }

        protected override void StopProcessing () {
            _proxy.CloseWindow();
        }

        #endregion PSCmdlet Methods

        #region Public Methods

        public void Dispose () {
            _proxy?.Dispose();
            _proxy = null;

            GC.SuppressFinalize(this);
        }

        #endregion Public Methods
    }
}
