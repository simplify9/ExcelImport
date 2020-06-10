using System.Linq;
using System;
using SW.ExcelImport.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SW.ExcelImport.Domain;

namespace SW.ExcelImport.Services
{
    public class ExcelRowParser : IExcelRowParser
    {
        readonly IExcelRepo repo;
        public ExcelRowParser(IExcelRepo repo)
        {
            this.repo = repo;
        }
        public virtual async Task<IExcelRowParseResult> Parse(IExcelRowParseRequest request, Type onType)
        {
            if (onType == null) throw new ArgumentNullException(nameof(onType));
            if (request == null) throw new ArgumentNullException(nameof(request));
            
            var result = new ExcelRowParseResult();
            
            var idParseResult = ParseId(request);
            result.Populate(idParseResult);
            
            var cellsParseReult = ParseCells(request, idParseResult.ExcludeColumns, onType);
            result.Populate(cellsParseReult);

            var idInStoreValidationResult = await ValidateIds(result,request);

            return result;
            
        }

        public virtual IdParseResult ParseId(IExcelRowParseRequest request)
        {

            var result = new IdParseResult();
            var id = 0;

            var excludeColumns = new List<int>();


            if(request.IsMainSheet && !request.MainSheetIndexAsId)
            {
                excludeColumns.Add(request.MainSheetIdColumn.Value);
                var val = request.Row.Cells[request.MainSheetIdColumn.Value].Value.ToString();
                if(int.TryParse(val,out id))
                    result.UserDefinedId = id;
                else
                    result.InvalidIdValue = true;
            }
            else
            {
                result.UserDefinedId = request.Row.Index;
                if(!request.IsMainSheet)
                {
                    result.ExcludeColumns = new int[] {0} ;
                    var val = request.Row.Cells[0].Value.ToString();
                    if(int.TryParse(val,out id))
                        result.ForeignUserDefinedId = id;
                    else
                        result.InvalidForeignIdValue = true;

                }
            }

            return result;
        }

        public virtual CellsParseReult ParseCells(IExcelRowParseRequest request , int[] excludeCells, Type onType )
        {
            var result = new CellsParseReult();

            var invalidCells = new List<int>();
            var values = new Dictionary<string,object>();
            for (int i = 0; i < request.Row.Cells.Length; i++)
            {
                if(excludeCells.Contains(i)) continue;

                var value = request.Row.Cells[i];
                var name = request.Sheet.HeaderMap[i];

                object castValue;
                var propertyPath = PropertyPath.TryParse(onType, name.ToPascalCase());

                var convertSucceeded = 
                    Converter.TryCreate( value, propertyPath.PropertyType, out castValue);

                if (convertSucceeded)
                {
                    if (castValue !=null && castValue.GetType() == typeof(string) && 
                        (castValue as string) == string.Empty )
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

            if(invalidCells.Count ==0)
                result.RowObject = onType.CreateFromDictionary(values);
            else result.InvalidCells = invalidCells.ToArray();

            return result;

        }
        
        public virtual async Task<IdInStoreValidationResult> ValidateIds(IExcelRowParseResult parseResult, IExcelRowParseRequest request)
        {
            var result = new IdInStoreValidationResult();

            if(parseResult.InvalidIdValue.HasValue && !parseResult.InvalidIdValue.Value && request.IsMainSheet )
            {
                var found = await repo.RowRecordExists(request.Sheet, parseResult.UserDefinedId.Value);
                if(found)
                    result.IdDuplicate = true;
            }
            
            if(parseResult.ForeignUserDefinedId.HasValue)
            {
                var found = await repo.RowRecordExists(request.MainSheet, parseResult.ForeignUserDefinedId.Value);
                if(!found)
                    result.ForeignIdNotFound = true;
            }

            return result;
        }
    }
}