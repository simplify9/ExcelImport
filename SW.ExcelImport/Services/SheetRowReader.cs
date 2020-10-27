using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SW.ExcelImport.Services
{
    public class SheetRowReader<T> where T : new()
    {
        readonly IExcelReader reader;
        readonly ExcelSheetOnTypeValidator sheetValidator;
        readonly ExcelRowTypeParser parser;
        private bool loaded = false;
        private SheetValidationResult sheetValidationResult;
        private JsonNamingStrategy namingStrategy;
        private bool ignoreFirst;
        private string[] map;
        public SheetRowReader(IExcelReader reader, ExcelSheetOnTypeValidator sheetValidator, ExcelRowTypeParser parser)
        {
            this.reader = reader;
            this.sheetValidator = sheetValidator;
            this.parser = parser;
        }

        public async Task<SheetValidationResult> Load(string url, int sheetIndex = 0,
            JsonNamingStrategy jsonNamingStrategy = JsonNamingStrategy.None,
            string[] headerMap = null, bool ignoreFirstRow = true) 
        {
            loaded = true;
            ignoreFirst = ignoreFirstRow;
            namingStrategy = jsonNamingStrategy;
            map = headerMap;
            var sheet = await reader.LoadSheet(url, sheetIndex, map);
            
            if (sheet == null)
                return SheetValidationResult.SheetNotPresent();
            
            var request = new SheetOnTypeParseRequest
            {
                MappingOptions = new SheetMappingOptions
                {
                    Ignore = ignoreFirst,
                    Map = headerMap,
                    IndexAsId = true
                },
                NamingStrategy = namingStrategy,
                RootType =  typeof(T),
                Sheet = sheet
            };
            
            sheetValidationResult=  await sheetValidator.Validate(request);
            return sheetValidationResult;

        }

        public async Task<(bool,RowParseResultTyped<T>)> Read()
        {
            if(!loaded)
                throw new InvalidOperationException("Sheet not loaded. Call the load sheet first");

            if(sheetValidationResult.HasErrors)
                throw new InvalidOperationException("Sheet is invalid");
            
            
            var found = await reader.ReadRow();
            if(reader.Current.Index == 1 && ignoreFirst)
                found = await reader.ReadRow();

            if (!found)
                return (false, null);
            
            var result = new RowParseResultTyped<T>();
            
            var invalidCells = new List<int>();
            var values = new Dictionary<string, object>();
            var row = reader.Current;
            var sheet = row.Sheet;
            var parseOnType = typeof(T);
            
            var headerMap = map ?? row.Sheet.Header.Select(x => x.Value.ToString()).ToArray() ;
            
            for (var i = 0; i < row.Cells.Length; i++)
            {

                var value = row.Cells[i].Value;
                var propertyName = headerMap[i].Transform(namingStrategy);
                object castValue;

                var propertyPath = PropertyPath.TryParse(parseOnType, propertyName);

                var convertSucceeded =
                    Converter.TryCreate(value, propertyPath.PropertyType, out castValue);
    
                if (convertSucceeded)
                {
                    if (castValue != null && castValue.GetType() == typeof(string) &&
                        (castValue as string) == string.Empty)
                        castValue = null;
                    values[propertyName] = castValue;
                }
                else
                    invalidCells.Add(i);
            }
            
            result.InvalidCells = invalidCells.ToArray();
            
            if (invalidCells.Count == 0)
                result.RowMapped = (T)parseOnType.CreateFromDictionary(values);

            return (true, result);
        }

        public async Task<ICollection<RowParseResultTyped<T>>> ReadAll()
        {
            var results = new List<RowParseResultTyped<T>>();
            var found = false;
            do
            {
                var (hasResult, result) = await Read();
                found = hasResult;
                if(found) results.Add(result);
            } while (found);

            return results;
        }
        
    }
}