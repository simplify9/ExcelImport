namespace SW.ExcelImport
{
    public interface ICell
    {
        string Type { get;  }
        object Value { get; }
        int NumberFormatIndex { get; }
        string NumberFormatString { get; }
    }

    
}