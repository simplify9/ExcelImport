using System.ComponentModel;
using System;

namespace SW.ExcelImport.Entity
{
    public class SheetRecord: ISheet
    {
        protected SheetRecord()
        {
            
        }
        
        public SheetRecord(ExcelFileRecord parent, ISheet sheet, SheetValidationResult validationResult)
        {
            //Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Index = sheet.Index;
            
            Name = sheet.Name;
            Empty = sheet.Empty;
            EmptyData = sheet.EmptyData;
            RowCount = sheet.RowCount;
            InvalidName = validationResult.InvalidName;
            InvalidHeaders = validationResult.InvalidHeaders;
            Map = validationResult.Map;
            IgnoreFirstRow = validationResult.IgnoreFirstRow;

        }
        public long Id { get; set; }
        public ExcelFileRecord ExcelFileRecord {get; private set;}
        public int ParentId { get; set; }
        public int Index { get; private set; }
        
        public string Name { get; private set;}

        public bool Empty { get; private set;}

        public bool EmptyData { get; private set; }

        public int RowCount { get; private set; }
        public CellRecord[] HeaderCellRecords { get; set; }
        public bool InvalidName { get; set; } 
        public int[] InvalidHeaders { get; set; }

        public string[] Map { get; }
        public bool IgnoreFirstRow { get; }

        public ICell[] Header => HeaderCellRecords;
        public ISheetContainer Parent => ExcelFileRecord;
        
    }
}
