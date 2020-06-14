using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using SW.ExcelImport;
using SW.ExcelImport.Domain;

namespace SW.ExcelImport.EF
{
    public static class ModelBuilderExtensions
    {
        public static void AddExcelImport(this ModelBuilder b, string schemaName = "excel")
        {
            var excelFile = b.Entity<ExcelFileRecord>();
            excelFile.ToTable("ExcelFile",schemaName);
            excelFile.Property(x=> x.Reference).HasMaxLength(30).IsUnicode();
            excelFile.HasKey(x=> x.Id);
            excelFile.HasIndex(x=> x.Reference).IsUnique();
            excelFile.HasMany(x=> x.SheetRecords).WithOne(x=> x.ExcelFileRecord);

            var sheet = b.Entity<SheetRecord>();
            sheet.ToTable("Sheet", schemaName);
            sheet.HasKey(x=> x.Id);
            sheet.HasIndex(x=> new { x.ExcelFileRecord.Id, x.Index }).IsUnique();
            sheet.HasIndex(x=> new { x.ExcelFileRecord.Id, x.Name }).IsUnique();
            sheet.Property(x=> x.Name).HasMaxLength(31).IsUnicode();

            var record = b.Entity<RowRecord>();
            record.HasKey(x=> x.Id);
            record.HasOne(x=> x.SheetRecord).WithMany();
            //record.HasOne()

        }
    }
}
