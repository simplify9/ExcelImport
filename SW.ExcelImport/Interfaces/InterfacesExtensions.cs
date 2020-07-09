using System.Linq;
namespace SW.ExcelImport
{
    public static class InterfacesExtensions
    {
        public static bool IsInvalidNullable(this bool? invalid) =>
            invalid.HasValue && invalid.Value;

        public static bool HasErrors(this IExcelRowParseResult result) => 
            result.InvalidIdValue.IsInvalidNullable() || result.InvalidForeignIdValue.IsInvalidNullable() 
                || result.IdDuplicate.IsInvalidNullable() || result.ForeignIdNotFound.IsInvalidNullable()
                || (result.InvalidCells != null  && result.InvalidCells.Length > 0);  
        
        public static bool HasErrors(this ISheet result) => 
            result.Empty || result.EmptyData || result.InvalidName || result.InvalidHeaders.Any();
        
    }
}