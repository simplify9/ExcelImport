using System.Collections.Generic;

namespace SW.ExcelImport.Model
{
    public interface IParseOptions
    {
        bool BlockOnParse { get; }
        int MaxParseErrors { get; }
        int MaxValidationErrors { get; }
        
        ICollection<SheetMappingOptions> SheetsOptions { get; }
    }
}
