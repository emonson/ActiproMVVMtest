using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActiproMVVMtest.Models
{
    class SimulationModel
    {
        private List<MotileCell> cells = new List<MotileCell>();

        public SimulationModel()
        {
            this.CreateCells(100);
        }

        public SimulationModel(int numCells)
        {
            this.CreateCells(numCells);
        }

        public void MoveAllCells(double dx)
        {
            foreach (MotileCell cell in cells)
            {
                cell.Move(dx);
            }
        }

        /// <summary>
        /// retrieve the list of cells
        /// </summary>
        public List<MotileCell> Cells
        {
            get { return cells; }
        }

        private void CreateCells(int numCells)
        {
            MotileCell currentCell;
            for (int ii = 0; ii < numCells; ++ii)
            {
                currentCell = new MotileCell();
                cells.Add(currentCell);
            }
        }
    }


    class MotileCell
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
            position[0] += dx * 2.0 * (rand.NextDouble() - 0.5);
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
    }
}
