using System.Xml.Linq;
using System;
using SW.ExcelImport.Model;
using System.Threading.Tasks;
using SW.ExcelImport.Domain;

namespace SW.ExcelImport.Services
{
    public class ExcelStorageMap : IExcelStorageMap
    {
        readonly ExcelRepo repo;
        readonly IExcelRowParser parser;

        public ExcelStorageMap(ExcelRepo repo, IExcelRowParser parser)
        {
            this.repo = repo;
            this.parser = parser;
        }
        
        public async Task Import(IExcelReader source, string url, Type onType, ParseOptions options)
        {
            await source.Load(url,onType,options);
            var excelRecord = await repo.CreateExcelFileRecordIfNotExists (source.Container);
            do
            {
                repo.Add(source.Container, source.CurrentSheet, source.Current);
                await repo.SaveChanges();
            } while (await source.ReadRow());
            
        }

        public async Task ImportAndParse(IExcelReader source, string url, Type onType, ParseOptions options)
        {
            await source.Load(url,onType,options);
            
            var excelRecord = await repo.CreateExcelFileRecordIfNotExists (source.Container);
            do
            {
                var parsed = await parser.Parse(new ExcelRowParseRequest 
                { 
                    IsMainSheet = source.CurrentSheet.Primary,
                    MainSheet = source.MainSheet,
                    Sheet = source.CurrentSheet,
                    MainSheetIdColumn = options.MainSheetIdColumn,
                    MainSheetIndexAsId = options.MainSheetIndexAsId,
                    Row = source.Current
                }, onType);

                var existing = source.Current as RowRecord;

                if(existing == null)
                {
                    
                }
                else
                {

                }
                repo.Add(source.Container, source.CurrentSheet, source.Current);
                await repo.SaveChanges();
            } while (await source.ReadRow());
            
        }
    }
}