using System.Threading.Tasks;
namespace SW.ExcelImport
{
    public abstract class ExcelRowValidator<TRequest,TResult> 
        where TRequest : IExcelRowValidationRequest
        where TResult : IExcelRowValidationResult
    {
        public abstract Task<TResult>  Validate(TRequest request);
    }
}