using System.Threading.Tasks;
using SW.ExcelImport.Model;
using System;

namespace SW.ExcelImport
{
    public interface IExcelStorageMap
    {
        Task Import(IExcelReader source, string url, Type onType, ParseOptions options);
        Task ImportAndParse(IExcelReader source, string url, Type onType, ParseOptions options);
    } 
}