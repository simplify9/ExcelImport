// using ExcelDataReader;
// using System;

// namespace SW.ExcelImport
// {
//     public class Sheet : ISheet
//     {

//         private Sheet()
//         {

//         }

//         public Sheet(IExcelDataReader reader, ISheetContainer parent, int index,ICell[] header, bool ignoreHeader)
//         {
//             if(reader == null) throw new ArgumentNullException(nameof(reader));

//             Parent = parent ?? throw new ArgumentNullException(nameof(parent));
//             Header = header;
//             Index = index;

//             Name = reader.Name;
//             RowCount = reader.RowCount;
//             Index = index;
//             if(RowCount == 0)
//                 Empty = true;
            
//             if(ignoreHeader && RowCount <= 1 )
//                 EmptyData = true;

//             if(!ignoreHeader && RowCount == 0 )
//                 EmptyData = true;

//         }
//         public ISheetContainer Parent { get; private set; }
//         public int Index { get; private set; }
//         public ICell[] Header{ get; private set; }
//         public string[] InvalidColumns { get; private set; }
//         public string Name { get; private set; }

//         public bool Empty { get; private set; }

//         public bool EmptyData { get; private set; }
//         public bool InvalidName { get; private set; }

//         public int RowCount { get; private set; }

//         public bool Primary { get; private set; }

//         public int[] InvalidMap { get; private set; }
//     }


// }