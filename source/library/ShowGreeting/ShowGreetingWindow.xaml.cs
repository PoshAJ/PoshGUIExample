// Copyright (c) 2025 Anthony J. Raymond, MIT License

using System.Windows;

namespace PoshGUIExample.ShowGreeting {
    public partial class ShowGreetingWindow : Window {
        #region Constructors

        public ShowGreetingWindow (ShowGreetingViewModel viewModel) {
            InitializeComponent();

            DataContext = viewModel;
        }

        #endregion Constructors

        #region Event Handlers

        private void OnCancel (object sender, RoutedEventArgs eventArgs) {
            DialogResult = false;
        }

        private void OnDefault (object sender, RoutedEventArgs eventArgs) {
            DialogResult = true;
        }

        #endregion Event Handlers
    }
}
