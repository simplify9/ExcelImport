namespace SW.ExcelImport
{
    public interface IExcelRowParseResult
    {
        int? UserDefinedId { get; }
        int? ForeignUserDefinedId { get; }
        bool? InvalidIdValue { get; }
        bool? InvalidForeignIdValue { get; }
        string RowAsJson { get; }
        int[] InvalidCells { get; }
        bool? IdDuplicate { get; }
        bool? ForeignIdNotFound { get; }

    }

    public static class InterfacesExtensions
    {
        public static bool IsInvalidNullable(this bool? invalid) =>
            invalid.HasValue && invalid.Value;

        public static bool HasErrors(this IExcelRowParseResult result) => 
            result.InvalidIdValue.IsInvalidNullable() || result.InvalidForeignIdValue.IsInvalidNullable() 
                || result.IdDuplicate.IsInvalidNullable() || result.ForeignIdNotFound.IsInvalidNullable()
                || (result.InvalidCells != null  && result.InvalidCells.Length > 0);  
        
    }
}