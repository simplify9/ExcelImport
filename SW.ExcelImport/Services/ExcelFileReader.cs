using System.ComponentModel.Design;
using System.Xml.Linq;
using System.IO;
using System;
using SW.PrimitiveTypes;
using System.Threading.Tasks;
using ExcelDataReader;
using System.Linq;
using System.Collections.Generic;

namespace SW.ExcelImport.Services
{
    
    public class ExcelFileReader : IExcelReader , IDisposable
    {
        private IExcelDataReader reader;
        private Stream stream;
        private MemoryStream memoryStream;
        //private int sheetIndex = 1;
        private int index = 0;
        private bool loaded;
        private string url;
        readonly ICloudFilesService cloudFilesService;

        public ExcelFileReader(ICloudFilesService cloudFilesService)
        {
            this.cloudFilesService = cloudFilesService;
        }        
        

        public async Task<ISheetContainer> Load(string url, ICollection<SheetMappingOptions> sheetsOptions)
        {
            this.url = url;
            using var tmpFileStream = await cloudFilesService.OpenReadAsync(url);
            var tmpStream = new MemoryStream();
            await tmpFileStream.CopyToAsync(tmpStream);

            using var tmpReader = ExcelReaderFactory.CreateReader(tmpStream); 

            
            
            if(tmpReader.ResultsCount == 0) return null;

            var sheets = new ISheet[tmpReader.ResultsCount];

            var result = new SheetContainer(url, sheets);

            for (int i = 0; i < sheets.Length; i++)
            {
                var ignoreHeader = true;
                var options = sheetsOptions?.FirstOrDefault(o => o.SheetIndex == i) ?? SheetMappingOptions.Default(i);
                if(options.Map != null)
                    ignoreHeader = false;
                tmpReader.Read();
                var sheet = new Sheet(tmpReader, result, i,ignoreHeader);
                result.Sheets[i] = sheet;
                tmpReader.NextResult();
            }
            Container = result;
            CurrentSheet = Container.Sheets[0];



            return result;
            
        }

        public ISheet CurrentSheet { get; private set; }

        public IExcelRow Current { get; private set; }

        public ISheetContainer Container { get; private set; }


        public void Dispose()
        {
            if(stream != null)
                stream.Dispose();
            if(reader != null)
                reader.Dispose();
        }

        public async Task<bool> ReadRow()
        {
            if (!loaded)
            {
                stream = await cloudFilesService.OpenReadAsync(url);
                memoryStream = new MemoryStream();
                await stream.CopyToAsync(memoryStream);
                reader = ExcelReaderFactory.CreateReader(memoryStream);
                loaded = true;
            }

            var found = reader.Read();


            if (!found)
            {
                if(reader.ResultsCount == CurrentSheet.Index + 1)
                    return false;
                CurrentSheet = Container.Sheets[CurrentSheet.Index + 1];
                reader.NextResult();
                index = 0;
                return await ReadRow();
            }
            index++;
            Current = new ExcelRow(index, CurrentSheet, reader);
            return true;

        }


        
    }
}