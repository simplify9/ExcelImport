namespace SW.ExcelImport.Domain
{
    public class ExcelCell: ICell
    {
        protected ExcelCell()
        {
            
        }
        public ExcelCell(ICell  cell)
        {
            this.Type = cell.Type;
            this.Value = cell.Value;
            this.NumberFormatIndex = cell.NumberFormatIndex;
            this.NumberFormatString = cell.NumberFormatString;
        }
        
        public string Type { get; set; }
        public object Value { get; set; }
        public int NumberFormatIndex { get; set; }
        public string NumberFormatString { get; set; }

    }
}
