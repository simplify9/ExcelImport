using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExcelDataReader;

namespace SW.ExcelImport
{
    public interface IExcelReader: IDisposable
    {
        Task<ISheetContainer> Load(string url, ICollection<SheetMappingOptions> sheetsOptions);
        IExcelRow Current { get; }
        Task<bool> ReadRow();
    }
}