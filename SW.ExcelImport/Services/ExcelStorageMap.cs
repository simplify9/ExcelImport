using System.Linq;
using System.Collections.Generic;
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
        readonly ExcelRowParser<IExcelRowParseRequest,IExcelRowParseResult>  parser;
        readonly ExcelSheetOnTypeValidator sheetValidator;
        public ExcelStorageMap(ExcelRepo repo, ExcelRowParser<IExcelRowParseRequest,IExcelRowParseResult>  parser, ExcelSheetOnTypeValidator sheetValidator)
        {
            this.repo = repo;
            this.parser = parser;
            this.sheetValidator = sheetValidator;
        }
        
        public async Task Import(IExcelReader source, string url, ParseOptions options)
        {
            var (hasErrors, container) = await Load(source, url, options);
            if(hasErrors) return;

            
            do
            {
                repo.Add(source.Current, container.Sheets.FirstOrDefault(s => s.Index == source.CurrentSheet.Index));
                await repo.SaveChanges();
            } while (await source.ReadRow());
            
        }

        public async Task ImportAndParse(IExcelReader source, string url,  ParseOptions options)
        {
            var totalRowsWithErrors = 0;
            var (hasErrors, container) = await Load(source, url, options);
            if(hasErrors) return;


            do
            {
                if(totalRowsWithErrors >= options.MaxParseErrors)
                    return;

                var result = await parser.Parse(new ExcelRowParseRequest 
                { 
                    Options = options.SheetsOptions.FirstOrDefault(o => o.SheetIndex == source.CurrentSheet.Index),
                    Row = source.Current
                });

                var existing = source.Current as RowRecord;

                if(existing == null)
                    repo.Add(source.Current, container.Sheets.FirstOrDefault(s => s.Index == source.CurrentSheet.Index),result);
                else
                    existing.FillParseResult(result);
                
                if(result.HasErrors())
                    totalRowsWithErrors ++;
                
                await repo.SaveChanges();
            } while (await source.ReadRow());
            
        }


        public async Task<(bool, ISheetContainer)> Load(IExcelReader source, string url,  ParseOptions options)
        {
            await source.Load(url,options);

            var validationResult = new Dictionary<int,SheetValidationResult>();
            var sheets = source.Container.Sheets;

            foreach(var sheet in sheets)
            {
                var result = await sheetValidator.Validate(sheet, options);
                validationResult[sheet.Index] = result;

            }

            var excelRecord = await repo.CreateExcelFileRecordIfNotExists (source.Container, validationResult);

            var hasErrors = validationResult.Values.Any(r=> r.HasErrors) || sheets.Any(s => s.Empty || s.EmptyData);


            return (hasErrors  , excelRecord);

        }
    }
}