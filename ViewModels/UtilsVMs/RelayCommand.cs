using System;
using System.Windows.Input;

namespace TienIchLich.ViewModels
{
    /// <summary>
    /// Relay command for command binding.
    /// Reference: https://www.c-sharpcorner.com/UploadFile/20c06b/icommand-and-relaycommand-in-wpf/
    /// </summary>
    public class RelayCommand : ICommand
    {
        public event EventHandler canExecuteChanged;
        private Action<object> execute;
        private Predicate<object> canExecute;

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecute == null || this.canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            this.execute(parameter);
        }
    }
}
