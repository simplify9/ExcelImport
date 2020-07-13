using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using SW.ExcelImport;
using SW.ExcelImport.Entity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Linq;
using Newtonsoft.Json;

namespace SW.ExcelImport.EF
{
    public static class ModelBuilderExtensions
    {

        public static PropertyBuilder<string[]> JsonStringArray(this PropertyBuilder<string[]> builder)
        {
            var c = new ValueConverter<string[], string>(
            array => array == null || array.Length < 1
                ? null
                : JsonConvert.SerializeObject(array),
            str => str == null
                ? new string[] { }
                : JsonConvert.DeserializeObject<string[]>(str));

            builder.HasConversion(c);
            return builder;
        }

        public static PropertyBuilder<CellRecord[]> JsonCellRecords(this PropertyBuilder<CellRecord[]> builder)
        {
            var c = new ValueConverter<CellRecord[], string>(
            array => array == null || array.Length < 1
                ? null
                : JsonConvert.SerializeObject(array),
            str => str == null
                ? new CellRecord[] { }
                : JsonConvert.DeserializeObject<CellRecord[]>(str));

            builder.HasConversion(c);
            return builder;
        }

        public static PropertyBuilder<int[]> SeparatorDelimited(this PropertyBuilder<int[]> builder, char separator = ';')
        {
            var c = new ValueConverter<int[], string>(
            array => array == null || array.Length < 1
                ? null
                : string.Join(separator.ToString(), array),
            str => str == null
                ? new int[] { }
                : str.Split(separator).Select(e => int.Parse(e)).ToArray());

            builder.HasConversion(c);
            return builder;
        }
        public static PropertyBuilder<string[]> SeparatorDelimited(this PropertyBuilder<string[]> builder, char separator = ';')
        {
            var c = new ValueConverter<string[], string>(
            array => array == null || array.Length < 1
                ? null
                : string.Join(separator.ToString(), array),
            str => str == null
                ? new string[] { }
                : str.Split(separator));

            builder.HasConversion(c);
            return builder;
        }
        public static void AddExcelImport(this ModelBuilder b, string schemaName = "excel")
        {
            var excelFile = b.Entity<ExcelFileRecord>();
            excelFile.ToTable("ExcelFiles", schemaName);

            excelFile.Property(x => x.Reference).HasMaxLength(50).IsUnicode().IsRequired();
            excelFile.OwnsOne( x=> x.ProcessOptionsObject);
            excelFile.HasKey(x => x.Id);
            excelFile.HasIndex(x => x.Reference).IsUnique();
            excelFile.Ignore(x => x.Sheets);
            excelFile.HasMany(x => x.SheetRecords).WithOne(x => x.ExcelFileRecord).HasForeignKey( x=> x.ParentId);

            var sheet = b.Entity<SheetRecord>();
            sheet.ToTable("Sheets", schemaName);
            sheet.HasKey(x => x.Id);
            sheet.HasIndex(x => new { x.ParentId, x.Index }).IsUnique();
            sheet.HasIndex(x => new { x.ParentId, x.Name }).IsUnique();
            sheet.Property(x => x.Name).HasMaxLength(31).IsUnicode();
            sheet.Property(x => x.InvalidHeaders).SeparatorDelimited();
            sheet.Property(x => x.Map).SeparatorDelimited();
            sheet.Property(x => x.HeaderCellRecords).JsonCellRecords();
            sheet.Ignore(x => x.Header);
            sheet.Ignore(x => x.Parent);

            var record = b.Entity<RowRecord>();
            record.ToTable("SheetRows", schemaName);
            record.HasKey(x => x.Id);
            record.HasOne(x => x.SheetRecord).WithMany();
            record.HasOne(x => x.Parent).WithMany(x => x.Children).HasForeignKey(x => x.ForeignId);
            record.Property(x => x.CellRecords).JsonCellRecords();
            record.Property(x => x.InvalidCells).SeparatorDelimited();
            record.Property(x => x.ValidationErrors).JsonStringArray();
            record.Ignore(x => x.Sheet);
            record.Ignore(x => x.Cells);

            //record.HasOne()

        }
    }
}
