﻿using System.Collections.Generic;

namespace SW.ExcelImport
{

    public interface ISheetWithRows<T>: ISheet 
    {
        public  IEnumerable<IExcelRowParsed<T>> Rows { get; set; }
    }
    public interface ISheet
    {
        ISheetContainer Parent { get; }
        /// <summary>
        /// Starts with 1
        /// </summary>
        /// <value></value>
        int Index { get; }
        /// <summary>
        /// Always filled regardless if the sheet parsing should ignore first row
        /// </summary>
        /// <value></value>
        ICell[] Header { get;  }
        string Name { get; }
        bool Empty { get; }
        bool EmptyData { get; }
        int RowCount { get; }
        bool InvalidName { get;  } 
        int[] InvalidHeaders { get; }
        bool IgnoreFirstRow { get; }

    }
}