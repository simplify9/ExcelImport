using ExcelDataReader;

namespace SW.ExcelImport
{
    public class Cell: ICell
    {
        readonly int index;
        
        readonly IExcelDataReader reader;
        public Cell(int index, IExcelDataReader reader)
        {
            this.index = index;
            this.reader= reader;
        }
        
        public string Type => reader.GetFieldType(index)?.GetType().Name;
        public object Value => reader.GetValue(index);
        public int NumberFormatIndex => reader.GetNumberFormatIndex(index);
        public string NumberFormatString => reader.GetNumberFormatString(index);

    }

}