using System;

namespace SW.ExcelImport
{
    public class SheetValidationResult
    {
        private SheetValidationResult()
        {
            
        }
        public static SheetValidationResult EmptySheet()
        {
            return new SheetValidationResult
            {
                Empty = true,
                InvalidName = false,
                InvalidHeaders = new int[]{}
            };
        }
        
        public static SheetValidationResult SheetNotPresent()
        {
            return new SheetValidationResult
            {
                SheetNotFound = true
            };
        }
        public SheetValidationResult(string[] map, bool ignoreFirstRow, bool invalidName, int[] invalidHeaders )
        {
            InvalidName = invalidName;
            InvalidHeaders = invalidHeaders;
            Empty = false;
            IgnoreFirstRow = ignoreFirstRow;
            Map = map;
            InvalidName = invalidName;
        }
        
        public bool Empty { get; private set; }
        public bool InvalidName { get; private set; }
        public int[] InvalidHeaders { get; private set; }
        public string[] Map { get; private set; }
        public bool IgnoreFirstRow { get; private set; }
        public bool SheetNotFound { get; set; }
        public bool HasErrors => InvalidName || Empty || InvalidHeaders.Length > 0 || SheetNotFound;
    }
}