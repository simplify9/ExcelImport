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
                x.Parent.Name == sheet.Name && 
                x.Parent.Parent.Reference == sheet.Parent.Reference);
        }

        public async Task<ISheetContainer> CreateExcelFileRecordIfNotExists(ISheetContainer container)
        {
            var exists = await db.Set<ExcelFileRecord>().FirstOrDefaultAsync( r=> r.Reference == container.Reference);
            if(exists != null)
                return exists;
            
            var record = new ExcelFileRecord(container.Reference, container.SheetCount);
            return record;
        }

        public void Add(ISheetContainer container, ISheet sheet, IExcelRow row)
        {
            var dbContainer = container as ExcelFileRecord;
            if(dbContainer == null)
                throw new ArgumentException("could not case container as ExcelFileRecord");
            
            SheetRecord sheetRecord = null;

            if(!added.Contains(sheet) &&  (sheet as SheetRecord) == null )
            {
                sheetRecord = new SheetRecord(dbContainer,sheet);
                db.Add(sheetRecord);
                added.Add(sheetRecord);
            }
            else
            {
                if(added.Contains(sheet))
                    sheetRecord = added.Where(x => x.Name == sheet.Name).Single() as SheetRecord;
                else
                    sheetRecord = sheet as SheetRecord;
            }
            
            var existingRow = row as RowRecord;
            if(existingRow == null)
            {
                var dbRow = new RowRecord(row,sheetRecord);
                db.Add(dbRow);
            }
            
        }
        public async Task SaveChanges()
        {
            await db.SaveChangesAsync();
        }

    }
}