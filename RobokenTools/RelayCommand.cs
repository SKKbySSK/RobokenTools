using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace RobokenTools
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        public RelayCommand() { }

        public RelayCommand(Action action)
        {
            Action = o => action();
        }

        public RelayCommand(Action<object> action)
        {
            Action = action;
        }

        public Action<object> Action { get; set; }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Action?.Invoke(parameter);
        }
    }
}
