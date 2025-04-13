// Copyright (c) 2025 Anthony J. Raymond, MIT License (see manifest for details)

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Windows;

namespace Example {
    public class ShowPublicFunctionProxy : IDisposable {
        #region Properties

        private ConcurrentDictionary<string, string> _parameters;
        private AutoResetEvent _loaded;
        private AutoResetEvent _closed;
        private ShowPublicFunctionWindow _window;
        private Exception _exception;

        #endregion Properties

        #region Constructors

        public ShowPublicFunctionProxy () {
            _parameters = new ConcurrentDictionary<string, string>();
        }

        #endregion Constructors

        #region Public Methods

        public void ShowWindow () {
            if (_window == null) {
                _loaded = new AutoResetEvent(false);
                _closed = new AutoResetEvent(false);

                Thread thread = new Thread(
                    new ThreadStart(
                        delegate {
                            try {
                                _window = new ShowPublicFunctionWindow(_parameters);

                                _window.Loaded += (object sender, RoutedEventArgs e) => { _loaded.Set(); };
                                _window.Closed += (object sender, EventArgs e) => { _closed.Set(); };

                                _window.ShowDialog();
                            } catch (Exception e) {
                                if (e.InnerException != null) {
                                    _exception = e.InnerException;
                                } else {
                                    _exception = e;
                                }
                            }
                        }
                    )
                );
                thread.SetApartmentState(ApartmentState.STA);

                thread.Start();
            }
        }

        public void ActivateWindow () {
            _loaded?.WaitOne();

            _window?.Dispatcher.Invoke(
                new ThreadStart(
                    delegate {
                        _window.Activate();
                    }
                )
            );
        }

        public void WaitWindow () => _closed?.WaitOne();

        public void CloseWindow () {
            _window?.Dispatcher.Invoke(
                new ThreadStart(
                    delegate {
                        _window.Close();
                    }
                )
            );
        }

        public Exception GetLastException () {
            Exception last = _exception;

            if (last == null) {
                return _exception;
            }

            _exception = null;

            return last;
        }

        public Dictionary<string, string> GetParameters () => new Dictionary<string, string>(_parameters);

        public void Dispose () {
            _loaded?.Dispose();
            _closed?.Dispose();

            _loaded = null;
            _closed = null;

            GC.SuppressFinalize(this);
        }

        #endregion Public Methods
    }
}
