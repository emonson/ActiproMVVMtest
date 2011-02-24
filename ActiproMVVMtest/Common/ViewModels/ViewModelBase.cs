using System;
using ActiproSoftware.Windows;

namespace ActiproMVVMtest.Common.ViewModels
{

	/// <summary>
	/// Represents a base class for all view-models.
	/// </summary>
	public abstract class ViewModelBase : ObservableObjectBase, IDisposable {

		private string name;

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Allows an object to attempt to free resources and perform other cleanup operations before the 
		/// object is reclaimed by garbage collection.
		/// </summary>
		~ViewModelBase() {
			// Dispose
			this.Dispose(false);
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Releases all resources used by the object.
		/// </summary>
		public void Dispose() {
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		/// <summary>
		/// Releases the unmanaged resources used by the object and optionally releases the managed resources.
		/// </summary>
		/// <param name="disposing">
		/// <c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources. 
		/// </param>
		/// <remarks>
		/// This method is called by the public <c>Dispose</c> method and the <c>Finalize</c> method. 
		/// <c>Dispose</c> invokes this method with the <paramref name="disposing"/> parameter set to <c>true</c>. 
		/// <c>Finalize</c> invokes this method with <paramref name="disposing"/> set to <c>false</c>.
		/// </remarks>
		protected virtual void Dispose(bool disposing) { }

		/// <summary>
		/// Gets or sets the name of the view-model.
		/// </summary>
		/// <value>The name of the view-model.</value>
		public string Name {
			get {
				return this.name;
			}
			set {
				if (this.name != value) {
					this.name = value;
					this.NotifyPropertyChanged("Name");
				}
			}
		}

	}
}
