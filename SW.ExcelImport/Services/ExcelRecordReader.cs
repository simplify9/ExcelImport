using System;
using SW.ExcelImport.Model;
using System.Threading.Tasks;

namespace SW.ExcelImport.Services
{
    public class ExcelRecordReader : IExcelReader
    {
        
        private ParseOptions options;
        private bool loaded;
        readonly IExcelRepo repo;

        public ExcelRecordReader(IExcelRepo repo)
        {
            this.repo = repo;
        }
        
        public  Task Load(string url, Type onType, ParseOptions options)
        {
            return Task.CompletedTask;
        }

        public ISheet CurrentSheet { get; private set; }
        public ISheet MainSheet { get; private set; }
        public IExcelRow Current { get; private set; }

        public ISheetContainer Container { get; private set; }

        

        public void Dispose()
        {
            
        }

        public Task<bool> ReadRow()
        {
            return Task.FromResult(true);
        }
    }
}