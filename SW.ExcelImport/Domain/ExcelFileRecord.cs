using System.Linq;
using System;
using System.Collections.Generic;
using SW.PrimitiveTypes;

namespace SW.ExcelImport.Domain
{
    public class ExcelFileRecord: BaseEntity, ISheetContainer
    {
        public ExcelFileRecord(string reference, IEnumerable<ISheet> sheets, IDictionary<int,SheetValidationResult> sheetsValidationResult)
        {
            this.Reference = reference ?? throw new ArgumentNullException(nameof(reference));
            this.SheetRecords = sheets.Select(x=> new SheetRecord(this, x, sheetsValidationResult[x.Index] )).ToList();
        }
        public string Reference { get; private set; }
        public Audit Audit { get; set; }
        public int SheetCount { get; set; }
        public IEnumerable<SheetRecord> SheetRecords { get; set; }
        public ISheet[] Sheets => SheetRecords?.ToArray();

    }
}
