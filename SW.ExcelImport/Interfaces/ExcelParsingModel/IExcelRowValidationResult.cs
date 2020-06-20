using System.Collections.Generic;

namespace SW.ExcelImport
{
    public interface IExcelRowValidationResult
    {
        string Data { get; } 
        bool? IsValid { get; }
        //IDictionary<string,string> Errors { get; }
    }
}