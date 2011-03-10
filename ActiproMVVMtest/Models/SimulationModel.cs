using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using ActiproMVVMtest.ViewModels;

namespace ActiproMVVMtest.Models
{
    public class SimulationModel
    {
        private List<MotileCell> cells = new List<MotileCell>();
        private SimConfigModel simConfigModel;
        private int time;

        public SimulationModel(SimConfigModel sm)
        {
            this.simConfigModel = sm;
            this.CreateCells();
            this.time = 0;
        }

        public void MoveAllCells()
        {
            foreach (MotileCell cell in cells)
            {
                cell.Move(simConfigModel.Dx);
            }
            time += 1;
        }

        /// <summary>
        /// retrieve the list of cells
        /// </summary>
        public List<MotileCell> Cells
        {
            get { return cells; }
        }

        public int Time
        {
            get { return time; }
        }

        private void CreateCells()
        {
            MotileCell currentCell;
            for (int ii = 0; ii < simConfigModel.NumCells; ++ii)
            {
                currentCell = new MotileCell();
                cells.Add(currentCell);
            }
        }

        public void ResetCells()
        {
            cells.Clear();
            MotileCell.ResetIds();
            time = 0;
            CreateCells();
        }
    }


    public class MotileCell
    {
        private double[] position;
        private static int idCounter = 0;
        private int cellId;
        private int cellType;
        private static Random rand = new Random();

        public MotileCell()
        {
            position = new double[3];
            position[0] = rand.NextDouble();
            position[1] = rand.NextDouble();
            position[2] = rand.NextDouble();

            cellId = idCounter;
            idCounter++;

            cellType = rand.Next(3);
        }

        public void Move(double dx)
        {
            position[0] += dx * 2.0 * (rand.NextDouble() - 0.5);
            position[1] += dx * 2.0 * (rand.NextDouble() - 0.5);
            position[2] += dx * 2.0 * (rand.NextDouble() - 0.5);
        }

        public double[] Position
        {
            get { return position; }
        }

        public int CellId
        {
            get { return cellId; }
        }

        public int CellType
        {
            get { return cellType; }
        }

        public static void ResetIds()
        {
            idCounter = 0;
        }
    }
}
