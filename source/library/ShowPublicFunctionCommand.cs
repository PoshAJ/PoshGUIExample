// Copyright (c) 2025 Anthony J. Raymond, MIT License (see manifest for details)

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace Example {
    [Cmdlet(VerbsCommon.Show, "PublicFunction")]
    [OutputType(typeof(void))]
    public class ShowPublicFunctionCommand : PSCmdlet, IDisposable {
        #region Properties

        private ShowPublicFunctionProxy _proxy;

        #endregion Properties

        #region Constructors

        public ShowPublicFunctionCommand () { }

        #endregion Constructors

        #region PSCmdlet Methods

        protected override void BeginProcessing () {
            _proxy = new ShowPublicFunctionProxy();
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
                        "PublicFunctionInvocationException",
                        ErrorCategory.OperationStopped,
                        null
                    )
                );
            }

            Dictionary<string, string> parameters = _proxy.GetParameters();

            // check mandatory parameter
            if (!parameters.TryGetValue("InputObject", out string InputObject) || string.IsNullOrEmpty(InputObject)) {
                return;
            }

            InvokeCommand.InvokeScript(
                "param ([System.Collections.IDictionary] $Parameters); Get-PublicFunction @Parameters",
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
