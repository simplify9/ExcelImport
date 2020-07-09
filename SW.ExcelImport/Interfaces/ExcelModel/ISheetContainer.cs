namespace SW.ExcelImport
{
    
    public interface ISheetContainer
    {
        string Reference { get; }
        ISheet[] Sheets { get;}
        IProcessOptions ProcessOptions { get;}
    }
    
}