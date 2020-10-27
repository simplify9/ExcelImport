using System.ComponentModel.Design;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SW.ExcelImport.Entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SW.ExcelImport.Services
{
    
    public class ExcelRowTypeParser : ExcelRowParser<ExcelRowParseOnTypeRequest, ExcelRowParseResult>
    {
        readonly ExcelRepo repo;
        public ExcelRowTypeParser(ExcelRepo repo)
        {
            this.repo = repo;
        }
        public override async Task<ExcelRowParseResult> Parse(ExcelRowParseOnTypeRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new ExcelRowParseResult();

            var idParseResult = ParseId(request.Row,request.Options);
            result.Populate(idParseResult);

            var cellsParseResult = ParseCells(request, idParseResult.ExcludeColumns);
            result.Populate(cellsParseResult);

            var idInStoreValidationResult = await ValidateIds(result, request);
            result.Populate(idInStoreValidationResult);

            return result;

        }

        public virtual IdParseResult ParseId(IExcelRow row, SheetMappingOptions options)
        {

            var result = new IdParseResult
            {
                InvalidForeignIdValue = false,
                InvalidIdValue = false
            };
            var id = 0;

            var excludeColumns = new List<int>();
            


            if (options.IdIndex.HasValue)
            {
                excludeColumns.Add(options.IdIndex.Value);
                var val = row.Cells[options.IdIndex.Value].Value?.ToString();
                if (int.TryParse(val, out id))
                    result.UserDefinedId = id;
                else
                    result.InvalidIdValue = true;
            }

            if (options.IndexAsId)
            {
                result.UserDefinedId = row.Index;
                result.InvalidIdValue = false;
            }

            if (options.ParentIdIndex.HasValue)
            {
                excludeColumns.Add(options.ParentIdIndex.Value);
                var val = row.Cells[options.ParentIdIndex.Value].Value?.ToString();
                if (int.TryParse(val, out id))
                    result.ForeignUserDefinedId = id;
                else
                    result.InvalidForeignIdValue = true;
            }

            result.ExcludeColumns = excludeColumns.ToArray();
            return result;
        }

        public virtual CellsParseReult ParseCells(ExcelRowParseOnTypeRequest request, int[] excludeCells)
        {
            var result = new CellsParseReult();

            var invalidCells = new List<int>();
            var values = new Dictionary<string, object>();
            var row = request.Row;
            var sheet = row.Sheet;
            Type parseOnType = null;
            var name = request.Options.SheetName ?? sheet.Name;
            var map = request.Options.Map ?? row.Sheet.Header.Select(x => x.Value.ToString()).ToArray() ;
            var strategy = request.NamingStrategy;

            parseOnType = sheet.Index == 0 ? request.RootType : request.RootType.GetEnumerablePropertyType(name, strategy);

            for (var i = 0; i < row.Cells.Length; i++)
            {
                if (excludeCells.Contains(i)) continue;

                var value = row.Cells[i].Value;
                var propertyName = map[i].Transform(strategy);
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
                result.RowMapped = JsonConvert.SerializeObject(parseOnType.CreateFromDictionary(values), 
                    JsonUtil.GetSettings(request.NamingStrategy));

            return result;

        }

        public virtual async Task<IdInStoreValidationResult> ValidateIds(IExcelRowParseResult parseResult, IExcelRowParseRequest request)
        {
            var result = new IdInStoreValidationResult();

            if (!parseResult.InvalidIdValue.Value)
            {
                var found = await repo.RowRecordExists(request.Row.Sheet, parseResult.UserDefinedId.Value);
                if (found.HasValue)
                    result.IdDuplicate = true;
            }

            if (parseResult.ForeignUserDefinedId.HasValue)
            {
                var found = await repo.RowRecordExists(request.Row.Sheet.Parent.Sheets.FirstOrDefault(x => x.Index == 0)
                    , parseResult.ForeignUserDefinedId.Value);
                if (!found.HasValue)
                    result.ForeignIdNotFound = true;
                else
                    result.ForeignId = found.Value;
            }

            return result;
        }


    }
}