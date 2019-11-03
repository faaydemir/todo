using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ToDoUICore.MVVMHelper
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> Execute_;
        private readonly Predicate<object> CanExecute_;

        public RelayCommand(Action<object> Execute, Predicate<object> CanExecute = null)
        {
            if (Execute == null)
                throw new ArgumentNullException("No action to execute for this command.");

            Execute_ = Execute;
            CanExecute_ = CanExecute;
        }

        public RelayCommand(Func<object, Task<bool>> Execute, Predicate<object> CanExecute = null)
        {
            if (Execute == null)
                throw new ArgumentNullException("No action to execute for this command.");

            Execute_ = async (value) =>
            {
                await Execute(value);
            };

            CanExecute_ = CanExecute;
        }

        public bool CanExecute(object parameter)
        {
            return (CanExecute_ == null) ? true : CanExecute_(parameter);
        }

        public event EventHandler CanExecuteChanged;

        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        public void Execute(object parameter)
        {
            Execute_(parameter);
        }
    }

    //TODO generate builder
    public class GenericRelayCommand<T> : ICommand where T : class
    {
        private readonly Action<T> Execute_;
        private readonly Predicate<T> CanExecute_;

        //public event EventHandler CanExecuteChanged
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        public GenericRelayCommand(Action<T> Execute, Predicate<T> CanExecute = null)
        {
            if (Execute == null)
                throw new ArgumentNullException("No action to execute for this command.");

            Execute_ = Execute;
            CanExecute_ = CanExecute;
        }

        public GenericRelayCommand(Func<T, Task<bool>> Execute, Predicate<T> CanExecute = null)
        {
            if (Execute == null)
                throw new ArgumentNullException("No action to execute for this command.");

            Execute_ = async (value) =>
            {
                await Execute(value as T);
            };

            CanExecute_ = CanExecute;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            var p = parameter as T;
            if (p == null)
                return false;

            return true;
        }

        public void Execute(object parameter)
        {
            var p = parameter as T;
            Execute_(p);
        }
    }
}