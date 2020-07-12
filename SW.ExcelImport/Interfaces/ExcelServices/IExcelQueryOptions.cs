namespace SW.ExcelImport
{
    public interface IExcelQueryOptions
    {
        string Reference { get; }
        int? PageIndex { get; }
        int? PageSize { get; }
        QueryRowStatus RowStatus { get; }
    }
}