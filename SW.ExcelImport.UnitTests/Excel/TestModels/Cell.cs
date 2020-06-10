using System;
using SW.ExcelImport;

namespace SW.ExcelImport.UnitTests
{
    public static class Cell
    {
        public static ICell String(string value) => new StringCell(value);
        public static ICell Int(int value) => new IntCell(value);
        public static ICell Double(double value) => new DoubleCell(value);
        public static ICell String(string value) => new StringCell(value);
        public static ICell String(string value) => new StringCell(value);


    }
    public class StringCell : ICell
    {
        public StringCell(string value) => Value = value;
        public string Type => "string";
        public object Value {get;}
        public int NumberFormatIndex => throw new NotImplementedException();
        public string NumberFormatString => throw new NotImplementedException();
    }

    public class IntCell : ICell
    {
        public IntCell(int value) => Value = value;
        public string Type => "int";
        public object Value {get;}
        public int NumberFormatIndex => throw new NotImplementedException();
        public string NumberFormatString => throw new NotImplementedException();
    }

    public class DoubleCell : ICell
    {
        public DoubleCell(double value) => Value = value;
        public string Type => "double";
        public object Value {get;}
        public int NumberFormatIndex => throw new NotImplementedException();
        public string NumberFormatString => throw new NotImplementedException();
    }

    public class BoolCell : ICell
    {
        public BoolCell(bool value) => Value = value;
        public string Type => "bool";
        public object Value {get;}
        public int NumberFormatIndex => throw new NotImplementedException();
        public string NumberFormatString => throw new NotImplementedException();
    }
    
    public class DateTimeCell : ICell
    {
        public DateTimeCell(DateTime value) => Value = value;
        public string Type => "bool";
        public object Value {get;}
        public int NumberFormatIndex => throw new NotImplementedException();
        public string NumberFormatString => throw new NotImplementedException();
    }

    public class TimeSpanCell : ICell
    {
        public TimeSpanCell(TimeSpan value) => Value = value;
        public string Type => "bool";
        public object Value {get;}
        public int NumberFormatIndex => throw new NotImplementedException();
        public string NumberFormatString => throw new NotImplementedException();
    }
    
    public class NullCell : ICell
    {
        public string Type => "null";
        public object Value => null;
        public int NumberFormatIndex => throw new NotImplementedException();
        public string NumberFormatString => throw new NotImplementedException();
    }
    
}