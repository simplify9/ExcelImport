// using System;
// using ExcelDataReader;

// namespace SW.ExcelImport
// {
//     public class SheetContainer : ISheetContainer
//     {

//         public SheetContainer(string reference, IExcelDataReader reader, bool ignoreHeder)
//         {
//             this.Reference = reference ?? throw new ArgumentNullException(nameof(reference));
            
//             Sheets = new ISheet[reader.ResultsCount];

//             Sheets[0] = new Sheet(reader,this, 1,reader.GetCells(),ignoreHeder);

//             var index = 1;
//             var sheetIndex = 2;
//             while (reader.NextResult())
//             {
//                 Sheets[index] = new Sheet(reader,this, 1,reader.GetCells(),ignoreHeder);
//                 index ++;
//                 sheetIndex ++;
//             }

//             reader.Reset();
//         }
//         public string Reference { get; }

//         public int SheetCount { get; }

//         public ISheet[] Sheets { get; }
//     }


// }