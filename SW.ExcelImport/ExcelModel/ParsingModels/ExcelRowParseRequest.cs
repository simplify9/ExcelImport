namespace SW.ExcelImport
{
    public class ExcelRowParseRequest: IExcelRowParseRequest
    {
        
        public IExcelRow Row { get; set; }
        public ISheet Sheet {get; set;}
        public ISheet MainSheet {get; set;}
        public bool IsMainSheet { get; set; }

        public int? MainSheetIdColumn { get; set; }
        public bool MainSheetIndexAsId { get; set; }
    }  
    

}