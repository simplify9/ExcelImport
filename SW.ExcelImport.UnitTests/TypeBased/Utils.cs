using System.Linq;
using Moq;

namespace SW.ExcelImport.UnitTests.TypeBased
{
    public static class Utils
    {
        public static ISheet GetSheet(int index, string name, object[] header, bool emptyData = false , bool empty =false  )
        {
            var sheet = new Mock<ISheet>();
            sheet.Setup(s => s.Index).Returns(index);
            sheet.Setup(s => s.Header).Returns(header.Select(v => GetCell(v)).ToArray());
            sheet.Setup(s => s.Name).Returns(name);
            sheet.Setup(s => s.Empty).Returns(empty);
            sheet.Setup(s => s.EmptyData).Returns(emptyData);
            return sheet.Object;
        }
        public static ICell GetCell(object val)
        {
            var cell = new Mock<ICell>();
            cell.Setup(c => c.Value).Returns(val);
            return cell.Object;
        }
        public static IExcelRow GetRow(int sheetIndex, object[] values, int rowIndex = 1)
        {
            var row = new Mock<IExcelRow>();
            var sheet = new Mock<ISheet>();

            sheet.Setup(s => s.Index).Returns(sheetIndex);

            row.Setup(r => r.Sheet).Returns(sheet.Object);
            row.Setup(r => r.Cells).Returns(values.Select(v => GetCell(v)).ToArray());
            row.Setup(r => r.Index).Returns(rowIndex);
            row.Setup(r => r.Sheet).Returns(sheet.Object);
            return row.Object;
        }

    }
}