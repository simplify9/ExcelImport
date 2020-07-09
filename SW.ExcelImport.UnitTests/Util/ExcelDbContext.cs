using Microsoft.EntityFrameworkCore;
using SW.ExcelImport.IServiceCollectionExtensions;
using SW.ExcelImport.EF;

namespace SW.ExcelImport.UnitTests
{
    public class ExcelDbContext : DbContext
    {
        
        public ExcelDbContext(DbContextOptions<ExcelDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            
            modelBuilder.AddExcelImport();

        }
        
        
    }
}
