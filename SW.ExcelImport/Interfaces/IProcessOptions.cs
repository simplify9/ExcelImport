using System.Collections.Generic;

namespace SW.ExcelImport
{
    public interface IProcessOptions
    {
        bool BlockOnParse { get; }
        int MaxParseErrors { get; }
        int MaxValidationErrors { get; }
        
        ICollection<SheetMappingOptions> SheetsOptions { get; }
        IExcelRowParseRequest RowParseRequest (IExcelRow row);

    }
}
