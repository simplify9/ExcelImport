using SW.PrimitiveTypes;

namespace SW.ExcelImport.Domain
{
    public class Batch: BaseEntity
    {
        public string ApiBaseUrl { get; set; }
        public string Status { get; set; }
        public Audit Audit { get; set; }
        
    }
}
