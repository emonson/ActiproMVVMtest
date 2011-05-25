using System.Windows;
using ActiproMVVMtest.Common;

namespace ActiproMVVMtest.Views {

	/// <summary>
	/// Provides the main view-model user control for this sample.
	/// </summary>
	public partial class MainView : System.Windows.Controls.UserControl {

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes an instance of the <c>MainView</c> class.
		/// </summary>
		public MainView() {
			InitializeComponent();
		}

        /// <summary>
        /// Occurs when the <c>File.Exit</c> menu item is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">A <see cref="RoutedEventArgs"/> that contains the event data.</param>
        private void OnFileExitMenuItemClick(object sender, RoutedEventArgs e)
        {
            // Show a message
            MessageBox.Show("Close the application here.");
        }

        // Example of rerouting MainView event as a MainViewModel command
        //private void MainDockSite_WindowActivated(object sender, ActiproSoftware.Windows.Controls.Docking.DockingWindowEventArgs e)
        //{
        //    var vm = this.DataContext as ActiproMVVMtest.ViewModels.MainViewModel;
        //    vm.ResetSimCommand.Execute(null);
        //}

    }
}