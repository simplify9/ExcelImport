using System.Data.Common;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using System.Xml.Linq;
using System;
using System.Threading.Tasks;
using SW.ExcelImport.Entity;

namespace SW.ExcelImport.Services
{
    
    public class ExcelService 
    {
        readonly ExcelRepo repo;
        readonly IExcelReader reader;
        readonly ExcelFileTypedParseToJsonLoader loader;
        readonly ExcelRowTypeParser parser;
        readonly ExcelRowTypeValidatorOnAnnotations validator;
        public ExcelService(ExcelRepo repo, IExcelReader reader, 
            ExcelFileTypedParseToJsonLoader loader, ExcelRowTypeParser parser,
            ExcelRowTypeValidatorOnAnnotations validator)
        {
            this.repo = repo;
            this.reader = reader;
            this.loader = loader;
            this.parser = parser;
            this.validator = validator;
        }
        
        public async Task<ISheetContainer> LoadExcelFileInfo(string url, TypedParseToJsonOptions options)
        {
            var result = await loader.Load(url, options);
            return result;
        }
        public async Task Import(string url, TypedParseToJsonOptions options)
        {
            
            var container = await loader.Load(url, options);

            if(container.SheetRecords.Any(x => x.HasErrors()))
                throw new InvalidOperationException("Excel sheet invalid");
            
            await repo.AddExcelSheetFile(container);
            
            var totalRowsWithErrors = 0;
            var totalRowsWithValidationErrors = 0;

            while (await reader.ReadRow())
            {

                if(reader.Current.Sheet.IgnoreFirstRow && reader.Current.Index == 1) 
                    continue;

                if(totalRowsWithErrors >= options.MaxParseErrors && options.MaxParseErrors !=0)
                    return;
                if(totalRowsWithValidationErrors >= options.MaxValidationErrors && options.MaxValidationErrors !=0)
                    return;
                
                var parseRequest = options.RowParseRequest(reader.Current) as ExcelRowParseOnTypeRequest;
                
                var result = await parser.Parse( parseRequest) ;
                
                if(result.HasErrors())
                    totalRowsWithErrors ++;
                
                var sheet= container.Sheets.FirstOrDefault(s => s.Index == reader.Current.Sheet.Index);
                var rowAdded =  repo.Add(reader.Current,sheet,result );

                if(container.Sheets.Length == 1 && !result.HasErrors())
                {
                    var validationResult = await validator.Validate(new ExcelRowValidateOnTypeRequest(rowAdded, options.OnType(),options.NamingStrategy));
                    rowAdded.FillData(validationResult.Data, validationResult.IsValid.Value, validationResult.ValidationErrors);
                    if(!validationResult.IsValid.Value)
                        totalRowsWithValidationErrors ++;
                }

                await repo.SaveChanges();
            };
            
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
                        new ExcelRowValidateOnTypeRequest( row,parseOptions.OnType(), parseOptions.NamingStrategy));
                    
                    row.FillData(result.Data, result.IsValid.Value, result.ValidationErrors);
                }
                await repo.SaveChanges();
            }
        }
    }
}