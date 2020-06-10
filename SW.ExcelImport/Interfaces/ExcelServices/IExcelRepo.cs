using System.Threading.Tasks;

namespace SW.ExcelImport
{
    public interface IExcelRepo 
    {
        Task<bool> RowRecordExists(ISheet sheet, int identifier );
        Task<ISheetContainer> CreateExcelFileRecordIfNotExists(ISheetContainer container);
        
    }
    
}