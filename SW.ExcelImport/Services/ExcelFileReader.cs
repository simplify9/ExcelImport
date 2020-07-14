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
        //private int sheetIndex = 1;
        private int index = 0;
        private bool loaded;
        readonly ICloudFilesService cloudFilesService;

        public ExcelFileReader(ICloudFilesService cloudFilesService)
        {
            this.cloudFilesService = cloudFilesService;
        }        
        

        public async Task<ISheetContainer> Load(string url, ICollection<SheetMappingOptions> sheetsOptions)
        {
            
            stream = await cloudFilesService.OpenReadAsync(url);
            reader = ExcelReaderFactory.CreateReader(stream);

            loaded = true;
            
            if(reader.ResultsCount == 0) return null;

            var sheets = new ISheet[reader.ResultsCount];

            var result = new SheetContainer(url, sheets);

            for (int i = 0; i < sheets.Length; i++)
            {
                var ignoreHeader = true;
                var options = sheetsOptions?.FirstOrDefault(o => o.SheetIndex == i) ?? SheetMappingOptions.Default(i);
                if(options.Map != null)
                    ignoreHeader = false;
                reader.Read();
                var sheet = new Sheet(reader, result, i,ignoreHeader);
                result.Sheets[i] = sheet;
                reader.NextResult();
            }
            Container = result;
            CurrentSheet = Container.Sheets[0];
            reader.Reset();

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

        public Task<bool> ReadRow()
        {
            if (!loaded)
                throw new InvalidOperationException("Excel sheet not loaded");

            var found = reader.Read();


            if (!found)
            {
                if(reader.ResultsCount == CurrentSheet.Index + 1)
                    return Task.FromResult(false);
                CurrentSheet = Container.Sheets[CurrentSheet.Index + 1];
                reader.NextResult();
                index = 0;
                return ReadRow();
            }
            index++;
            Current = new ExcelRow(index, CurrentSheet, reader);
            return Task.FromResult(true);

        }


        
    }
}