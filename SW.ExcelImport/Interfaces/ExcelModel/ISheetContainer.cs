namespace SW.ExcelImport
{
    public interface ISheetContainer
    {
        string Reference { get; }
        int SheetCount { get; }
    }
    
}