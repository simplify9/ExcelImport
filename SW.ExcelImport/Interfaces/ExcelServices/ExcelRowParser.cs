using System.Threading.Tasks;
using System;
namespace SW.ExcelImport
{
    public abstract class ExcelRowParser<TRequest,TResult> 
        where TRequest : IExcelRowParseRequest
        where TResult : IExcelRowParseResult
    {
        public abstract Task<TResult>  Parse(TRequest request);
    }
}