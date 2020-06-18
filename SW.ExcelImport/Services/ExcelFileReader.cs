using System.Xml.Linq;
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
        private TypedParseToJsonOptions options;
        private bool loaded;
        
        public ExcelFileReader(ICloudFilesService cloudFilesService)
        {
            this.cloudFilesService = cloudFilesService;
        }

        public async Task Load(string url,  TypedParseToJsonOptions options)
        {

            if (reader == null)
            {
                stream = await cloudFilesService.OpenReadAsync(url);
                reader = ExcelReaderFactory.CreateReader(stream);
            }
            else
            {
                reader.Reset();
            }

            loaded = true;
            this.options = options;
            
            CurrentSheet = Container.Sheets[0];

        }


        public ISheet CurrentSheet { get; private set; }

        public IExcelRow Current { get; private set; }

        public ISheetContainer Container { get; private set; }


        public void Dispose()
        {
            stream.Dispose();
            reader.Dispose();
        }

        public Task<bool> ReadRow()
        {
            if (!loaded)
                throw new InvalidOperationException("Excel sheet not loaded");

            var found = reader.Read();


            if (!found)
            {
                if(reader.ResultsCount == CurrentSheet.Index)
                    return Task.FromResult(false);
                CurrentSheet = Container.Sheets[CurrentSheet.Index];
                reader.NextResult();
                return ReadRow();
            }

            Current = new ExcelRow(index, CurrentSheet, reader);
            return Task.FromResult(true);

        }

        public Task<bool> ReadRow(int sheetIndex, int startOnRow)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ReadRow(string sheetName, int startOnRow)
        {
            throw new NotImplementedException();
        }
    }
}