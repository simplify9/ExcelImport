using System;

namespace SW.ExcelImport
{
    public interface IExcelRowValidated : IExcelRowParsed, IExcelRowValidationResult
    {
        
    }

    public interface IExcelRowParsed<out T> : IExcelRowParsed
    {
        public T DataTyped { get; }
    }

}