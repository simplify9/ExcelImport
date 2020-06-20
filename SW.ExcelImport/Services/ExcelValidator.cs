using SW.ExcelImport.Model;
using System.Threading.Tasks;

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
                    
                    row.FillData(result.Data, result.IsValid.Value);
                }
                await repo.SaveChanges();
            }
        }
    }
}