namespace SW.ExcelImport.Model
{
    public class ParseOptions
    {
        public bool BlockOnParse { get; set; }
        public int MaxParseErrors { get; set; }
        public int MaxValidationErrors { get; set; }
        public bool HeaderInFirstRow { get; set; }
        public SheetHeaderMap[] SheetsMap { get; set; }
        public int? MainSheetIdColumn { get; set; }
        public bool MainSheetIndexAsId { get; set; }

    }
}
