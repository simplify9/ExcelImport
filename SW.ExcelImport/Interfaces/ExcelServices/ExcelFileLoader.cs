using System.Threading.Tasks;
namespace SW.ExcelImport
{
    public abstract class ExcelFileLoader<TRequest,TResult> 
        where TRequest : IProcessOptions
        where TResult : ISheetContainer
    {
        public abstract Task<TResult>  Load(string url, TRequest request);
    }
    
}