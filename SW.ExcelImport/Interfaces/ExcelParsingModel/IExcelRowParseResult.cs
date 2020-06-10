namespace SW.ExcelImport
{
    public interface IExcelRowParseResult
    {
        int? UserDefinedId { get; }
        int? ForeignUserDefinedId { get; }
        bool? InvalidIdValue { get; }
        bool? InvalidForeignIdValue { get; }
        object RowObject { get; }
        int[] InvalidCells { get; }
        bool? IdDuplicate { get; }
        bool? ForeignIdNotFound { get; }

    }

}