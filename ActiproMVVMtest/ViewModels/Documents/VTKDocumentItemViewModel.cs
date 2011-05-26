using System;
using System.Windows;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Forms.Integration;
using ActiproMVVMtest.Models;
using ActiproMVVMtest.Common.ViewModels;
using Kitware.VTK;

namespace ActiproMVVMtest.ViewModels
{
    public enum ColorSpaceModel { RGB, HSV, Lab, Diverging }
    public enum ColorSpaceRamp { Linear, Log10 }

	/// <summary>
	/// Represents a text view-model for the sample.
	/// </summary>
	public class VTKDocumentItemViewModel : DocumentItemViewModel {

		private static int counter = 1;
		private string text;
        private RenderWindowControl rwc;
        private WindowsFormsHost wfh;
        private VTKDataModel vtkData;    // public for xaml access...
        private vtkPolyDataMapper mapper;
        private vtkColorTransferFunction ctf;
        private System.Windows.Media.Color ctf_min_color = new System.Windows.Media.Color();
        private System.Windows.Media.Color ctf_max_color = new System.Windows.Media.Color();

        public ObservableCollection<string> CellAttributeArrayNames { get; set; }
        private string cellColorArrayName;
        private ColorSpaceModel cellColorMapSpaceModel;

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

            this.CellAttributeArrayNames = new ObservableCollection<string>();
            this.CellAttributeArrayNames.Add(this.vtkData.CellIdsArrayName);
            this.CellAttributeArrayNames.Add(this.vtkData.CellTypeArrayName);
            this.cellColorArrayName = this.CellAttributeArrayNames[0];
            this.cellColorMapSpaceModel = ColorSpaceModel.HSV;

            // create a VTK output control and make the forms host point to it
            rwc = new RenderWindowControl();
            wfh = new WindowsFormsHost();
            wfh.Child = rwc;
            rwc.CreateGraphics();
            rwc.RenderWindow.SetCurrentCursor(9);
            rwc.RenderWindow.GetInteractor().LeftButtonPressEvt += new vtkObject.vtkObjectEventHandler(leftMouseDown);

            // set up basic viewing
            vtkRenderer ren = rwc.RenderWindow.GetRenderers().GetFirstRenderer();
            vtkCamera camera = ren.GetActiveCamera();

            // background color
            ren.SetBackground(0.1, 0.1, 0.1);

            vtkSphereSource sph = vtkSphereSource.New();
            sph.SetThetaResolution(16);
            sph.SetPhiResolution(16);
            sph.SetRadius(0.02);

            vtkGlyph3D glyp = vtkGlyph3D.New();
            glyp.SetSourceConnection(sph.GetOutputPort(0));
            glyp.SetInputConnection(this.vtkData.OutputPort);
            glyp.ScalingOff();
            glyp.OrientOff();


            ctf = vtkColorTransferFunction.New();
            ctf_min_color = System.Windows.Media.Color.FromRgb(0, 128, 255);
            ctf_max_color = System.Windows.Media.Color.FromRgb(64, 255, 64);
            this.BuildCTF();

            //lut.SetValueRange(0.5, 1.0);
            //lut.SetSaturationRange(0.1, 1.0);
            //lut.SetHueRange(0.4, 0.6);
            //lut.SetAlphaRange(0.2, 1.0);
            //lut.SetRampToLinear();
            //lut.Build();

            mapper = vtkPolyDataMapper.New();
            mapper.SetInputConnection(glyp.GetOutputPort());
            mapper.SetLookupTable(ctf);
            mapper.ScalarVisibilityOn();
            mapper.SetScalarModeToUsePointFieldData();
            mapper.SelectColorArray(this.cellColorArrayName);
            // scalar range doens't affect anything when using a ctf (instead of a lut)
            // mapper.SetScalarRange(0, this.vtkData.NumPoints - 1);

            vtkActor actor = vtkActor.New();
            actor.SetMapper(mapper);
            //actor.GetProperty().SetRepresentationToWireframe();
            actor.GetProperty().SetRepresentationToSurface();
            rwc.RenderWindow.GetRenderers().GetFirstRenderer().AddViewProp(actor);
            ren.ResetCamera();
		}

        //
        // Public accessors
        //
        public System.Windows.Media.Color CTF_max_color
        {
            get { return ctf_max_color; }
            set
            {
                if (value == ctf_max_color)
                    return;
                else
                {
                    ctf_max_color = value;
                    this.BuildCTF();
                    this.Update();
                    this.NotifyPropertyChanged("CTF_max_color");
                }
            }
        }

        public System.Windows.Media.Color CTF_min_color
        {
            get { return ctf_min_color; }
            set
            {
                if (value == ctf_min_color)
                    return;
                else
                {
                    ctf_min_color = value;
                    this.BuildCTF();
                    this.Update();
                    this.NotifyPropertyChanged("CTF_min_color");
                }
            }
        }

        public string CellColorArrayName
        {
            get { return this.cellColorArrayName; }
            set
            {
                if (value == this.cellColorArrayName)
                    return;
                else
                {
                    this.cellColorArrayName = value;
                    this.BuildCTF();
                    this.mapper.SelectColorArray(this.cellColorArrayName);
                    this.Update();
                    this.NotifyPropertyChanged("CellColorArrayName");
                }
            }
        }

        public ColorSpaceModel CellColorMapSpaceModel
        {
            get { return this.cellColorMapSpaceModel; }
            set
            {
                if (value == this.cellColorMapSpaceModel)
                    return;
                else
                {
                    this.cellColorMapSpaceModel = value;
                    this.BuildCTF();
                    this.Update();
                    this.NotifyPropertyChanged("CellColorMapSpaceModel");
                }
            }
        }

		/////////////////////////////////////////////////////////////////////////////////////////////////////
		// PUBLIC PROCEDURES
		/////////////////////////////////////////////////////////////////////////////////////////////////////

        public void Update()
        {
            rwc.Update();
            rwc.Invalidate();
        }

        public void BuildCTF()
        {
            double[] range = new double[2];
            range = this.vtkData.Output.GetPointData().GetArray(this.cellColorArrayName).GetRange();

            ctf.SetColorSpace((int)this.cellColorMapSpaceModel);
            ctf.RemoveAllPoints();
            ctf.AddRGBPoint(range[0], ctf_min_color.ScR, ctf_min_color.ScG, ctf_min_color.ScB);
            ctf.AddRGBPoint(range[1], ctf_max_color.ScR, ctf_max_color.ScG, ctf_max_color.ScB);
            ctf.SetScale((int)ColorSpaceRamp.Linear);
            ctf.Build();
        }

        public void ResetColorMapRange()
        {
            mapper.SetScalarRange(0, this.vtkData.NumPoints - 1);
        }

        public void ResetCamera()
        {
            rwc.RenderWindow.GetRenderers().GetFirstRenderer().ResetCamera();
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
