namespace SW.ExcelImport
{
    public interface IExcelRow
    {
        
        int Index { get;}
        ICell[] Cells { get;}
        
    }

    
}