using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Data;
using System.Threading.Tasks;
using SW.ExcelImport.Domain;

namespace SW.ExcelImport.Services
{
    public class ExcelRepo: IExcelRepo
    {
        readonly DbContext db;

        private List<ISheet> added = new List<ISheet>();
        public ExcelRepo(DbContext db)
        {
            this.db = db;
        }
        
        public Task<bool> RowRecordExists(ISheet sheet, int identifier )
        {
            return db.Set<RowRecord>().AnyAsync(x=> x.UserDefinedId == identifier && 
                x.Sheet.Name == sheet.Name && 
                x.Sheet.Parent.Reference == sheet.Parent.Reference);
        }

        public async Task<ISheetContainer> CreateExcelFileRecordIfNotExists(ISheetContainer container, IDictionary<int,SheetValidationResult> sheetsValidationResult  )
        {
            //db.Set
            var exists = await db.Set<ExcelFileRecord>().Include(x=> x.SheetRecords).
                FirstOrDefaultAsync( r=> r.Reference == container.Reference);

            if(exists != null)
                return exists;
            
            
            var record = new ExcelFileRecord(container.Reference, container.Sheets,sheetsValidationResult);
            db.Add(record);
            await db.SaveChangesAsync();

            return record;
        }

        public void Add(IExcelRow row, ISheet sheet, IExcelRowParseResult parseResult = null )
        {
            if(row == null)
                throw new ArgumentNullException(nameof(row));
            if(sheet == null)
                throw new ArgumentNullException(nameof(sheet));
            
            var excelFileRecord = sheet.Parent as ExcelFileRecord;
            if(excelFileRecord == null)
                throw new ArgumentException("could not cast container as ExcelFileRecord");
            var sheetRecord = sheet as SheetRecord;
            if(sheetRecord == null)
                throw new ArgumentException("could not cast sheet as SheetRecord");
            
            var existingRow = row as RowRecord;
            if(existingRow == null)
            {
                
                var rowRecord = parseResult == null ? 
                    new RowRecord(row,sheetRecord) : new RowRecord(row,sheetRecord , parseResult) ;
                db.Add(rowRecord);
            }
            else
            {
                existingRow.FillParseResult(parseResult);
            }
            
        }
        public async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }

    }
}