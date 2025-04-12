// Copyright (c) 2025 Anthony J. Raymond, MIT License (see manifest for details)

using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

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
            if (string.IsNullOrEmpty(parameters["InputObject"])) {
                return;
            }

            StringBuilder sb = new StringBuilder();

            foreach (KeyValuePair<string, string> kvp in parameters) {
                sb.Append($" -{kvp.Key} '{kvp.Value}'");
            }

            InvokeCommand.InvokeScript(
                $"Get-PublicFunction{sb.ToString()}",
                false,
                PipelineResultTypes.Output | PipelineResultTypes.Warning | PipelineResultTypes.Error,
                null,
                null
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
