using System;
using System.Windows.Input;

namespace SalesMaster.Utils
{
    public class RelayCommand : ICommand
    {
        public readonly Action<object> ExecuteAction;

        public readonly Predicate<object> CanExecutePre;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute = null)
        {
            ExecuteAction = execute;
            CanExecutePre = canExecute;
        }

        public event EventHandler CanExecuteChanged
        { 
            add { CommandManager.RequerySuggested += value; } 
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            if (CanExecutePre == null)
                return true;
            return CanExecutePre(parameter);
        }

        public void Execute(object parameter) => ExecuteAction(parameter);
    }
}
