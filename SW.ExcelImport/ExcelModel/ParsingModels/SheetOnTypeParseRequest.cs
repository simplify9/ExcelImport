using System;

namespace SW.ExcelImport
{
    public class SheetOnTypeParseRequest
    {
        public ISheet Sheet { get; set; }
        public string LongName { get; set; }
        public SheetMappingOptions MappingOptions { get; set; }
        public Type RootType { get; set; }
        public JsonNamingStrategy NamingStrategy { get; set; }
    }
}