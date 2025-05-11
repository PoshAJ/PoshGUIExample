// Copyright (c) 2024 .NET Foundation and Contributors, MIT License
// https://github.com/CommunityToolkit/dotnet/blob/main/src/CommunityToolkit.Mvvm/Input/RelayCommand{T}.cs

using System;
using System.Windows.Input;

namespace PoshGUIExample {
    public class RelayCommand<T> : ICommand {
        #region Fields

        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        #endregion Fields

        #region Constructors

        public RelayCommand (Action<T> execute) : this(execute, null) { }

        public RelayCommand (Action<T> execute, Predicate<T> canExecute) {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion Constructors

        #region ICommand Members

        public bool CanExecute (object parameter) {
            return _canExecute == null || _canExecute.Invoke((T) parameter) != false;
        }

        public void Execute (object parameter) {
            _execute.Invoke((T) parameter);
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        #endregion ICommand Members
    }
}
