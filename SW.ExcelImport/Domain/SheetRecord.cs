using System.ComponentModel;
using System;

namespace SW.ExcelImport.Domain
{
    public class SheetRecord: ISheet
    {
        protected SheetRecord()
        {
            
        }
        
        public SheetRecord(ExcelFileRecord parent, ISheet sheet)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Index = sheet.Index;
            HeaderMap = sheet.HeaderMap;
            Name = sheet.Name;
            Empty = sheet.Empty;
            EmptyData = sheet.EmptyData;
            RowCount = sheet.RowCount;
            Primary = sheet.Primary;
            InvalidMap = sheet.InvalidMap;
            InvalidName = sheet.InvalidName;

        }
        public ISheetContainer Parent {get; private set;}
        public int Index { get; private set; }
        public string[] HeaderMap { get; private set; }
        public string Name { get; private set;}

        public bool Empty { get; private set;}

        public bool EmptyData { get; private set; }

        public int RowCount { get; private set; }

        public bool Primary { get; private set; }
        public int[] InvalidMap { get; private set; }
        public bool InvalidName { get; private set; }
    }
}
