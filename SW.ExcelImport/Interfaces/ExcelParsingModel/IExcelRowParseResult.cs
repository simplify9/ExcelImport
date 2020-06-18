namespace SW.ExcelImport
{
    public interface IExcelRowParseResult
    {
        int? UserDefinedId { get; }
        int? ForeignUserDefinedId { get; }
        long? ForeignId { get; }
        bool? InvalidIdValue { get; }
        bool? InvalidForeignIdValue { get; }
        string RowAsData { get; }
        int[] InvalidCells { get; }
        bool? IdDuplicate { get; }
        bool? ForeignIdNotFound { get; }
        bool? ParseOk { get; }
    }
}