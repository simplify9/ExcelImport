using System.Linq;
using System;
using System.Collections.Generic;

namespace SW.ExcelImport
{


    public class TypedParseToJsonOptions : IProcessOptions
    {
        public TypedParseToJsonOptions()
        {
            NamingStrategy = JsonNamingStrategy.None;
        }
        
        public bool BlockOnParse { get; set; }
        public int MaxParseErrors { get; set; }
        public int MaxValidationErrors { get; set; }
        public string TypeAssemblyQualifiedName { get; set; }
        public ICollection<SheetMappingOptions> SheetsOptions { get; }
        public JsonNamingStrategy NamingStrategy { get; set; }
        public ExcelRowParseOnTypeRequest RowParseOnTypeRequest(IExcelRow row) =>
            new ExcelRowParseOnTypeRequest
            {
                NamingStrategy = NamingStrategy,
                RootType = this.OnType(),
                Options = SheetsOptions?.FirstOrDefault( o => o.SheetIndex == row.Sheet.Index) ?? 
                    SheetMappingOptions.Default(row.Sheet.Index),
                Row = row
            };

        public IExcelRowParseRequest RowParseRequest(IExcelRow row)
            => RowParseOnTypeRequest(row);
    }
}
