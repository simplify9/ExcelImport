using System.Data;
namespace SW.ExcelImport.Model
{
    public class SheetMappingOptions
    {
        public static SheetMappingOptions Default(int index)
        {
            return new SheetMappingOptions
            {
                SheetIndex = index,
                Map = null,
                IdIndex = index == 1 ? 1 : (int?)null,
                ParentIdIndex = index == 1 ? (int?)null :1,
                IndexAsId = index == 1 ? false : true,
            };
        }
        
        public int SheetIndex { get; set; }
        public string[] Map { get; set; }
        public int? IdIndex { get; set; }
        public int? ParentIdIndex { get; set; }
        public string SheetLongName { get; set; }
        public bool IndexAsId { get; set; }
    }
}
