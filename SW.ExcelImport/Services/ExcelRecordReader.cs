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
        
        public  Task Load(string url,  ParseOptions options)
        {
            return Task.CompletedTask;
        }

        public ISheet CurrentSheet { get; private set; }
        public IExcelRow Current { get; private set; }

        public ISheetContainer Container { get; private set; }

        public ISheet MainSheet => throw new NotImplementedException();

        public void Dispose()
        {
            
        }

        public Task<bool> ReadRow()
        {
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