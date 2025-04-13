// Copyright (c) 2025 Anthony J. Raymond, MIT License (see manifest for details)

using System.Windows;
using System.Collections.Concurrent;

namespace Example {
    public partial class ShowPublicFunctionWindow : Window {
        #region Properties

        private ConcurrentDictionary<string, string> _parameters;

        #endregion Properties

        #region Constructors

        public ShowPublicFunctionWindow (ConcurrentDictionary<string, string> parameters) {
            InitializeComponent();

            _parameters = parameters;
        }

        #endregion Constructors

        #region Private Methods

        private void Button_Click (object sender, RoutedEventArgs e) {
            _parameters["InputObject"] = tbInput.Text;

            Close();
        }

        #endregion Private Methods
    }
}
