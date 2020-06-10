using System;
using System.Threading.Tasks;
using SW.ExcelImport.Model;

namespace SW.ExcelImport
{
    public interface IExcelReader: IDisposable
    {
        Task Load(string url, Type onType, ParseOptions options);
        ISheetContainer Container { get; }
        ISheet CurrentSheet { get; }
        ISheet MainSheet { get; }
        IExcelRow Current { get; }
        Task<bool> ReadRow();

    }
}