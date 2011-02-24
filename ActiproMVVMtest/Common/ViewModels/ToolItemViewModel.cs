using System.Windows.Media;
using System.Windows.Controls;

namespace ActiproMVVMtest.Common.ViewModels
{

	/// <summary>
	/// Represents a view-model for tool items.
	/// </summary>
	public class ToolItemViewModel : DockingItemViewModelBase {

		private Dock defaultDock;
		private string dockGroup;
		private bool isInitiallyAutoHidden;

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Gets or sets the default dock side of the window.
		/// </summary>
		/// <value>The default dock side of the window.</value>
		public Dock DefaultDock {
			get {
				return this.defaultDock;
			}
			set {
				if (this.defaultDock != value) {
					this.defaultDock = value;
					this.NotifyPropertyChanged("DefaultDock");
				}
			}
		}

		/// <summary>
		/// Gets or sets the dock group associated with the window.
		/// </summary>
		/// <value>The dock group associated with the window.</value>
		public string DockGroup {
			get {
				return this.dockGroup;
			}
			set {
				if (this.dockGroup != value) {
					this.dockGroup = value;
					this.NotifyPropertyChanged("DockGroup");
				}
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the window should be auto-hidden.
		/// </summary>
		/// <value><c>true</c> if the window should be auto-hidden; otherwise, <c>false</c>.</value>
		public bool IsInitiallyAutoHidden {
			get {
				return this.isInitiallyAutoHidden;
			}
			set {
				if (this.isInitiallyAutoHidden != value) {
					this.isInitiallyAutoHidden = value;
					this.NotifyPropertyChanged("IsInitiallyAutoHidden");
				}
			}
		}

	}
}
