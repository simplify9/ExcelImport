using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Globalization;
using System.Data;
using System.Threading.Tasks;
using SW.ExcelImport.Entity;

namespace SW.ExcelImport.Services
{
    public class ExcelRepo 
    {
        readonly DbContext db;

        public ExcelRepo(DbContext db)
        {
            this.db = db;
        }

        public async Task<long?> RowRecordExists(ISheet sheet, int identifier)
        {
            var result = await db.Set<RowRecord>().Where(x => x.UserDefinedId == identifier &&
                x.SheetRecord.Name == sheet.Name &&
                x.SheetRecord.ExcelFileRecord.Reference == sheet.Parent.Reference).Select(r => r.Id).FirstOrDefaultAsync();
            return result == 0 ? (long?)null : result;
        }

        public async Task<IEnumerable<RowRecord>> GetParsedOk(string reference, int skip, int take)
        {
            var query = GetParsedOk(reference);
            query = query.Include(x => x.Children).ThenInclude(x => x.SheetRecord);
            var result = await query.Skip(skip).Take(take).OrderBy(x => x.Index).ToListAsync();
            return result;
        }

        public async Task<int> GetParsedOkCount(string reference)
        {
            var query = GetParsedOk(reference);
            var count = await query.CountAsync();

            return count;
        }

        private IQueryable<RowRecord> GetParsedOk(string reference)
        {
            return db.Set<RowRecord>().Where(x => x.SheetRecord.ExcelFileRecord.Reference == reference &&
                x.ParseOk == true && x.Children.All(c => c.ParseOk == true) && x.SheetRecord.Index == 0);
        }

        public async Task AddExcelSheetFile(ExcelFileRecord record)
        {
            db.Add(record);
            await SaveChanges();
        }

        public async Task MarkFileValidated(string reference)
        {
            var record = await db.Set<ExcelFileRecord>().FirstOrDefaultAsync(r => r.Reference == reference);
            record.ValidationComplete = true;
            await SaveChanges();
        }
        public RowRecord Add(IExcelRow row, ISheet sheet, IExcelRowParseResult parseResult)
        {
            if (row == null)
                throw new ArgumentNullException(nameof(row));
            if (sheet == null)
                throw new ArgumentNullException(nameof(sheet));

            var excelFileRecord = sheet.Parent as ExcelFileRecord;
            if (excelFileRecord == null)
                throw new ArgumentException("could not cast container as ExcelFileRecord");
            var sheetRecord = sheet as SheetRecord;
            if (sheetRecord == null)
                throw new ArgumentException("could not cast sheet as SheetRecord");

            var rowRecord = new RowRecord(row, sheetRecord, parseResult);
            db.Add(rowRecord);
            return rowRecord;

            

        }
        public async Task SaveChanges()
        {
            await db.SaveChangesAsync();
            
            var changedEntriesCopy = db.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                e.State == EntityState.Modified ||
                e.State == EntityState.Deleted)
                .ToList();

            foreach (var entry in changedEntriesCopy)
            {
                if(entry.Entity.GetType() == typeof(RowRecord))
                    entry.State = EntityState.Detached;
            }
                
        
        }

    }
}