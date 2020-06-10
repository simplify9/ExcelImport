namespace SW.ExcelImport
{
    public class IdParseResult
    {
        public int? UserDefinedId { get; set; }
        public int? ForeignUserDefinedId { get; set; }
        public bool? InvalidIdValue { get; set; }
        public bool? InvalidForeignIdValue { get; set; }
        public int[] ExcludeColumns {get; set;}
    }

}