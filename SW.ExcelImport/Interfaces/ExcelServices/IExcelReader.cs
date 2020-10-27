using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ExcelDataReader;

namespace SW.ExcelImport
{
    public interface IExcelReader: IDisposable
    {
        Task<ISheet> LoadSheet(string url, int sheetIndex, string[] map = null);
        Task<ISheetContainer> Load(string fileUrl, ICollection<SheetMappingOptions> sheetsOptions);
        IExcelRow Current { get; }
        Task<bool> ReadRow();
    }
}