// Copyright (c) 2025 Anthony J. Raymond, MIT License

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows;

namespace PoshGUIExample.ShowGreeting {
    public class ShowGreetingProxy : IDisposable {
        #region Fields

        private AutoResetEvent _closed;
        private bool? _dialogResult;
        private Exception _exception;
        private AutoResetEvent _loaded;
        private ShowGreetingViewModel _viewModel;
        private ShowGreetingWindow _window;

        #endregion Fields

        #region Constructors

        public ShowGreetingProxy () { }

        #endregion Constructors

        #region IDisposable Members

        public void Dispose () {
            if (_loaded != null)
                _loaded.Dispose();

            if (_closed != null)
                _closed.Dispose();

            _loaded = null;
            _closed = null;

            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members

        #region Public Methods

        public void ActivateWindow () {
            Trace.Assert(_loaded != null);

            _loaded.WaitOne();

            Trace.Assert(_window != null);

            _window.Dispatcher.Invoke(
                () => _window.Activate()
            );
        }

        public void CloseWindow () {
            Trace.Assert(_window != null);

            _window.Dispatcher.Invoke(
                () => _window.Close()
            );
        }

        public bool GetDialogResult () {
            return _dialogResult ?? false;
        }

        public Exception GetLastException () {
            Exception last = _exception;

            if (last != null)
                _exception = null;

            return last;
        }

        public Dictionary<string, string> GetParameters () {
            return new Dictionary<string, string>(_viewModel.parameters);
        }

        public void ShowWindow () {
            Trace.Assert(_viewModel == null);
            Trace.Assert(_window == null);

            _loaded = new AutoResetEvent(false);
            _closed = new AutoResetEvent(false);

            Thread thread = new Thread(
                () => {
                    try {
                        _viewModel = new ShowGreetingViewModel();
                        _window = new ShowGreetingWindow(_viewModel);

                        _window.Loaded += (object sender, RoutedEventArgs eventArgs) =>
                            _loaded.Set();
                        _window.Closed += (object sender, EventArgs eventArgs) => {
                            _closed.Set();
                            _dialogResult = _window.DialogResult;
                        };

                        _window.ShowDialog();
                    } catch (Exception exception) {
                        if (exception.InnerException != null)
                            _exception = exception.InnerException;
                        else
                            _exception = exception;
                    }
                }
            );
            thread.SetApartmentState(ApartmentState.STA);

            thread.Start();
        }

        public void WaitWindow () {
            Trace.Assert(_closed != null);

            _closed.WaitOne();
        }

        #endregion Public Methods
    }
}
