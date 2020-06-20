using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW.ExcelImport.Model;
using SW.ExcelImport.Services;
using Newtonsoft.Json;

namespace SW.ExcelImport.UnitTests.TypeBased.Json.RowTypeParserTests
{
    [TestClass]
    public class ParseCellsTests
    {
        [TestMethod]
        public void OnRootBasic()
        {
            var row = Utils.GetRow(1, new object[] { "1", "101", "the user number", "kg", 8 });
            var options = new SheetMappingOptions
            { Map = new string[] { "id", "number", "user_number", "weight.unit", "weight.value" }, SheetLongName = "order" };

            var strategy = JsonNamingStrategy.SnakeCase;

            var request = new ExcelRowParseOnTypeRequest
            { NamingStrategy = strategy, Options = options, RootType = typeof(Order), Row = row };
            var excludeCells = new int[] { 0 };
            var svc = new ExcelRowTypeParser(null);
            var result = svc.ParseCells(request, excludeCells);

            Assert.AreEqual(0, result.InvalidCells.Length);
            Assert.IsNotNull(result.RowAsJson);
            var obj = JsonConvert.DeserializeObject<Order>(result.RowAsJson, JsonUtil.GetSettings(strategy));
            Assert.AreEqual("101", obj.Number);
            Assert.AreEqual("the user number", obj.UserNumber);
            Assert.IsNotNull(obj.Weight);
            Assert.AreEqual("kg", obj.Weight.Unit);
            Assert.AreEqual(8m, obj.Weight.Value);

        }

        [TestMethod]
        public void CellWithStringArray()
        {
            var row = Utils.GetRow(1, new object[] { "1", "a@a.com;b@b.com" , "John" });
            var options = new SheetMappingOptions
            { Map = new string[] { "id", "emails", "people" }, SheetLongName = "order" };

            var strategy = JsonNamingStrategy.SnakeCase;

            var request = new ExcelRowParseOnTypeRequest
            { NamingStrategy = strategy, Options = options, RootType = typeof(Order), Row = row };
            var excludeCells = new int[] { 0 };
            var svc = new ExcelRowTypeParser(null);
            var result = svc.ParseCells(request, excludeCells);

            Assert.AreEqual(0, result.InvalidCells.Length);
            Assert.IsNotNull(result.RowAsJson);
            var obj = JsonConvert.DeserializeObject<Order>(result.RowAsJson, JsonUtil.GetSettings(strategy));
            Assert.AreEqual("a@a.com", obj.Emails[0]);
            Assert.AreEqual("b@b.com", obj.Emails[1]);
            Assert.AreEqual("John", obj.People[0]);
            
        }

        [TestMethod]
        public void OnRootInvalidBasic()
        {
            var row = Utils.GetRow(1, new object[] { "1", "101", "the user number", "kg", "invalid" });
            var options = new SheetMappingOptions
            { Map = new string[] { "id", "number", "user_number", "weight.unit", "weight.value" }, SheetLongName = "order" };

            var strategy = JsonNamingStrategy.SnakeCase;

            var request = new ExcelRowParseOnTypeRequest
            { NamingStrategy = strategy, Options = options, RootType = typeof(Order), Row = row };
            var excludeCells = new int[] { 0 };
            var svc = new ExcelRowTypeParser(null);
            var result = svc.ParseCells(request, excludeCells);

            Assert.AreEqual(1, result.InvalidCells.Length);
            Assert.IsNull(result.RowAsJson);
            Assert.AreEqual(4, result.InvalidCells[0]);
        }
        [TestMethod]
        public void OnLeafBasic()
        {
            var row = Utils.GetRow(2, new object[] { "1", "item1", "US", 1d });
            var options = new SheetMappingOptions
            { Map = new string[] { "OrderId", "name", "country_code", "quantity" }, SheetLongName = "items" };

            var strategy = JsonNamingStrategy.SnakeCase;

            var request = new ExcelRowParseOnTypeRequest
            { NamingStrategy = strategy, Options = options, RootType = typeof(Order), Row = row };
            var excludeCells = new int[] { 0 };
            var svc = new ExcelRowTypeParser(null);
            var result = svc.ParseCells(request, excludeCells);

            Assert.AreEqual(0, result.InvalidCells.Length);
            Assert.IsNotNull(result.RowAsJson);
            var obj = JsonConvert.DeserializeObject<Item>(result.RowAsJson, JsonUtil.GetSettings(strategy));
            Assert.AreEqual("item1", obj.Name);
            Assert.AreEqual("US", obj.CountryCode);
            Assert.AreEqual(1, obj.Quantity);

        }
    }
}