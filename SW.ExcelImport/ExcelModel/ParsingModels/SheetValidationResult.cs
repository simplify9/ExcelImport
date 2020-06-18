using System;

namespace SW.ExcelImport
{
    public class SheetValidationResult
    {

        public SheetValidationResult(bool invalidName, int[] invalidHeaders,Type onType, bool ignoreFirstRow )
        {
            this.InvalidName = invalidName;
            this.InvalidHeaders = invalidHeaders;
        }
        public bool InvalidName { get; private set; }
        public int[] InvalidHeaders { get; private set; }
        public string[] Map { get; private set; }
        public bool IgnoreFirstRow { get; private set; }
        public bool HasErrors => InvalidName || InvalidHeaders.Length > 0;
    }
}