using System;
using System.Linq;
using ExcelDataReader;
namespace SW.ExcelImport
{
    public class ExcelRow: IExcelRow
    {
        public int Index { get; private set; }
        
        public Cell[] ExcelCells { get; private set;}
        public ICell[] Cells => ExcelCells;

        private readonly Sheet sheet;
        
        public ExcelRow(int index, IExcelDataReader reader)
        {
            this.Index = index;
            FillCells(reader);
        }
        
        private void FillCells(IExcelDataReader reader)
        {
            this.ExcelCells = new Cell[reader.FieldCount];
            for (int i = 0; i < Cells.Length; i++)
                Cells[i] = new Cell(i, reader);
        }
        
    }
}