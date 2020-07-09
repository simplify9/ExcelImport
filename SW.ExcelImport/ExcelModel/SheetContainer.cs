namespace SW.ExcelImport
{
    public class SheetContainer : ISheetContainer
    {
        public SheetContainer(string reference, ISheet[] sheets)
        {
            Reference = reference;
            Sheets = sheets;
        }
        public string Reference { get; }

        public ISheet[] Sheets { get; set; }

        public IProcessOptions ProcessOptions { get; }
    }


}