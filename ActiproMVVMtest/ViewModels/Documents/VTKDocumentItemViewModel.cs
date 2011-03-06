using System;
using System.Windows;
using System.Windows.Forms.Integration;
using ActiproMVVMtest.Models;
using ActiproMVVMtest.Common.ViewModels;
using Kitware.VTK;

namespace ActiproMVVMtest.ViewModels
{

	/// <summary>
	/// Represents a text view-model for the sample.
	/// </summary>
	public class VTKDocumentItemViewModel : DocumentItemViewModel {

		private static int counter = 1;
		private string text;
        private RenderWindowControl rwc;
        private WindowsFormsHost wfh;
        private VTKDataModel vtkData;

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// OBJECT
		/////////////////////////////////////////////////////////////////////////////////////////////////////

		/// <summary>
		/// Initializes a new instance of the <see cref="TextDocumentItemViewModel"/> class.
		/// </summary>
		public VTKDocumentItemViewModel(VTKDataModel dataModel) {

			this.Title = string.Format("VTK Doc {0}", counter++);
			this.Text = string.Format("Dynamically created at {0}", DateTime.Now);
			this.Description = "VTK document";
            this.vtkData = dataModel;

            // create a VTK output control and make the forms host point to it
            rwc = new RenderWindowControl();
            wfh = new WindowsFormsHost();
            wfh.Child = rwc;
            rwc.CreateGraphics();
            rwc.RenderWindow.SetCurrentCursor(9);
            // rwc.RenderWindow.GetInteractor().LeftButtonPressEvt += new vtkObject.vtkObjectEventHandler(leftMouseDown);

            // set up basic viewing
            vtkRenderer ren = rwc.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkCamera camera = ren.GetActiveCamera();

            // background color
            ren.SetBackground(0.1, 0.1, 0.1);


            vtkSphereSource sph = vtkSphereSource.New();
            sph.SetThetaResolution(16);
            sph.SetPhiResolution(16);
            sph.SetRadius(0.05);

            vtkGlyph3D glyp = vtkGlyph3D.New();
            glyp.SetSourceConnection(sph.GetOutputPort(0));
            glyp.SetInputConnection(this.vtkData.OutputPort);
            glyp.ScalingOff();
            glyp.OrientOff();

            vtkLookupTable lut = vtkLookupTable.New();
            lut.SetValueRange(0.5, 1.0);
            lut.SetSaturationRange(0.1, 1.0);
            lut.SetHueRange(0.4, 0.6);
            lut.SetRampToLinear();
            lut.Build();

            vtkPolyDataMapper mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(glyp.GetOutputPort());
            mapper.SetLookupTable(lut);
            mapper.ScalarVisibilityOn();
            mapper.SetScalarModeToUsePointFieldData();
            // uncomment the next line to avoid the out of memory problem
            mapper.SelectColorArray(this.vtkData.CellIdsArrayName);
            mapper.SetScalarRange(0, this.vtkData.NumPoints - 1);

            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            //actor.GetProperty().SetRepresentationToWireframe();
            actor.GetProperty().SetRepresentationToSurface();
            rwc.RenderWindow.GetRenderers().GetFirstRenderer().AddViewProp(actor);
            ren.ResetCamera();
		}

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update()
        {
            rwc.Update();
            rwc.Invalidate();
        }

        /// <summary>
        /// handler for left mouse button down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void leftMouseDown(vtkObject sender, vtkObjectEventArgs e)
        {
            rwc.RenderWindow.GetRenderers().GetFirstRenderer().ResetCamera();
        }

        /// <summary>
		/// Gets or sets the text associated with the view-model.
		/// </summary>
		/// <value>The text associated with the view-model.</value>
		public string Text {
			get {
				return this.text;
			}
			set {
				if (this.text != value) {
					this.text = value;
					this.NotifyPropertyChanged("Text");
				}
			}
		}

        /// <summary>
        /// Gets the VTK RenderWindowControl associated with the view-model.
        /// </summary>
        /// <value>The VTK RenderWindowControl associated with the view-model.</value>
        public WindowsFormsHost Wfh
        {
            get { return this.wfh; }
        }

    }
}
