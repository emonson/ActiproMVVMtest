using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using ActiproSoftware.Windows;
using ActiproMVVMtest.Common;
using ActiproMVVMtest.Models;
using ActiproMVVMtest.Common.ViewModels;

namespace ActiproMVVMtest.ViewModels {

	/// <summary>
	/// Represents the main view-model of the sample.
	/// </summary>
	public class MainViewModel : ViewModelBase {

		private DeferrableObservableCollection<ToolItemViewModel> toolItems;
        private DeferrableObservableCollection<DocumentItemViewModel> documentItems = new DeferrableObservableCollection<DocumentItemViewModel>();
        private DelegateCommand<object> newDocumentCommand;
        private DelegateCommand<object> startSimCommand;
        private DelegateCommand<object> resetSimCommand;

        private SimulationModel simModel;
        private VTKDataModel vtkModel;
        private SimConfigModel simConfigModel;

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="MainViewModel"/> class.
		/// </summary>
		public MainViewModel() {

            simConfigModel = new SimConfigModel();

			// Build tool items
			this.toolItems = new DeferrableObservableCollection<ToolItemViewModel>();

			// Tool 2
            Tool2ViewModel viewModel = new Tool2ViewModel();
			viewModel.DefaultDock = Dock.Bottom;
			viewModel.DockGroup = "BottomGroup";
            viewModel.TextOutput = "0";
			this.toolItems.Add(viewModel);

            // Sim Config
            SimConfigViewModel simConfigViewModel = new SimConfigViewModel(simConfigModel);
            simConfigViewModel.DefaultDock = Dock.Left;
            simConfigViewModel.DockGroup = "LeftGroup";
            this.toolItems.Add(simConfigViewModel);

            // Tool 3
			Tool3ViewModel viewModel3 = new Tool3ViewModel();
			viewModel3.DefaultDock = Dock.Left;
            viewModel3.DockGroup = "HiddenGroup";
			viewModel3.IsInitiallyAutoHidden = true;
			this.toolItems.Add(viewModel3);

            this.simModel = new SimulationModel(simConfigModel);

            // this.AddNewDocument("VTKDocument");
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
        /// Adds new TextDocument to documentItems
        /// </summary>
        public void AddNewDocument(object parameter)
        {
            string doc_type = (string)parameter;
            if (doc_type != null)
            {
                if (doc_type == "TextDocument")
                {
                    this.documentItems.Add(new TextDocumentItemViewModel());
                }
                else if (doc_type == "VTKDocument")
                {
                    // Not creating VTKDataModel unless a window is open,
                    // but I don't know how to get rid of it on last close...
                    if (this.vtkModel == null)
                    {
                        this.vtkModel = new VTKDataModel(this.simModel);
                    }
                    this.documentItems.Add(new VTKDocumentItemViewModel(this.vtkModel));
                }
            }
        }

        public void StartSim(object parameter)
        {
            for (int ii = 0; ii < this.simConfigModel.Duration; ++ii)
            {
                this.simModel.MoveAllCells();
                foreach (ToolItemViewModel vm in this.toolItems)
                {
                    Tool2ViewModel tmp = vm as Tool2ViewModel;
                    if (tmp != null)
                    {
                        tmp.TextOutput = this.simModel.Time.ToString();
                    }
                }
                if (this.vtkModel != null)
                {
                    this.vtkModel.Update();
                    if (ii % this.simConfigModel.VisInterval == 0)
                    {
                        // I think this should be possible through command bindings
                        // and delegate subscriptions...
                        foreach (DocumentItemViewModel vm in this.documentItems)
                        {
                            VTKDocumentItemViewModel tmp = vm as VTKDocumentItemViewModel;
                            if (tmp != null)
                            {
                                tmp.Update();
                            }
                        }
                    }
                }
            }
        }

        public void ResetSim(object parameter)
        {
            this.simModel.ResetCells();
            foreach (ToolItemViewModel vm in this.toolItems)
            {
                Tool2ViewModel tmp = vm as Tool2ViewModel;
                if (tmp != null)
                {
                    tmp.TextOutput = this.simModel.Time.ToString();
                }
            }
            if (this.vtkModel != null)
            {
                this.vtkModel.Update();
                // I think this should be possible through command bindings
                // and delegate subscriptions...
                foreach (DocumentItemViewModel vm in this.documentItems)
                {
                    VTKDocumentItemViewModel tmp = vm as VTKDocumentItemViewModel;
                    if (tmp != null)
                    {
                        tmp.ResetColorMapRange();
                        tmp.Update();
                    }
                }
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////
        // ICOMMANDS
        /////////////////////////////////////////////////////////////////////////////////////////////////////

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

        /// <summary>
        /// Gets the start sim command.
        /// </summary>
        /// <value>The add new document command.</value>
        public ICommand StartSimCommand
        {
            get
            {
                if (this.startSimCommand == null)
                    this.startSimCommand = new DelegateCommand<object>(this.OnStartSimCommandExecuted);
                return this.startSimCommand;
            }
        }

        /// <summary>
        /// Gets the reset sim command.
        /// </summary>
        /// <value>The add new document command.</value>
        public ICommand ResetSimCommand
        {
            get
            {
                if (this.resetSimCommand == null)
                    this.resetSimCommand = new DelegateCommand<object>(this.OnResetSimCommandExecuted);
                return this.resetSimCommand;
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
            this.AddNewDocument(parameter);
        }

        /// <summary>
        /// Occurs when the <see cref="StartSimCommand"/> is executed.
        /// </summary>
        /// <param name="parameter">The associated command parameter; otherwise, <see langword="null"/>.</param>
        private void OnStartSimCommandExecuted(object parameter)
        {
            this.StartSim(parameter);
        }

        /// <summary>
        /// Occurs when the <see cref="ResetSimCommand"/> is executed.
        /// </summary>
        /// <param name="parameter">The associated command parameter; otherwise, <see langword="null"/>.</param>
        private void OnResetSimCommandExecuted(object parameter)
        {
            this.ResetSim(parameter);
        }

    }
}
