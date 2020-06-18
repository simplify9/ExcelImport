using System;
using System.Collections.Generic;

namespace SW.ExcelImport.Model
{
    public class TypedParseToJsonOptions
    {
        public bool BlockOnParse { get; set; }
        public int MaxParseErrors { get; set; }
        public int MaxValidationErrors { get; set; }

        public ICollection<SheetMappingOptions> SheetsOptions { get; set; }
        public Type OnType { get; set; }
        public JsonNamingStrategy NamingStrategy { get; set; }

    }
}
