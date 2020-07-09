using SW.PrimitiveTypes;

namespace SW.ExcelImport.Entity
{
    public class Batch: BaseEntity
    {
        public string ApiBaseUrl { get; set; }
        public string Status { get; set; }
        public Audit Audit { get; set; }
        
    }
}
