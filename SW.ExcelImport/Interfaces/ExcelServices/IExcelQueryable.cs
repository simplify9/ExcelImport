using System.Threading.Tasks;
using SW.PrimitiveTypes;

namespace SW.ExcelImport
{
    public interface IExcelQueryable
    {
        Task<ISearchyResponse<IExcelRowParsed>> GetParsed (IExcelQueryParsedOptions options);
        
        Task<ISearchyResponse<IExcelRowValidated>> GetValidated (IExcelQueryValidatedOptions options);
    }
}