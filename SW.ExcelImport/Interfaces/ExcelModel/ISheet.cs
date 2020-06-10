namespace SW.ExcelImport
{

    public interface ISheet
    {
        ISheetContainer Parent { get; }
        int Index { get; }
        string[] HeaderMap { get;  }
        int[] InvalidMap { get;  }
        string Name { get; }
        bool Empty { get; }
        bool EmptyData { get; }
        bool InvalidName { get; }
        int RowCount { get; }
        bool Primary { get; }

    }
}