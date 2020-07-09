using System;

namespace SW.ExcelImport
{

    public class ExcelRowParseOnTypeRequest: IExcelRowParseRequest
    {
        
        public IExcelRow Row { get; set; }
        public SheetMappingOptions Options { get; set; }
        public Type RootType { get; set; }
        public JsonNamingStrategy NamingStrategy { get; set; }

    }  
    
    

}