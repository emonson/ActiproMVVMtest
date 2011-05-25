using System;
using System.Windows.Media.Imaging;
using ActiproSoftware.Windows;
using ActiproMVVMtest.Common.ViewModels;

namespace ActiproMVVMtest.ViewModels
{

	/// <summary>
	/// Represents a tool view-model for the sample.
	/// </summary>
	public class Tool3ViewModel : ToolItemViewModel {

        private DeferrableObservableCollection<string> _remoteOptions;
        public DeferrableObservableCollection<string> RemoteDocDisplayOptions 
        {
            get { return _remoteOptions; }
            set
            {
                if (_remoteOptions == value)
                {
                    return;
                }
                else
                {
                    _remoteOptions = value;
                    this.NotifyPropertyChanged("RemoteDocDisplayOptions");
                }
            }
        }
        public DeferrableObservableCollection<string> DefaultOptions { get; set; }

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="Tool3ViewModel"/> class.
		/// </summary>
		public Tool3ViewModel() {
			this.ImageSource = new BitmapImage(new Uri("/Resources/Images/Toolbox16.png", UriKind.Relative));
			this.Name = "toolWindow3";
			this.Title = "Tool 3";
            DefaultOptions = new DeferrableObservableCollection<string>();
            DefaultOptions.Add("no options");
            RemoteDocDisplayOptions = DefaultOptions;
		}
	}
}
