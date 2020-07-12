using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using SW.ExcelImport.Entity;
using SW.PrimitiveTypes;

namespace SW.ExcelImport.Services
{
    public class ExcelQueryable: IExcelQueryable
    {
        
        readonly DbContext db;

        public ExcelQueryable(DbContext db)
        {
            this.db = db;
        }

        public async Task<ISearchyResponse<IExcelRowParsed>> GetParsed(IExcelQueryParsedOptions options)
        {
            var query = db.Set<RowRecord>().Where(r => r.SheetRecord.ExcelFileRecord.Reference == options.Reference);

            if(options.RowStatus == QueryRowStatus.Valid)
                query = query.Where(r => r.ParseOk ==true);
            
            if(options.RowStatus == QueryRowStatus.Invalid)
                query = query.Where(r => r.ParseOk ==false);
            
            if(options.RowStatus == QueryRowStatus.All)
                query = query.Where(r => r.ParseOk !=null);
            
            
            if(options.FromSheet.HasValue)
                query = query.Where(r => r.SheetRecord.Index >= options.FromSheet.Value);
            if(options.ToSheet.HasValue)
                query = query.Where(r => r.SheetRecord.Index <= options.ToSheet.Value);

            if(options.FromRow.HasValue)
                query = query.Where(r => r.Index >= options.FromRow.Value);
            if(options.ToRow.HasValue)
                query = query.Where(r => r.Index <= options.ToRow.Value);
            
            var pageIndex = options.PageIndex ?? 0;
            var pageSize = options.PageSize ?? 10;

            var count = await query.CountAsync();

            query = query.Skip(pageIndex).Take(pageSize);

            var result = await query.ToListAsync();

            return new SearchyResponse<IExcelRowParsed>
            {
                Result = result,
                TotalCount = count
            };
        }

        public async Task<ISearchyResponse<IExcelRowValidated>> GetValidated(IExcelQueryValidatedOptions options)
        {
            var query = db.Set<RowRecord>().Where(r => r.SheetRecord.ExcelFileRecord.Reference == options.Reference && 
                r.SheetRecord.Index == 0);
            
            if(options.RowStatus == QueryRowStatus.Valid)
                query = query.Where(r => r.IsValid ==true);
            
            if(options.RowStatus == QueryRowStatus.Invalid)
                query = query.Where(r => r.IsValid ==false);
            
            if(options.RowStatus == QueryRowStatus.All)
                query = query.Where(r => r.IsValid !=null);
            
            if(options.FromRow.HasValue)
                query = query.Where(r => r.Index >= options.FromRow.Value);
            if(options.ToRow.HasValue)
                query = query.Where(r => r.Index <= options.ToRow.Value);
            
            var pageIndex = options.PageIndex ?? 0;
            var pageSize = options.PageSize ?? 10;

            var count = await query.CountAsync();

            query = query.Skip(pageIndex).Take(pageSize);

            var result = await query.ToListAsync();

            return new SearchyResponse<IExcelRowValidated>
            {
                Result = result,
                TotalCount = count
            };

        }
    }
}