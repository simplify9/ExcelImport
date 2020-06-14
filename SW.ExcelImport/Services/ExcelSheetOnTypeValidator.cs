using System.Linq;
using System.Collections.Generic;
using System;
using SW.ExcelImport.Model;
using System.Threading.Tasks;

namespace SW.ExcelImport.Services
{
    public class ExcelSheetOnTypeValidator 
    {
        public Task<SheetValidationResult> Validate(ISheet sheet, ParseOptions options)
        {
            if (sheet.Empty || sheet.EmptyData)
                return Task.FromResult(new SheetValidationResult(false, new int[] { },null,false));

            var invalidName = false;
            var invalidHeaders = new List<int>();
            Type type = null;

            var sheetOptions = GetOptions(sheet, options);
            var (ignoreFirstRow,headerMap) = GetMap(sheet, sheetOptions);

            var propertyName = sheetOptions.SheetLongName ?? sheet.Name;

            if (sheet.Index == 1)
            {
                type = options.OnType;
                invalidName = false;
            }
            else
            {
                type = GetType(sheet, sheetOptions, options);
                invalidName = type == null;
            }

            var invalidMap = type?.ParsePayloadMap(headerMap) ?? new int[] { };

            return Task.FromResult(new SheetValidationResult(invalidName, invalidHeaders.ToArray(),type, ignoreFirstRow ));

        }

        private SheetMappingOptions GetOptions(ISheet sheet, ParseOptions options) =>
            options.SheetsOptions.FirstOrDefault(o => o.SheetIndex == sheet.Index) ??
                 SheetMappingOptions.Default(sheet.Index);
        private (bool,string[]) GetMap(ISheet sheet, SheetMappingOptions options) =>
            (options.Map !=null, options.Map ?? sheet.Header.Select(x => x.Value.ToString()).ToArray());

        private Type GetType(ISheet sheet, SheetMappingOptions mappingOptions, ParseOptions options)
        {
            var propertyName = mappingOptions.SheetLongName ?? sheet.Name;

            if (propertyName == null)
                return null;

            var propertyPath = PropertyPath.TryParse(options.OnType, propertyName.ToPascalCase());
            if (propertyPath == null)
                return null;
            else
                return propertyPath.PropertyType;




        }

    }
}