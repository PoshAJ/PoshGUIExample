// Copyright (c) 2025 Anthony J. Raymond, MIT License

using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Input;
using System.Diagnostics;

namespace PoshGUIExample.ShowGreeting {
    public class ShowGreetingViewModel : INotifyPropertyChanged {
        #region Fields

        private readonly string[] _mandatoryParameters;
        private bool _mandatoryParametersHaveValues;

        #endregion Fields

        #region Properties

        public bool mandatoryParametersHaveValues {
            get => _mandatoryParametersHaveValues;
            set {
                if (_mandatoryParametersHaveValues != value) {
                    _mandatoryParametersHaveValues = value;
                    OnPropertyChanged("mandatoryParametersHaveValues");
                }
            }
        }

        public ObservableDictionary<string, string> parameters { get; }

        public ICommand ResetCommand { get; }

        #endregion Properties

        #region Constructors

        public ShowGreetingViewModel () {
            _mandatoryParameters = new[] { "InputObject" };

            parameters = new ObservableDictionary<string, string>();

            parameters.CollectionChanged += (object sender, NotifyCollectionChangedEventArgs eventArgs) =>
                EvaluateMandatoryParametersHaveValues();

            ResetCommand = new RelayCommand(Reset);
        }

        #endregion Constructors

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members

        #region Private Methods

        private void EvaluateMandatoryParametersHaveValues () {
            Trace.Assert(_mandatoryParameters.Length > 0);

            bool parametersHaveValues = true;

            foreach (string parameter in _mandatoryParameters) {
                if (
                    !parameters.TryGetValue(parameter, out string value)
                    || string.IsNullOrEmpty(value)
                ) {
                    parametersHaveValues = false;

                    break;
                }
            }

            mandatoryParametersHaveValues = parametersHaveValues;
        }

        private void Reset () {
            Trace.Assert(parameters != null);

            parameters.Clear();
            OnPropertyChanged("parameters");
        }

        #endregion Private Methods

        #region Event Handlers

        private void OnPropertyChanged (string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Event Handlers
    }
}
