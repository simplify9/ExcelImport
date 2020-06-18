using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SW.ExcelImport
{
    public class ExcelRowValidationResult : IExcelRowValidationResult
    {
        
        public ExcelRowValidationResult(string data)
        {
            Data = data;
            IsValid = true; 
        }
        public ExcelRowValidationResult(List<ValidationResult> errors)
        {
            IsValid = false;
            //errors.Select(e => e.)
        }
        public string Data { get; }

        public bool? IsValid { get; }
    }

}