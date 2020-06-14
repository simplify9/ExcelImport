using System;
using SW.ExcelImport.Model;

namespace SW.ExcelImport
{
    
    public class ExcelRowParseRequest: IExcelRowParseRequest
    {
        
        public IExcelRow Row { get; set; }
        public SheetMappingOptions Options { get; set; }
        
    }  
    

}