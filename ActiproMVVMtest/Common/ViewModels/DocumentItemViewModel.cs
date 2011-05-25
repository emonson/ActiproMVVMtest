using System.Windows.Media;
using ActiproSoftware.Windows;

namespace ActiproMVVMtest.Common.ViewModels
{

	/// <summary>
	/// Represents a view-model for document items.
	/// </summary>
	public class DocumentItemViewModel : DockingItemViewModelBase {

		private bool isReadOnly;
		private string fileName;
        public DeferrableObservableCollection<string> DisplayList = new DeferrableObservableCollection<string>();

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Gets or sets the file name associated with the view-model.
		/// </summary>
		/// <value>The file name associated with the view-model.</value>
		public string FileName {
			get {
				return this.fileName;
			}
			set {
				if (this.fileName != value) {
					this.fileName = value;
					this.NotifyPropertyChanged("FileName");
				}
			}
		}

		/// <summary>
		/// Gets or sets the read-only state of the associated with the view-model.
		/// </summary>
		/// <value>The read-only state of the associated with the view-model.</value>
		public bool IsReadOnly {
			get {
				return this.isReadOnly;
			}
			set {
				if (this.isReadOnly != value) {
					this.isReadOnly = value;
					this.NotifyPropertyChanged("IsReadOnly");
				}
			}
		}

	}
}
