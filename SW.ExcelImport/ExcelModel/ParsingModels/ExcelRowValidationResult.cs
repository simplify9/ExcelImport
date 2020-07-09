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
            ValidationErrors = errors.Select(x => x.ErrorMessage).ToArray();
        }
        public string Data { get; }

        public bool? IsValid { get; }

        public string[] ValidationErrors { get; }
    }

}