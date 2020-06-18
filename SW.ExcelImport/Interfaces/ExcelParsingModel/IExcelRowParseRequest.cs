using SW.ExcelImport.Model;

namespace SW.ExcelImport
{
    public interface IExcelRowParseRequest
    {
        IExcelRow Row { get; }
        SheetMappingOptions Options { get; }
    }
}