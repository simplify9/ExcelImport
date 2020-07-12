namespace SW.ExcelImport
{
    public class ExcelQueryParsedOptions : IExcelQueryParsedOptions
    {
        public string Reference { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public QueryRowStatus RowStatus { get; set; } = QueryRowStatus.Valid;
        public int? FromSheet { get; set;} 
        public int? ToSheet { get; set;} 
        public int? FromRow { get; set;}
        public int? ToRow { get; set;}
    }
}
