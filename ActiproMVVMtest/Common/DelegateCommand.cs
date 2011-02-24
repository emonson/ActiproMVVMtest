using System;
using System.Windows.Input;

namespace ActiproMVVMtest.Common
{

	/// <summary>
	/// Represents a command that forwards the <c>Execute</c> and <c>CanExecute</c> calls to specified delegates.
	/// </summary>
	public class DelegateCommand<T> : ICommand {

		private readonly Action<T>		executeCallback;
		private readonly Predicate<T>	canExecuteCallback;

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateCommand&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="executeCallback">The execute callback delegate.</param>
		public DelegateCommand(Action<T> executeCallback)
			: this(executeCallback, null) {
			// No-op
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DelegateCommand&lt;T&gt;"/> class.
		/// </summary>
		/// <param name="executeCallback">The execute callback delegate.</param>
		/// <param name="canExecuteCallback">The can execute callback delegate.</param>
		public DelegateCommand(Action<T> executeCallback, Predicate<T> canExecuteCallback) {
			if (executeCallback == null)
				throw new ArgumentNullException("executeCallback");

			this.executeCallback = executeCallback;
			this.canExecuteCallback = canExecuteCallback;
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// INTERFACE IMPLEMENTATION
		/////////////////////////////////////////////////////////////////////////////////////////////////////
		
		#region ICommand Members

		/// <summary>
		/// Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null"/>.</param>
		/// <returns>
		/// <c>true</c> if this command can be executed; otherwise, <c>false</c>.
		/// </returns>
		public bool CanExecute(object parameter) {
			return (this.canExecuteCallback == null) ? true : this.canExecuteCallback((T)parameter);
		}

		/// <summary>
		/// Occurs when changes occur that affect whether or not the command should execute.
		/// </summary>
		public event EventHandler CanExecuteChanged {
			add {
				if (this.canExecuteCallback != null)
					CommandManager.RequerySuggested += value;
			}
			remove {
				if (this.canExecuteCallback != null)
					CommandManager.RequerySuggested -= value;
			}
		}

		/// <summary>
		/// Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">Data used by the command.  If the command does not require data to be passed, this object can be set to <see langword="null"/>.</param>
		public void Execute(object parameter) {
			this.executeCallback((T)parameter);
		}

		#endregion // ICommand Members

	}
}