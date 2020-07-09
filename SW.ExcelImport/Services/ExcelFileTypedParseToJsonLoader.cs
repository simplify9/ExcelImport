using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using SW.PrimitiveTypes;
using SW.ExcelImport.Entity;

namespace SW.ExcelImport.Services
{
    public class ExcelFileTypedParseToJsonLoader: ExcelFileLoader<TypedParseToJsonOptions, ExcelFileRecord>
    {
        readonly ExcelSheetOnTypeValidator sheetValidator;
        readonly IExcelReader reader;
        public ExcelFileTypedParseToJsonLoader(ExcelSheetOnTypeValidator sheetValidator, IExcelReader reader)
        {
            this.sheetValidator = sheetValidator;
            this.reader = reader;
        }

        

        public override async Task<ExcelFileRecord> Load(string url, TypedParseToJsonOptions options)
        {
            var container = await reader.Load(url, options.SheetsOptions);
            var validationResult = new Dictionary<int,SheetValidationResult>();
            var sheets = container.Sheets;
            var onType  = Type.GetType(options.AssemblyQualifiedTypeName);
            foreach(var sheet in sheets)
            {
                var sheetOptions = GetOptions(sheet, options);
                var request = new SheetOnTypeParseRequest
                {
                    LongName = sheetOptions?.SheetName,
                    MappingOptions = sheetOptions,
                    NamingStrategy = options.NamingStrategy,
                    RootType =  options.OnType(),
                    Sheet = sheet
                };
                var result = await sheetValidator.Validate(request);
                validationResult[sheet.Index] = result;

            }

            return new ExcelFileRecord(url, sheets, validationResult, options);
        }
        
        private SheetMappingOptions GetOptions(ISheet sheet, TypedParseToJsonOptions options) =>
            options.SheetsOptions?.FirstOrDefault(o => o.SheetIndex == sheet.Index) ??
                 SheetMappingOptions.Default(sheet.Index);
    }
}