using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using ActiproSoftware.Windows;
using ActiproMVVMtest.Common;
using ActiproMVVMtest.Common.ViewModels;

namespace ActiproMVVMtest.ViewModels {

	/// <summary>
	/// Represents the main view-model of the sample.
	/// </summary>
	public class MainViewModel : ViewModelBase {

		private DeferrableObservableCollection<ToolItemViewModel> toolItems;
        private DeferrableObservableCollection<DocumentItemViewModel> documentItems = new DeferrableObservableCollection<DocumentItemViewModel>();
        private DelegateCommand<object> newDocumentCommand;

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

            this.AddNewDocument();
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

        /// <summary>
        /// Gets the document items associated with this view-model.
        /// </summary>
        /// <value>The document items associated with this view-model.</value>
        public IList<DocumentItemViewModel> DocumentItems
        {
            get
            {
                return this.documentItems;
            }
        }

        /// <summary>
        /// Attempts to close this view-model.
        /// </summary>
        public void AddNewDocument()
        {
            this.documentItems.Add(new TextDocumentItemViewModel());
        }

        /// <summary>
        /// Gets the add new document command.
        /// </summary>
        /// <value>The add new document command.</value>
        public ICommand NewDocumentCommand
        {
            get
            {
                if (this.newDocumentCommand == null)
                    this.newDocumentCommand = new DelegateCommand<object>(this.OnNewDocumentCommandExecuted);
                return this.newDocumentCommand;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // NON-PUBLIC PROCEDURES
        /////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>
        /// Occurs when the <see cref="NewDocumentCommand"/> is executed.
        /// </summary>
        /// <param name="parameter">The associated command parameter; otherwise, <see langword="null"/>.</param>
        private void OnNewDocumentCommandExecuted(object parameter)
        {
            this.AddNewDocument();
        }

    }
}
