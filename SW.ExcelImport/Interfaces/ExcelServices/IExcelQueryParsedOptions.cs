namespace SW.ExcelImport
{
    public interface IExcelQueryParsedOptions : IExcelQueryOptions
    {
        int? FromSheet { get; } 
        int? ToSheet { get; } 
        int? FromRow { get; }
        int? ToRow { get; }
    }
}