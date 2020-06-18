using System.ComponentModel.Design;
using System.Linq;
using System;
using SW.ExcelImport.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SW.ExcelImport.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SW.ExcelImport.Services
{
    public class ExcelRowTypeParser : ExcelRowParser<ExcelRowParseOnTypeRequest, ExcelRowParseResult>
    {
        readonly IExcelRepo repo;
        public ExcelRowTypeParser(IExcelRepo repo)
        {
            this.repo = repo;
        }
        public override async Task<ExcelRowParseResult> Parse(ExcelRowParseOnTypeRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

            var result = new ExcelRowParseResult();

            var idParseResult = ParseId(request);
            result.Populate(idParseResult);

            var cellsParseReult = ParseCells(request, idParseResult.ExcludeColumns);
            result.Populate(cellsParseReult);

            var idInStoreValidationResult = await ValidateIds(result, request);
            result.Populate(idInStoreValidationResult);

            return result;

        }

        public virtual IdParseResult ParseId(ExcelRowParseOnTypeRequest request)
        {

            var result = new IdParseResult
            {
                InvalidForeignIdValue = false,
                InvalidIdValue = false
            };
            var id = 0;

            var excludeColumns = new List<int>();
            var row = request.Row;

            var options = request.Options;


            if (options.IdIndex.HasValue)
            {
                excludeColumns.Add(options.IdIndex.Value);
                var val = request.Row.Cells[options.IdIndex.Value].Value?.ToString();
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
                var val = request.Row.Cells[options.ParentIdIndex.Value].Value?.ToString();
                if (int.TryParse(val, out id))
                    result.ForeignUserDefinedId = id;
                else
                    result.InvalidForeignIdValue = true;
            }


            return result;
        }

        public virtual CellsParseReult ParseCells(ExcelRowParseOnTypeRequest request, int[] excludeCells)
        {
            var result = new CellsParseReult();

            var invalidCells = new List<int>();
            var values = new Dictionary<string, object>();
            var row = request.Row;
            var sheet = row.Sheet as SheetRecord;
            Type parseOnType = null;
            var name = request.Options.SheetLongName ?? sheet.Name;
            var map = request.Options.Map;

            if (sheet.Index == 1)
                parseOnType = request.RootType;
            else
                parseOnType = PropertyPath.TryParse(request.RootType, name.ToPascalCase()).PropertyType;



            for (int i = 0; i < row.Cells.Length; i++)
            {
                if (excludeCells.Contains(i)) continue;

                var value = row.Cells[i];

                object castValue;

                var propertyPath = PropertyPath.TryParse(parseOnType, name.ToPascalCase());

                var convertSucceeded =
                    Converter.TryCreate(value, propertyPath.PropertyType, out castValue);

                if (convertSucceeded)
                {
                    if (castValue != null && castValue.GetType() == typeof(string) &&
                        (castValue as string) == string.Empty)
                    {
                        castValue = null;
                    }
                    values[name.ToPascalCase()] = castValue;
                }
                else
                {
                    invalidCells.Add(i);
                }
            }

            var contractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            };
            var settings = new JsonSerializerSettings
            {
                ContractResolver = contractResolver,
            };

            if (invalidCells.Count == 0)
                result.RowAsJson = JsonConvert.SerializeObject(parseOnType.CreateFromDictionary(values), settings);
            else result.InvalidCells = invalidCells.ToArray();

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
                var found = await repo.RowRecordExists(request.Row.Sheet.Parent.Sheets.FirstOrDefault(x => x.Index == 1)
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