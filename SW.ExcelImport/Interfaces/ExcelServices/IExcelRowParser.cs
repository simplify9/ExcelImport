using System.Threading.Tasks;
using System;
namespace SW.ExcelImport
{
    public interface IExcelRowParser
    {
        Task<IExcelRowParseResult>  Parse(IExcelRowParseRequest request, Type onType);
        
    }
}