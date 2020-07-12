namespace SW.ExcelImport
{
    public class ExcelQueryValidatedOptions : IExcelQueryValidatedOptions
    {
        public string Reference { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public QueryRowStatus RowStatus { get; set; } = QueryRowStatus.Valid;
        
        public int? FromRow { get; set;}
        public int? ToRow { get; set;}
    }
}
