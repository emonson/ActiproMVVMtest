using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kitware.VTK;

namespace ActiproMVVMtest.Models
{
    public class VTKDataModel
    {
        private SimulationModel simModel;

        private vtkPolyData poly;
        private vtkPoints points;
        private vtkCellArray verts;
        private vtkIntArray cellIDs, cellTypes;
        private string cellIdsArrayName = "cellID";
        private string cellTypeArrayName = "cellType";

        public VTKDataModel(SimulationModel sm)
        {
            simModel = sm;

            int numCells = sm.Cells.Count;

            cellIDs = vtkIntArray.New();
            cellIDs.SetNumberOfComponents(1);
            cellIDs.SetNumberOfValues(numCells);
            cellIDs.SetName(cellIdsArrayName);

            cellTypes = vtkIntArray.New();
            cellTypes.SetNumberOfComponents(1);
            cellTypes.SetNumberOfValues(numCells);
            cellTypes.SetName(cellTypeArrayName);

            points = vtkPoints.New();
            points.SetNumberOfPoints(numCells);

            verts = vtkCellArray.New();
            verts.Allocate(verts.EstimateSize(1, numCells), 1000);
            verts.InsertNextCell(numCells);

            foreach (MotileCell cell in sm.Cells)
            {
                int i = cell.CellId;
                int c = cell.CellType;
                double[] p = cell.Position;
                points.SetPoint(i, p[0], p[1], p[2]);
                cellIDs.SetValue(i, i);
                cellTypes.SetValue(i, c);
                verts.InsertCellPoint(i);
            }

            poly = vtkPolyData.New();
            poly.SetPoints(points);
            poly.SetVerts(verts);
            poly.GetPointData().AddArray(cellIDs);
            poly.GetPointData().AddArray(cellTypes);
        }

        public void Update()
        {
            int numCells = simModel.Cells.Count;

            cellIDs.SetNumberOfValues(numCells);
            cellTypes.SetNumberOfValues(numCells);
            points.SetNumberOfPoints(numCells);
            verts.Allocate(verts.EstimateSize(1, numCells), 1000);
            verts.InsertNextCell(numCells);

            foreach (MotileCell cell in simModel.Cells)
            {
                int i = cell.CellId;
                int c = cell.CellType;
                double[] p = cell.Position;
                points.SetPoint(i, p[0], p[1], p[2]);
                cellIDs.SetValue(i, i);
                cellTypes.SetValue(i, c);
                verts.InsertCellPoint(i);
            }
            poly.Modified();
        }

        public vtkPolyData Output
        {
            get { return poly; }
        }

        public long NumPoints
        {
            get { return poly.GetPoints().GetNumberOfPoints(); }
        }
        
        public vtkAlgorithmOutput OutputPort
        {
            get { return poly.GetProducerPort(); }
        }

        public string CellIdsArrayName
        {
            get { return cellIdsArrayName; }
        }

        public string CellTypeArrayName
        {
            get { return cellTypeArrayName; }
        }
    }
}
