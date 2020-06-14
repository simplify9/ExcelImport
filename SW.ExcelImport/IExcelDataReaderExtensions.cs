using ExcelDataReader;

namespace SW.ExcelImport
{
    public static class IExcelDataReaderExtensions
    {
        public static ICell[] GetCells(this IExcelDataReader reader)
        {
            if(reader.RowCount == 0) return null;
            var cells = new Cell[reader.FieldCount];
            for (int i = 0; i < cells.Length; i++)
                cells[i] = new Cell(i, reader);
            return cells;
        }
    }
}
