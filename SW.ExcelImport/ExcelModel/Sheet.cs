using ExcelDataReader;
using System;

namespace SW.ExcelImport
{
    public class Sheet : ISheet
    {

        private Sheet()
        {

        }

        public Sheet(IExcelDataReader reader, ISheetContainer parent, int index, bool ignoreHeader)
        {
            if (reader == null) throw new ArgumentNullException(nameof(reader));

            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
            Index = index;

            Name = reader.Name;
            RowCount = reader.RowCount;
            Index = index;
            if (RowCount == 0)
                Empty = true;

            if (ignoreHeader && RowCount <= 1)
                EmptyData = true;

            if (!ignoreHeader && RowCount == 0)
                EmptyData = true;

            Header = new ICell[reader.FieldCount];

            if (RowCount != 0)
                for (int i = 0; i < reader.FieldCount; i++)
                    Header[i] = new Cell(i, reader);
            IgnoreFirstRow = ignoreHeader;

        }
        public ISheetContainer Parent { get; private set; }
        public int Index { get; private set; }
        public ICell[] Header { get; private set; }
        public string Name { get; private set; }

        public bool Empty { get; private set; }

        public bool EmptyData { get; private set; }
        public bool InvalidName => false;

        public int RowCount { get; private set; }
        public int[] InvalidHeaders => new int[]{};

        public bool IgnoreFirstRow { get; private set; }
    }


}