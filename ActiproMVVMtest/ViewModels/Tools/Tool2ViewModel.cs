using System;
using System.Windows.Media.Imaging;
using ActiproMVVMtest.Common.ViewModels;

namespace ActiproMVVMtest.ViewModels
{

	/// <summary>
	/// Represents a tool view-model for the sample.
	/// </summary>
	public class Tool2ViewModel : ToolItemViewModel {

        private string textOutput;
        
        /// <summary>
		/// Initializes a new instance of the <see cref="Tool2ViewModel"/> class.
		/// </summary>
		public Tool2ViewModel() {
			this.ImageSource = new BitmapImage(new Uri("/Resources/Images/Toolbox16.png", UriKind.Relative));
			this.Name = "toolWindow2";
			this.Title = "Output Window";
            this.textOutput = "";
		}

        public string TextOutput
        {
            get { return textOutput; }
            set
            {
                if (value == textOutput)
                    return;
                textOutput = value;
                NotifyPropertyChanged("TextOutput");
            }
        }
	}
}
