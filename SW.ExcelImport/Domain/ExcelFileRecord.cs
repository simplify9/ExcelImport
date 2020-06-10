using System;
using SW.PrimitiveTypes;

namespace SW.ExcelImport.Domain
{
    public class ExcelFileRecord: BaseEntity, ISheetContainer
    {
        public ExcelFileRecord(string reference, int sheetCount)
        {
            this.Reference = reference ?? throw new ArgumentNullException(nameof(reference));
            this.SheetCount = sheetCount;
        }
        public string Reference { get; private set; }
        public Audit Audit { get; set; }
        public int SheetCount { get; set; }

        string ISheetContainer.Reference => throw new NotImplementedException();
    }
}
