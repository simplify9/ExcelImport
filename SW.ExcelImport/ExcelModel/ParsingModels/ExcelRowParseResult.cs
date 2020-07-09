namespace SW.ExcelImport
{
    public class ExcelRowParseResult : IExcelRowParseResult
    {
        public int? UserDefinedId { get; private set; }
        public int? ForeignUserDefinedId { get; private set; }
        public bool? InvalidIdValue { get; private set; }
        public bool? InvalidForeignIdValue { get; private set; }
        public string RowAsData { get; private set; }
        public int[] InvalidCells { get; private set; }
        public bool? IdDuplicate { get; private set; }
        public bool? ForeignIdNotFound { get; private set; }

        public long? ForeignId { get; private set; }

        public bool? ParseOk =>  !this.HasErrors();

        // public bool HasError => InvalidCells.Length > 0 || InvalidIdValue || InvalidForeignIdValue 
        //     || IdDuplicate || ForeignIdNotFound;

        public void Populate(IdParseResult result)
        {
            UserDefinedId = result.UserDefinedId;
            ForeignUserDefinedId  = result.ForeignUserDefinedId;
            InvalidIdValue = result.InvalidIdValue;
            InvalidForeignIdValue = result.InvalidForeignIdValue;
        }

        public void Populate(CellsParseReult result)
        {
            RowAsData = result.RowMapped;
            InvalidCells = result.InvalidCells;
        }

        public void Populate(IdInStoreValidationResult result)
        {
            IdDuplicate = result.IdDuplicate;
            ForeignIdNotFound = result.ForeignIdNotFound;
            ForeignId = result.ForeignId;

        }
    }

}