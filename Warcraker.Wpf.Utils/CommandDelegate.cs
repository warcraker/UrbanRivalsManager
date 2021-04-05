using System;
using System.Windows.Input;
using Warcraker.Utils;

namespace Warcraker.Wpf.Utils
{
    public class CommandDelegate : ICommand
    {
        private static readonly Func<object, bool> RETURN_TRUE = new Func<object, bool>((x) => true);

        public event EventHandler CanExecuteChanged;

        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        private CommandDelegate(Action<object> execute, Func<object, bool> canExecute)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public static CommandDelegate CreateAlwaysEnabledcommand(Action<object> execute)
        {
            AssertArgument.CheckIsNotNull(execute, nameof(execute));

            return new CommandDelegate(execute, RETURN_TRUE);
        }
        public static CommandDelegate CreateCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            AssertArgument.CheckIsNotNull(execute, nameof(execute));
            AssertArgument.CheckIsNotNull(canExecute, nameof(canExecute));

            return new CommandDelegate(execute, canExecute);
        }

        public bool CanExecute(object parameter)
        {
            return canExecute.Invoke(parameter);
        }
        public void Execute(object parameter)
        {
            execute.Invoke(parameter);
        }
    }
}
