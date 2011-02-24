using System.Windows.Media;

namespace ActiproMVVMtest.Common.ViewModels
{

	/// <summary>
	/// Represents a base class for all docking item view-models.
	/// </summary>
	public abstract class DockingItemViewModelBase : ViewModelBase {

		private string description;
		private ImageSource imageSource;
		private string title;

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Gets or sets the description associated with the view-model.
		/// </summary>
		/// <value>The description associated with the view-model.</value>
		public string Description {
			get {
				return this.description;
			}
			set {
				if (this.description != value) {
					this.description = value;
					this.NotifyPropertyChanged("Description");
				}
			}
		}

		/// <summary>
		/// Gets or sets the image associated with the view-model.
		/// </summary>
		/// <value>The image associated with the view-model.</value>
		public ImageSource ImageSource {
			get {
				return this.imageSource;
			}
			set {
				if (this.imageSource != value) {
					this.imageSource = value;
					this.NotifyPropertyChanged("ImageSource");
				}
			}
		}

		/// <summary>
		/// Gets or sets the title associated with the view-model.
		/// </summary>
		/// <value>The title associated with the view-model.</value>
		public string Title {
			get {
				return this.title;
			}
			set {
				if (this.title != value) {
					this.title = value;
					this.NotifyPropertyChanged("Title");
				}
			}
		}

	}
}
