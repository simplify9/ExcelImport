namespace SW.ExcelImport
{
    public interface IExcelRowValidationResult
    {
        string Data { get; } 
        bool? IsValid { get; }
        
    }
}