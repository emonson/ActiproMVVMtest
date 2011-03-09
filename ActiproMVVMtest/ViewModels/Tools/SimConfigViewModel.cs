using System;
using System.Windows.Media.Imaging;
using ActiproMVVMtest.Models;
using ActiproMVVMtest.Common.ViewModels;

namespace ActiproMVVMtest.ViewModels
{

	/// <summary>
	/// Represents a tool view-model for the sample.
	/// </summary>
	public class SimConfigViewModel : ToolItemViewModel {

        private SimConfigModel simConfig;

		/// <summary>
		/// Initializes a new instance of the <see cref="Tool1ViewModel"/> class.
		/// </summary>
        public SimConfigViewModel(SimConfigModel sc)
        {
			this.ImageSource = new BitmapImage(new Uri("/Resources/Images/Toolbox16.png", UriKind.Relative));
			this.Name = "simConfigWindow";
			this.Title = "Simulation Configuration";

            this.simConfig = sc;
		}

        public int Duration
        {
            get { return simConfig.Duration; }
            set
            {
                if (value == simConfig.Duration)
                    return;
                simConfig.Duration = value;
                NotifyPropertyChanged("Duration");
            }
        }

        public int VisInterval
        {
            get { return simConfig.VisInterval; }
            set
            {
                if (value == simConfig.VisInterval)
                    return;
                simConfig.VisInterval = value;
                NotifyPropertyChanged("VisInterval");
            }
        }

        public int NumCells
        {
            get { return simConfig.NumCells; }
            set
            {
                if (value == simConfig.NumCells)
                    return;
                simConfig.NumCells = value;
                NotifyPropertyChanged("NumCells");
            }
        }

        public double Dx
        {
            get { return simConfig.Dx; }
            set
            {
                if (value == simConfig.Dx)
                    return;
                simConfig.Dx = value;
                NotifyPropertyChanged("Dx");
            }
        }

    }
}
