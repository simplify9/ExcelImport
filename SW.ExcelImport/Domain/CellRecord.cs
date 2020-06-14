namespace SW.ExcelImport.Domain
{
    public class CellRecord: ICell
    {
        protected CellRecord()
        {
            
        }
        public CellRecord(ICell  cell)
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