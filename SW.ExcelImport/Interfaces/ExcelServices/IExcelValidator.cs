using System.Threading.Tasks;
using SW.ExcelImport.Model;

namespace SW.ExcelImport
{
    public interface IExcelValidator
    {
        Task Process(string reference, TypedParseToJsonOptions parseOptions);
    } 
}