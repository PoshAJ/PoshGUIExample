// Copyright (c) 2024 .NET Foundation and Contributors, MIT License
// https://github.com/CommunityToolkit/dotnet/blob/main/src/CommunityToolkit.Mvvm/Input/RelayCommand.cs

using System;
using System.Windows.Input;

namespace PoshGUIExample {
    public class RelayCommand : ICommand {
        #region Fields

        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        #endregion Fields

        #region Constructors

        public RelayCommand (Action execute) : this(execute, null) { }

        public RelayCommand (Action execute, Func<bool> canExecute) {
            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion Constructors

        #region ICommand Members

        public bool CanExecute (object parameter) {
            return _canExecute == null || _canExecute.Invoke() != false;
        }

        public void Execute (object parameter) {
            _execute.Invoke();
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        #endregion ICommand Members
    }
}
