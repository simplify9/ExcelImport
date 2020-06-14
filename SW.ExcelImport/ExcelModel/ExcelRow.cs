using System;
using System.Linq;
using ExcelDataReader;
namespace SW.ExcelImport
{
    public class ExcelRow: IExcelRow
    {
        public int Index { get; }
        
        public ICell[] Cells { get; }

        public ISheet Sheet { get; }
        
        public ExcelRow(int index, ISheet sheet, IExcelDataReader reader)
        {
            this.Index = index;
            Sheet = sheet;
            this.Cells = reader.GetCells();
        }
        
        
    }
}