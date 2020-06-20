using Microsoft.VisualStudio.TestTools.UnitTesting;

using System;
using System.Linq;
using SW.ExcelImport.Model;
using SW.ExcelImport.Services;
using Moq;

namespace SW.ExcelImport.UnitTests.TypeBased.Json.RowTypeParserTests
{

    [TestClass]
    public class ParseIdsTests
    {
        
        [TestMethod]
        public void Basic()
        {

            var options = new SheetMappingOptions { IdIndex = 0 };
            var svc = new ExcelRowTypeParser(null);
            var row = Utils.GetRow(1, new object[] { "1", 5d, DateTime.Now });

            var result = svc.ParseId(row, options);

            Assert.AreEqual(false, result.InvalidIdValue);
            Assert.AreEqual(false, result.InvalidForeignIdValue);
            Assert.AreEqual(1, result.UserDefinedId);
            Assert.IsNull(result.ForeignUserDefinedId);
            Assert.IsTrue(result.ExcludeColumns.Length == 1);
            Assert.IsTrue(result.ExcludeColumns[0] == 0);
        }

        [TestMethod]
        public void WithForeignBasic()
        {
            var options = new SheetMappingOptions { IndexAsId = true, ParentIdIndex = 0, SheetIndex = 2 };
            var svc = new ExcelRowTypeParser(null);
            var row = Utils.GetRow(1, new object[] { "10", 5d, DateTime.Now });

            var result = svc.ParseId(row, options);

            Assert.AreEqual(false, result.InvalidForeignIdValue);
            Assert.AreEqual(false, result.InvalidIdValue);
            Assert.AreEqual(1, result.UserDefinedId);
            Assert.IsNotNull(result.ForeignUserDefinedId);
            Assert.AreEqual(10, result.ForeignUserDefinedId);
            Assert.IsTrue(result.ExcludeColumns.Length == 1);
            Assert.IsTrue(result.ExcludeColumns[0] == 0);
        }


        [TestMethod]
        public void InvalidBasic()
        {

            var options = new SheetMappingOptions { IdIndex = 0 };
            var svc = new ExcelRowTypeParser(null);
            var row = Utils.GetRow(1, new object[] { "invalid value", 5d, DateTime.Now });

            var result = svc.ParseId(row, options);

            Assert.AreEqual(true, result.InvalidIdValue);
            Assert.AreEqual(false, result.InvalidForeignIdValue);

            Assert.IsNull(result.ForeignUserDefinedId);
            Assert.IsTrue(result.ExcludeColumns.Length == 1);
            Assert.IsTrue(result.ExcludeColumns[0] == 0);
        }

        [TestMethod]
        public void WithForeignInvalidBasic()
        {
            var options = new SheetMappingOptions { IndexAsId = true, ParentIdIndex = 0, SheetIndex = 2 };
            var svc = new ExcelRowTypeParser(null);
            var row = Utils.GetRow(1, new object[] { "not a valid 1", 5d, DateTime.Now });

            var result = svc.ParseId(row, options);

            Assert.AreEqual(true, result.InvalidForeignIdValue);
            Assert.AreEqual(false, result.InvalidIdValue);
            Assert.AreEqual(1, result.UserDefinedId);
            Assert.IsNull(result.ForeignUserDefinedId);
            
            Assert.IsTrue(result.ExcludeColumns.Length == 1);
            Assert.IsTrue(result.ExcludeColumns[0] == 0);
        }

        
    }
}