// Copyright (c) 2025 Anthony J. Raymond, MIT License (see manifest for details)

using System.Windows;
using System.Collections.Concurrent;

namespace PoshGUIExample.ShowGreeting {
    public partial class ShowGreetingWindow : Window {
        #region Properties

        private ConcurrentDictionary<string, string> _parameters;

        #endregion Properties

        #region Constructors

        public ShowGreetingWindow (ConcurrentDictionary<string, string> parameters) {
            InitializeComponent();

            _parameters = parameters;
        }

        #endregion Constructors

        #region Private Methods

        private void Button_Reset (object sender, RoutedEventArgs e) {
            tbInput.Clear();
        }

        private void Button_Submit (object sender, RoutedEventArgs e) {
            _parameters["InputObject"] = tbInput.Text;

            Close();
        }

        #endregion Private Methods
    }
}
