using System.IO;
using System;
using SW.PrimitiveTypes;
using SW.ExcelImport.Model;
using System.Threading.Tasks;
using ExcelDataReader;
using System.Linq;

namespace SW.ExcelImport.Services
{
    public class ExcelFileReader : IExcelReader
    {
        readonly ICloudFilesService cloudFilesService;
        private IExcelDataReader reader;
        private Stream stream;
        private int sheetIndex = 1;
        private int index = 1;
        private ParseOptions options;
        private bool loaded;
        private Type onType;
        public ExcelFileReader(ICloudFilesService cloudFilesService)
        {
            this.cloudFilesService = cloudFilesService;
        }
        
        public async Task Load(string url,Type onType, ParseOptions options)
        {
            
            if(stream != null || reader !=null)
                Dispose();
            loaded = true;
            this.options = options;
            this.onType = onType;
            Container = new SheetContainer(url, reader.ResultsCount);

            stream = await cloudFilesService.OpenReadAsync(url);
            reader = ExcelReaderFactory.CreateReader(stream);

            LoadSheetInformation(true);
            
        }

        private void LoadSheetInformation(bool primary = false)
        {
            
            if(reader.RowCount == 0)
            {
                CurrentSheet = Sheet.EmptySheet(Container,reader.Name, sheetIndex,primary);
                return;
            }

            string[] headerMap = null; 
            if(options.HeaderInFirstRow)
            {
                
                reader.Read();
                index += 1;
                var row = new ExcelRow(0,reader);
                headerMap = row.Cells.Select(c=> c.Value.ToString()).ToArray();
            }
            else
            {
                headerMap = options.SheetsMap[0].Map;
            }

            if( options.HeaderInFirstRow && reader.RowCount == 1 )
                CurrentSheet = Sheet.EmptyRecordsSheet(Container,headerMap,reader.Name, sheetIndex,primary);
            else
            {
                var type = primary ? onType : onType.GetEnumerablePropertyType(reader.Name);
                if(type == null)
                {
                    CurrentSheet = Sheet.InvalidNameSheet(Container,reader.Name,sheetIndex);
                }
                else
                {
                    var invalidMap = type.ParsePayloadMap(headerMap);
                    if(invalidMap.Length > 0)
                        CurrentSheet = new Sheet(Container, headerMap,reader.Name,sheetIndex,reader.RowCount -1 ,primary );
                    else
                        CurrentSheet = Sheet.InvalidMapSheet(Container,headerMap,reader.Name, sheetIndex,primary,invalidMap);
                }
                
            }
        }

        public ISheet CurrentSheet { get; private set; }
        public ISheet MainSheet { get; private set; }

        public IExcelRow Current { get; private set; }

        public ISheetContainer Container { get; private set; }


        public void Dispose()
        {
            stream.Dispose();
            reader.Dispose();
        }

        public Task<bool> ReadRow()
        {
            if(!loaded)
                throw new InvalidOperationException("Excel sheet not loaded");

            if(CurrentSheet.InvalidMap.Length > 0 || CurrentSheet.EmptyData )
            {
                if(CurrentSheet.Primary)
                    return Task.FromResult(false); 
            }
                
            
            var found = reader.Read();
            
            index +=1;

            if(!found && sheetIndex  == Container.SheetCount)
            {
                index = 1;
                return Task.FromResult(false);   
            }
                
            
            if(!found)
            {
                reader.NextResult();
                sheetIndex +=1;

                index = 1;
                LoadSheetInformation();
                return ReadRow();
            }

            Current = new ExcelRow(index,reader);
            return Task.FromResult(true);

        }
    }
}