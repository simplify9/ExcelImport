using System;
using System.Threading.Tasks;
using SW.ExcelImport.Model;

namespace SW.ExcelImport
{
    public interface IExcelReader: IDisposable
    {
        Task Load(string url, TypedParseToJsonOptions options);
        ISheetContainer Container { get; }
        ISheet CurrentSheet { get; }
        IExcelRow Current { get; }
        Task<bool> ReadRow();
        Task<bool> ReadRow(int sheetIndex, int startOnRow);
        Task<bool> ReadRow(string sheetName, int startOnRow);

    }
}