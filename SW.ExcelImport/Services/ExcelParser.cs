// using System.Runtime.InteropServices;
// using System.Xml.Linq;
// using System;
// using System.Diagnostics;
// using SW.PrimitiveTypes;
// using SW.ExcelImport.Model;
// using System.Text;
// using System.Threading.Tasks;
// using ExcelDataReader;
// using System.Collections.Generic;
// using System.Linq;
// using System.Globalization;
// using System.Data;
// using Microsoft.EntityFrameworkCore;
// using SW.ExcelImport.Domain;

// namespace SW.ExcelImport.Services
// {

//     public class ExcelParser
//     {
//         readonly ExcelRepo repo;
//         readonly ICloudFilesService cloudFilesService;
//         public ExcelParser(ExcelRepo repo, ICloudFilesService cloudFilesService)
//         {
//             this.repo= repo;
//             this.cloudFilesService = cloudFilesService;
//         }

//         public async Task Parse(string url, Type payloadType, ParseOptions options)
//         {
//             var totalErrors = 0;
//             var mainRecordIndex = 0;
//             var sheetIndex = 0;
//             var excelRecord = await repo.AddExcelFileIfNotFound(url);
//             string[] headerMap = null;
//             using (var stream = (await cloudFilesService.OpenReadAsync(url)))
//             {
//                 using (var reader = ExcelReaderFactory.CreateReader(stream))
//                 {
//                     if(options.HeaderInFirstRow)
//                     {
//                         if(reader.RowCount == 0)
//                         {
//                             var emptySheet = ExcelSheet.EmptySheet(excelRecord,reader.Name, sheetIndex);
//                             await repo.AddSheetIfNotFound(emptySheet);
//                             return;
//                         }
//                         reader.Read();

//                         mainRecordIndex +=1;
//                         var row = new ExcelRow(0,reader);
//                         headerMap = row.Cells.Select(c=> c.Value.ToString()).ToArray();
//                         if(reader.RowCount == 1)
//                         {
//                             var emptySheet = ExcelSheet.EmptyRecords(excelRecord,headerMap,reader.Name, sheetIndex);
//                             await repo.AddSheetIfNotFound(emptySheet);
//                             return;
//                         }
//                     }
//                     else
//                     {
//                         headerMap = options.SheetsMap[0].Map;
//                         if(reader.RowCount == 0)
//                         {
//                             var emptySheet = ExcelSheet.EmptySheet(excelRecord,reader.Name, sheetIndex);
//                             await repo.AddSheetIfNotFound(emptySheet);
//                             return;
//                         }
//                     }

//                     var invalidColumns = payloadType.ParsePayloadMap(headerMap);
//                     if(invalidColumns.Length > 0)
//                     {
                        
//                         var invalidMainSheet = ExcelSheet.InvalidHeader(excelRecord,headerMap,invalidColumns,reader.Name,sheetIndex);
//                         await repo.AddSheetIfNotFound(invalidMainSheet);
//                             return;
//                     }
                    
//                     var mainSheet = new ExcelSheet(excelRecord,headerMap,reader.Name,sheetIndex);
//                     await repo.AddSheetIfNotFound(mainSheet);

//                     while (reader.Read())
//                     { 
//                         if(totalErrors >= options.MaxParseErrors)
//                             return;
                        
//                         var row = new ExcelRow(mainRecordIndex,reader);
//                         var result =payloadType.TryParsePayload(new TypeParseRequest
//                         {
//                             MainSheet = true,
//                             MainSheetIdColumn = options.MainSheetIdColumn ?? 0,
//                             MainSheetIndexAsId = options.MainSheetIndexAsId,
//                             Map = mainSheet.HeaderMap,
//                             Row = row
//                         });
//                         var parsed = new ExcelRowParsed(row,result);
//                         if(result.HasError)

//                     }
                    
                    

//                 }
//             }
//         }
    
//         public async Task<bool> ParseRow(int index, )
//     }
// }