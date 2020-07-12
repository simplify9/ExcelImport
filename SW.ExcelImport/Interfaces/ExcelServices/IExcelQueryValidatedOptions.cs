namespace SW.ExcelImport
{
    public interface IExcelQueryValidatedOptions : IExcelQueryOptions
    {
        int? FromRow { get; }
        int? ToRow { get; }
    }
}