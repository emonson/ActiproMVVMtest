using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ActiproSoftware.Windows;
using ActiproMVVMtest.Common;
using ActiproMVVMtest.Models;
using ActiproMVVMtest.Common.ViewModels;
using ActiproSoftware.Windows.Controls.Docking;

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

            // Tool 3
            Tool3ViewModel viewModel3 = new Tool3ViewModel();
            viewModel3.Name = "DisplayOptions";
            viewModel3.Title = "Doc Display Options";
            viewModel3.DefaultDock = Dock.Right;
            viewModel3.DockGroup = "RightGroup";
            viewModel3.IsInitiallyAutoHidden = false;
            this.toolItems.Add(viewModel3);

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
            //Tool3ViewModel viewModel3 = new Tool3ViewModel();
            //viewModel3.DefaultDock = Dock.Left;
            //viewModel3.DockGroup = "HiddenGroup";
            //viewModel3.IsInitiallyAutoHidden = true;
            //this.toolItems.Add(viewModel3);

            this.simModel = new SimulationModel(simConfigModel);
            
            this.AddNewDocument("VTKDocument");

            this.AddNewDocument("TextDocument");
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
        /// Adds new TextDocument to documentItems.
        /// Only accessible internally or through command binding to NewDocumentCommand
        /// </summary>
        /// <param name="parameter">Sets type of document ("TextDocument" or "VTKDocument").</param>
        private void AddNewDocument(object parameter)
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
                    VTKDocumentItemViewModel vtk_mod = new VTKDocumentItemViewModel(this.vtkModel);
                    // TODO: This should be done through messaging, not property changed events...
                    // TODO: This type of thing probably leaks memory on VTK doc window closure...
                    vtk_mod.PropertyChanged += this.VTKModelPropertyChanged;
                    this.documentItems.Add(vtk_mod);
                    // MessageBoxResult tmp = MessageBox.Show("Added a VTK view.");
                }
            }
        }

        /// <summary>
        /// Starts a simulation run.
        /// Only accessible internally or through command binding to StartSimCommand
        /// </summary>
        /// <param name="parameter">Not used right now.</param>
        private void StartSim(object parameter)
        {
            for (int ii = 0; ii < this.simConfigModel.Duration; ++ii)
            {
                this.simModel.MoveAllCells();
                // TODO: Should only have one instance of this output window rather than loop...
                foreach (ToolItemViewModel vm in this.toolItems)
                {
                    Tool2ViewModel tmp = vm as Tool2ViewModel;
                    if (tmp != null)
                    {
                        tmp.TextOutput = "Sim Time: " + this.simModel.Time.ToString();
                    }
                }
                if (this.vtkModel != null)
                {
                    this.vtkModel.Update();
                    if (ii % this.simConfigModel.VisInterval == 0)
                    {
                        // I think this should be possible through command bindings
                        // and delegate subscriptions... or mediator pattern...
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

        /// <summary>
        /// Reset simulation to initial state.
        /// Only accessible internally or through command binding to ResetSimCommand
        /// </summary>
        /// <param name="parameter">Not used right now.</param>
        private void ResetSim(object parameter)
        {
            this.simModel.ResetCells();
            // TODO: Should only have one instance of this output window rather than loop...
            foreach (ToolItemViewModel vm in this.toolItems)
            {
                Tool2ViewModel tmp = vm as Tool2ViewModel;
                if (tmp != null)
                {
                    tmp.TextOutput = "Sim Time: " + this.simModel.Time.ToString();
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

        private void VTKModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            VTKDocumentItemViewModel vtk_mod = sender as VTKDocumentItemViewModel;

            if (vtk_mod != null)
            {
                if (e.PropertyName == "SelectedCell")
                {
                    // TODO: Should only have one instance of this output window rather than loop...
                    foreach (ToolItemViewModel vm in this.toolItems)
                    {
                        Tool2ViewModel tmp = vm as Tool2ViewModel;
                        if (tmp != null)
                        {
                            tmp.TextOutput = "Selected Cell: " + vtk_mod.SelectedCell.ToString();
                        }
                    }
                }
            }
            return;
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
                    this.newDocumentCommand = new DelegateCommand<object>(this.AddNewDocument);
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
                    this.startSimCommand = new DelegateCommand<object>(this.StartSim);
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
                    this.resetSimCommand = new DelegateCommand<object>(this.ResetSim);
                return this.resetSimCommand;
            }
        }

        /// <summary>
        /// Handles the <c>WindowActivated</c> event of the <c>DockSite</c> control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="DockingWindowEventArgs"/> instance containing the event data.</param>
        public static void OnDockSiteWindowActivated(object sender, DockingWindowEventArgs e)
        {
            DockSite dockSite = sender as DockSite;
            if (dockSite == null)
                return;

            // Ensure the DockingWindow exists and is generated for an item
            DockingWindow dockingWindow = e.Window;
            if (dockingWindow == null || !dockingWindow.IsContainerForItem)
                return;

            // TODO: Later should have specific single case of display options tool window
            // which is either visible or not rather than looping and checking...
            ToolWindowCollection toolWindows = dockSite.ToolWindows;
            foreach (ToolWindow tw in toolWindows)
            {
                Tool3ViewModel vm = tw.DataContext as Tool3ViewModel;
                if (tw != null && vm != null)
                {
                    if (dockingWindow is DocumentWindow)
                    {
                        DocumentItemViewModel doc_model = dockingWindow.DataContext as DocumentItemViewModel;
                        if (doc_model != null)
                        {
                            vm.RemoteDocDisplayOptions = doc_model;
                        }
                    }
                    else
                    {
                        if (tw != dockingWindow)
                        {
                            vm.RemoteDocDisplayOptions = vm.DefaultOptions;
                        }
                    }
                }
            }
        }


    }
}
