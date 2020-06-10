using System;

namespace SW.ExcelImport.Model
{
    public class CreateBatch
    {
        public string FileUrl { get; set; }
        public string ApiUrl { get; set; }
        public string PayloadType { get; set; }
        public ParseOptions ParseOptions { get; set; }
        public ActionOptions ActionOptions { get; set; }
    }
}
