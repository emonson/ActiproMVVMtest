using System.Collections.Generic;
using System.Windows.Controls;
using ActiproSoftware.Windows;
using ActiproMVVMtest.Common.ViewModels;

namespace ActiproMVVMtest.ViewModels {

	/// <summary>
	/// Represents the main view-model of the sample.
	/// </summary>
	public class MainViewModel : ViewModelBase {

		private DeferrableObservableCollection<ToolItemViewModel> toolItems;

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="MainViewModel"/> class.
		/// </summary>
		public MainViewModel() {

			// Build tool items
			this.toolItems = new DeferrableObservableCollection<ToolItemViewModel>();

			// Tool 1
			ToolItemViewModel viewModel = new Tool1ViewModel();
			viewModel.DefaultDock = Dock.Right;
			viewModel.DockGroup = "Group1";
			this.toolItems.Add(viewModel);

			// Tool 2
			viewModel = new Tool2ViewModel();
			viewModel.DefaultDock = Dock.Right;
			viewModel.DockGroup = "Group1";
			this.toolItems.Add(viewModel);

			// Tool 3
			viewModel = new Tool3ViewModel();
			viewModel.DefaultDock = Dock.Left;
			viewModel.IsInitiallyAutoHidden = true;
			this.toolItems.Add(viewModel);
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Gets the tool items associated with this view-model.
		/// </summary>
		/// <value>The tool items associated with this view-model.</value>
		public IList<ToolItemViewModel> ToolItems {
			get {
				return this.toolItems;
			}
		}

	}
}
