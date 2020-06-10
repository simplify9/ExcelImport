using System.Linq;
namespace SW.ExcelImport.Domain
{
    public class RowRecord: IExcelRowParsed
    {
        private RowRecord()
        {

        }
        public RowRecord(IExcelRow row, SheetRecord parent)
        {
            Index = row.Index;
            Parent = parent;
            ExcelCells = row.Cells.Select(x=> new ExcelCell(x)).ToArray();

        }
        public RowRecord(IExcelRow row, SheetRecord parent, ExcelRowParseResult result)
        {
            Index = row.Index;
            Parent = parent;
            ExcelCells = row.Cells.Select(x=> new ExcelCell(x)).ToArray();
            UserDefinedId = result.UserDefinedId;
            ForeignUserDefinedId = result.ForeignUserDefinedId;
            InvalidIdValue = result.InvalidIdValue;
            InvalidForeignIdValue = result.InvalidForeignIdValue;
            InvalidCells = result.InvalidCells;
            IdDuplicate = result.IdDuplicate;
            ForeignIdNotFound = result.ForeignIdNotFound;

        }
        public SheetRecord Parent { get; set; }
        public int Index { get; private set; }
        public int? UserDefinedId { get; private set; }
        public int? ForeignUserDefinedId { get; private set; }
        public ExcelCell[] ExcelCells { get; private set; }

        public bool? InvalidIdValue { get; private set; }

        public bool? InvalidForeignIdValue { get; private set; }

        public bool? IdDuplicate { get; set; }

        public bool? ForeignIdNotFound { get; set; }

        public ICell[] Cells => ExcelCells;

        public object RowObject => throw new System.NotImplementedException();

        public int[] InvalidCells { get; set; }
    }

    
}
