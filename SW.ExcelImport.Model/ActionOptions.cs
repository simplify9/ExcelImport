namespace SW.ExcelImport.Model
{
    public class ActionOptions
    {
        public bool RunInSequence { get; set; }
        public int RetryCount { get; set; }
        public bool RetryOnBadRequests { get; set; }
    }
}
