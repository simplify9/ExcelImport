using System.Collections.Generic;
using System.Linq;
namespace SW.ExcelImport.Entity
{
    public class RowRecord: IExcelRow, IExcelRowParsed, IExcelRowValidated
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
            ForeignId = parseResult.ForeignId;
            RowAsData = parseResult.RowAsData;
            ParseOk = !parseResult.HasErrors();
        }
        public void FillData(string json, bool isValid, string[] errors = null)
        {
            IsValid = isValid;
            Data = json;
            if(isValid)
                RowAsData = null;
            ValidationErrors = errors;
            if(Children == null) return;
            
            foreach (var item in Children)
            {
                if(isValid)
                    item.RowAsData = null;
                
                item.IsValid = isValid;
            }

            
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
        public string RowAsData { get; private set; }
        public int[] InvalidCells { get; private set; }
        public bool? IdDuplicate { get; private set; }
        public bool? ForeignIdNotFound { get; private set; }
        public long? ForeignId { get; private set; }
        public IEnumerable<RowRecord> Children { get; set; }
        public RowRecord Parent { get; set; }

        public string Data { get; private set; }
        public bool? IsValid { get; private set; }

        public bool? ParseOk { get; private set; }

        public string[] ValidationErrors { get; private set; }
    }

    
}
