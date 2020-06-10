using System;

namespace SW.ExcelImport
{
    public class SheetContainer : ISheetContainer
    {

        public SheetContainer(string reference, int sheetCount)
        {
            this.Reference = reference ?? throw new ArgumentNullException(nameof(reference));
            this.SheetCount = sheetCount;

        }
        public string Reference { get; }

        public int SheetCount { get; }
    }


}