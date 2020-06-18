using System;
using System.Collections.Generic;
using SW.ExcelImport.Domain;
using SW.ExcelImport.Model;

namespace SW.ExcelImport
{
    public class ExcelRowValidateOnTypeRequest: IExcelRowValidationRequest
    {
        readonly RowRecord row;
        readonly Type onType;
        protected ExcelRowValidateOnTypeRequest()
        {
            
        }
        public ExcelRowValidateOnTypeRequest(RowRecord row, Type onType, JsonNamingStrategy namingStrategy = JsonNamingStrategy.None)
        {
            this.row = row;
            this.onType = onType;
            this.NamingStrategy = namingStrategy;
        }
        public virtual IEnumerable<IExcelRowParsed> RelatedRows => row.Children;

        public virtual IExcelRowParsed Row => row;

        public virtual Type OnType => onType;
        public virtual JsonNamingStrategy NamingStrategy { get; }
    }

}