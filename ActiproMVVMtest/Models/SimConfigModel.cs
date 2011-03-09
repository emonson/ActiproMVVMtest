using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActiproMVVMtest.Models
{
    public class SimConfigModel
    {
        private int duration;
        private int visInterval;
        private int numCells;
        private double dx;
        
        public SimConfigModel()
        {
            duration = 2000;
            visInterval = 100;
            numCells = 100;
            dx = 0.001;
        }

        public int Duration
        {
            get { return duration; }
            set { duration = value; }
        }

        public int VisInterval
        {
            get { return visInterval; }
            set { visInterval = value; }
        }

        public int NumCells
        {
            get { return numCells; }
            set { numCells = value; }
        }

        public double Dx
        {
            get { return dx; }
            set { dx = value; }
        }
    }
}
