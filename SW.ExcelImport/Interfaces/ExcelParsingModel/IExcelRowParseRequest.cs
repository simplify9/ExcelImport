namespace SW.ExcelImport
{
    public interface IExcelRowParseRequest
    {
        IExcelRow Row { get; }
        ISheet Sheet {get;}
        ISheet MainSheet {get;}
        bool IsMainSheet { get; }
        int? MainSheetIdColumn { get; }
        bool MainSheetIndexAsId { get; }   
    }

}