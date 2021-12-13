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
        // Method that provides logic for this command
        private Action<object> execute;

        // Method that decides if this command can be executed
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

        /// <summary>
        /// Can this command be executed.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool CanExecute(object parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        /// <summary>
        /// Execute the logic of this command.
        /// </summary>
        /// <param name="parameter">Parameter for this command</param>
        public void Execute(object parameter)
        {
            execute(parameter);
        }
    }
}