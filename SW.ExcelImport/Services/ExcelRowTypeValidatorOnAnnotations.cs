using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Collections;
using System.Reflection;

namespace SW.ExcelImport.Services
{
    public class ExcelRowTypeValidatorOnAnnotations : ExcelRowValidator<ExcelRowValidateOnTypeRequest, ExcelRowValidationResult>
    {
        public override Task<ExcelRowValidationResult> Validate(ExcelRowValidateOnTypeRequest request)
        {
            var (json, obj) = ConvertToObject(request);
            var (isValid, result) = ValidateObject(obj);
            if (isValid)
                return Task.FromResult(new ExcelRowValidationResult(json));
            else
                return Task.FromResult(new ExcelRowValidationResult(result));
        }

        public virtual (string, object) ConvertToObject(ExcelRowValidateOnTypeRequest request)
        {

            if(request.RelatedRows == null || request.RelatedRows.Count() == 0)
            {
                var simpleObject = JsonConvert.DeserializeObject(request.Row.RowAsData, request.OnType, JsonUtil.GetSettings(request.NamingStrategy));
                return (request.Row.RowAsData, simpleObject);
            }
                
            var rootObjectJson = JObject.Parse(request.Row.RowAsData);
            
            foreach (var sheetName in request.RelatedRows.Select(r => r.Sheet.Name).Distinct())
            {
                rootObjectJson.Property(sheetName).Remove();
                var newArray = new JArray();
                rootObjectJson.Add(sheetName, newArray);
            }

            foreach (var (name, rowAsData) in request.RelatedRows.Select(r => (r.Sheet.Name, r.RowAsData)))
            {
                var item = JObject.Parse(rowAsData);
                var newArray = (JArray)rootObjectJson[name];
                newArray.Add(item);
            }


            var json = rootObjectJson.ToString();
            var obj = JsonConvert.DeserializeObject(json, request.OnType, JsonUtil.GetSettings(request.NamingStrategy));
            return (json, obj);
        }

        public (bool, List<ValidationResult>) ValidateObject(object obj)
        {
            var validator = new DataAnnotationsValidator();
            var results = new List<ValidationResult>();
            var isValid = validator.TryValidateObjectRecursive(obj,results);
            return (isValid, results);

        }

        
    }
}