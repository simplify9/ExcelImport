namespace SW.ExcelImport
{
    public class RowParseResultTyped<T> 
    {
        public IExcelRow Row { get; internal set; }
        public T RowMapped { get;internal  set; }
        public int[] InvalidCells { get; internal set; }
    }
}