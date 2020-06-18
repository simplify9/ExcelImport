using System.Collections.Generic;
using System.Threading.Tasks;

namespace SW.ExcelImport
{
    public interface IExcelRepo 
    {
        Task<long?> RowRecordExists(ISheet sheet, int identifier );
        Task<ISheetContainer> CreateExcelFileRecordIfNotExists(ISheetContainer container, IDictionary<int,SheetValidationResult> sheetsValidationResult);
        void Add(IExcelRow row, ISheet sheet, IExcelRowParseResult parseResult = null);
        
    }
    
}