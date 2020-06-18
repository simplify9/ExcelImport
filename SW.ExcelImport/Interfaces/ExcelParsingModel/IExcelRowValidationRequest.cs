using System.Collections.Generic;

namespace SW.ExcelImport
{
    public interface IExcelRowValidationRequest
    {
        IExcelRowParsed Row { get; }
        IEnumerable<IExcelRowParsed> RelatedRows { get; }
    }
}