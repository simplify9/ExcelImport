using ExcelDataReader;
using System;

namespace SW.ExcelImport
{
    public class Sheet : ISheet
    {

        private Sheet()
        {

        }
        public static Sheet EmptySheet(ISheetContainer parent,string name, int index,bool primary)
        {
            var sheet = new Sheet();

            sheet.Name = name;
            sheet.Index = index;
            sheet.Empty = true;
            sheet.EmptyData = true;
            sheet.Primary = primary;
            
            return sheet;

        }
        public static Sheet EmptyRecordsSheet(ISheetContainer parent,string[] header, string name, int index, bool primary)
        {
            var sheet = new Sheet();


            sheet.Name = name;
            sheet.Index = index;
            sheet.EmptyData = true;
            sheet.Primary = primary;

            return sheet;
        }

        public static Sheet InvalidMapSheet(ISheetContainer parent,string[] header, string name, int index, bool primary, int[] invalidMap)
        {
            var sheet = new Sheet();


            sheet.Name = name;
            sheet.Index = index;
            sheet.EmptyData = true;
            sheet.Primary = primary;
            sheet.InvalidMap = invalidMap;
            return sheet;
        }
        
        public static Sheet InvalidNameSheet(ISheetContainer parent,string name, int index)
        {
            var sheet = new Sheet();


            sheet.Name = name;
            sheet.Index = index;
            sheet.EmptyData = true;
            sheet.InvalidName = true;
            
            return sheet;
        }

        public Sheet(ISheetContainer parent, string[] header, string name, int index, int rowCount, bool primary)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            HeaderMap = header;
            Name = name;
            Index = index;
            Primary = primary;
        }
        public ISheetContainer Parent { get; private set; }
        public int Index { get; private set; }
        public string[] HeaderMap { get; private set; }
        public string[] InvalidColumns { get; private set; }
        public string Name { get; private set; }

        public bool Empty { get; private set; }

        public bool EmptyData { get; private set; }
        public bool InvalidName { get; private set; }

        public int RowCount { get; private set; }

        public bool Primary { get; private set; }

        public int[] InvalidMap { get; private set; }
    }


}