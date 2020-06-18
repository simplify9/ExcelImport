using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using SW.ExcelImport.Model;
using System.Threading.Tasks;
using SW.ExcelImport.Domain;

namespace SW.ExcelImport.Services
{
    public class ExcelValidator : IExcelValidator
    {
        readonly ExcelRepo repo;
        readonly ExcelRowValidator<ExcelRowValidateOnTypeRequest, ExcelRowValidationResult> validator;
        public ExcelValidator(ExcelRepo repo, ExcelRowValidator<ExcelRowValidateOnTypeRequest, ExcelRowValidationResult> validator)
        {
            this.repo = repo;
            this.validator  = validator;
        }
        public async Task Process(string reference, TypedParseToJsonOptions parseOptions)
        {
            var count = await repo.GetParsedOkCount(reference);
            for (int i = 0; i <  (count / 10) + 1 ; i++)
            {
                var rows = await repo.GetParsedOk(reference, i *10 , 10);
                foreach (var row in rows)
                {
                    var result = await validator.Validate(
                        new ExcelRowValidateOnTypeRequest( row,parseOptions.OnType, parseOptions.NamingStrategy));
                    row.FillData(result.Data);
                }
                await repo.SaveChanges();
            }
        }
    }
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
        
        public async Task Import(IExcelReader source, string url, TypedParseToJsonOptions options)
        {
            var (hasErrors, container) = await Load(source, url, options);
            if(hasErrors) return;

            
            do
            {
                repo.Add(source.Current, container.Sheets.FirstOrDefault(s => s.Index == source.CurrentSheet.Index));
                await repo.SaveChanges();
            } while (await source.ReadRow());
            
        }

        public async Task ImportAndParse(IExcelReader source, string url,  TypedParseToJsonOptions options)
        {
            var totalRowsWithErrors = 0;
            
            
            var (hasErrors, container) = await Load(source, url, options);

            if(hasErrors) return;


            do
            {
                if(totalRowsWithErrors >= options.MaxParseErrors)
                    return;

                var result = await parser.Parse(new ExcelRowParseOnTypeRequest 
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


        public async Task<(bool, ISheetContainer)> Load(IExcelReader source, string url,  TypedParseToJsonOptions options)
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