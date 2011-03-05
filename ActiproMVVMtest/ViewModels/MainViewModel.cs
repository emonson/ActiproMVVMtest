﻿using System.Collections.Generic;
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

        private SimulationModel simModel;
        private VTKDataModel vtkModel;

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="MainViewModel"/> class.
		/// </summary>
		public MainViewModel() {

			// Build tool items
			this.toolItems = new DeferrableObservableCollection<ToolItemViewModel>();

			// Tool 2
            ToolItemViewModel viewModel = new Tool2ViewModel();
			viewModel.DefaultDock = Dock.Bottom;
			viewModel.DockGroup = "BottomGroup";
			this.toolItems.Add(viewModel);

            // Sim Config
            viewModel = new SimConfigViewModel();
            viewModel.DefaultDock = Dock.Left;
            viewModel.DockGroup = "LeftGroup";
            this.toolItems.Add(viewModel);

            // Tool 3
			viewModel = new Tool3ViewModel();
			viewModel.DefaultDock = Dock.Left;
            viewModel.DockGroup = "HiddenGroup";
			viewModel.IsInitiallyAutoHidden = true;
			this.toolItems.Add(viewModel);

            this.simModel = new SimulationModel();

            this.AddNewDocument("VTKDocument");
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
            this.AddNewDocument(parameter);
        }

    }
}
