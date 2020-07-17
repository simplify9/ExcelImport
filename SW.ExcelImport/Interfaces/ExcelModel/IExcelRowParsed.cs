namespace SW.ExcelImport
{
    public interface IExcelRowParsed: IExcelRow, IExcelRowParseResult
    {
        string Reference { get; }
    }

}