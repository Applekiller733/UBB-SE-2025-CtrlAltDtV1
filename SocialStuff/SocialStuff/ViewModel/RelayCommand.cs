namespace SocialStuff.ViewModel
{
    using System;
    using System.Windows.Input;

    /// <summary>
    /// Command for actions that require a parameter.
    /// </summary>
    /// <typeparam name="T">The type of the parameter.</typeparam>
    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Predicate<T> _canExecute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        public RelayCommand(Action<T> execute)
            : this(execute, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand{T}"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        /// <param name="canExecute">The predicate to determine if the action can execute.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this._canExecute = canExecute;
        }

        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Determines whether the command can execute.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>True if the command can execute; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            return this._canExecute == null || this._canExecute((T)parameter);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void Execute(object? parameter)
        {
            this._execute((T)parameter);
        }
    }

    /// <summary>
    /// Command for actions that don't require a parameter.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action _execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand"/> class.
        /// </summary>
        /// <param name="execute">The action to execute.</param>
        public RelayCommand(Action execute)
        {
            this._execute = execute ?? throw new ArgumentNullException(nameof(execute));
        }

        public event EventHandler? CanExecuteChanged;

        /// <summary>
        /// Determines whether the command can execute.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns>True if the command can execute; otherwise, false.</returns>
        public bool CanExecute(object? parameter)
        {
            return true; // You can implement logic to disable the command if necessary
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        public void Execute(object? parameter)
        {
            this._execute();
        }

        /// <summary>
        /// Raises the CanExecuteChanged event.
        /// </summary>
        public void RaiseCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
