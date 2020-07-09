using ExcelDataReader;

namespace SW.ExcelImport
{
    public class Cell: ICell
    {
        public Cell(int index, IExcelDataReader reader)
        {
            Type = reader.GetFieldType(index)?.GetType().Name;
            Value = reader.GetValue(index);
            NumberFormatIndex = reader.GetNumberFormatIndex(index);
            NumberFormatString = reader.GetNumberFormatString(index);

        }
        
        public string Type { get; } 
        public object Value { get; } 
        public int NumberFormatIndex { get; } 
        public string NumberFormatString { get; } 

    }

}