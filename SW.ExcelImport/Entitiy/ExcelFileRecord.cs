using System.Linq;
using System;
using System.Collections.Generic;
using SW.PrimitiveTypes;

namespace SW.ExcelImport.Entity
{
    public class ExcelFileRecord: ISheetContainer
    {
        protected ExcelFileRecord()
        {
            
        }

        public ExcelFileRecord(string reference, IEnumerable<ISheet> sheets, 
            IDictionary<int,SheetValidationResult> sheetsValidationResult, object processOptions, 
            string createdBy = null)
        {
            Reference = reference ?? throw new ArgumentNullException(nameof(reference));
            SheetRecords = sheets.Select(x=> new SheetRecord(this, x, sheetsValidationResult[x.Index] )).ToList();
            ProcessOptionsObject = processOptions.ToObjectSerialized();
            CreatedOn = DateTime.Now;
            CreatedBy = createdBy;
        }
        public int Id { get; private set; }
        public string Reference { get; private set; }
        public IEnumerable<SheetRecord> SheetRecords { get; set; }
        public ISheet[] Sheets => SheetRecords?.ToArray();
        public ObjectSerialized ProcessOptionsObject { get; set; }
        public IProcessOptions ProcessOptions => ProcessOptionsObject.ToObject() as IProcessOptions;
        public bool ParseComplete { get; set; }
        public bool ValidationComplete { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
