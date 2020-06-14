using System.Linq;
namespace SW.ExcelImport.Domain
{
    public class RowRecord: IExcelRow, IExcelRowParseResult
    {
        protected RowRecord()
        {

        }
        public RowRecord(IExcelRow row, SheetRecord sheet)
        {
            FillData(row,sheet);
        }
        public RowRecord(IExcelRow row,  SheetRecord sheet,IExcelRowParseResult parseResult)
        {
            FillData(row,sheet);
            FillParseResult(parseResult);
        }
        private void FillData(IExcelRow row, SheetRecord sheetRecord)
        {
            Index = row.Index;
            SheetRecord = sheetRecord;
            CellRecords = row.Cells.Select(x=> new CellRecord(x)).ToArray();
        }

        public void FillParseResult(IExcelRowParseResult parseResult)
        {
            UserDefinedId = parseResult.UserDefinedId;
            ForeignUserDefinedId = parseResult.ForeignUserDefinedId;
            InvalidIdValue = parseResult.InvalidIdValue;
            InvalidForeignIdValue = parseResult.InvalidForeignIdValue;
            InvalidCells = parseResult.InvalidCells;
            IdDuplicate = parseResult.IdDuplicate;
            ForeignIdNotFound = parseResult.ForeignIdNotFound;
            RowAsJson = parseResult.RowAsJson;
        }
        public long Id { get; private set; }
        public SheetRecord SheetRecord { get; private set; }
        public int Index { get; private set; }
        public CellRecord[] CellRecords { get; private set; }

        public ISheet Sheet => SheetRecord;

        public ICell[] Cells => CellRecords;

        public int? UserDefinedId { get; private set; }

        public int? ForeignUserDefinedId { get; private set; }

        public bool? InvalidIdValue { get; private set; }

        public bool? InvalidForeignIdValue { get; private set; }

        public string RowAsJson { get; private set; }

        public int[] InvalidCells { get; private set; }

        public bool? IdDuplicate { get; private set; }

        public bool? ForeignIdNotFound { get; private set; }
    }

    
}
