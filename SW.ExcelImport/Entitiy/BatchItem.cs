using System.Net;
using SW.PrimitiveTypes;

namespace SW.ExcelImport.Entity
{
    public class BatchItem: BaseEntity
    {
        public BatchItem()
        {
            
        }
        public BatchItem(Batch batch)
        {
            this.Batch = batch;
        }
        public Batch Batch { get; protected set; }
        public int SequenceNo { get; set; }
        public string SourceRowData { get; set; }
        public string Url { get; set; }
        public string Payload { get; set; }
        public string PayloadType { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Response { get; set; }
        public string CorrelationId { get; set; }
        public Audit Audit { get; set; }
    }

    
}
