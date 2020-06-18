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
        
        public async Task<long?> RowRecordExists(ISheet sheet, int identifier )
        {
            var result = await db.Set<RowRecord>().Where(x=> x.UserDefinedId == identifier && 
                x.Sheet.Name == sheet.Name && 
                x.Sheet.Parent.Reference == sheet.Parent.Reference).Select(r=> r.Id).FirstOrDefaultAsync();
            return result == 0 ? (long?)null : result;
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

        public async Task<IEnumerable<RowRecord>> GetParsedOk (string reference, int skip, int take)
        {
            var query = GetParsedOk(reference);
            query = query.Include(x => x.Children).ThenInclude(x => x.Sheet);
            var result = await query.Skip(skip).Take(take).OrderBy(x=> x.Index).ToListAsync();
            return result;
        }

        public async Task<int> GetParsedOkCount (string reference)
        {
            var query = GetParsedOk(reference);
            var count = await query.CountAsync();
            
            return count;
        }

        private IQueryable<RowRecord> GetParsedOk (string reference)
        {
            return db.Set<RowRecord>().Where(x=> x.Sheet.Parent.Reference == reference && 
                x.ParseOk == true && x.Children.All(c => c.ParseOk == true) && x.Sheet.Index == 1);
        }

        private IQueryable<RowRecord> GetParsed (string reference)
        {
            return db.Set<RowRecord>().Where(x=> x.Sheet.Parent.Reference == reference && 
                x.ParseOk != null && x.Children.All(c => c.ParseOk != null) && x.Sheet.Index == 1);
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