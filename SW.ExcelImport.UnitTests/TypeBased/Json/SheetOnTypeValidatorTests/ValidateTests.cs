using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW.ExcelImport.Model;
using SW.ExcelImport.Services;
using Newtonsoft.Json;


namespace SW.ExcelImport.UnitTests.TypeBased.Json.SheetOnTypeValidatorTests
{
    [TestClass]
    public class ValidateTests
    {
        [TestMethod]
        public async Task Basic()
        {
            var sheet = Utils.GetSheet(1, "sheet1",
                new object[] { "number", "user_number", "customer", "emails", "phones" });

            var request = new SheetOnTypeParseRequest
            {

                MappingOptions = SheetMappingOptions.Default(sheet.Index),
                NamingStrategy = JsonNamingStrategy.SnakeCase,
                RootType = typeof(Order),
                Sheet = sheet
            };

            var svc = new ExcelSheetOnTypeValidator();

            var result = await svc.Validate(request);

            Assert.AreEqual(false, result.HasErrors);
            Assert.AreEqual(true, result.IgnoreFirstRow);
            Assert.AreEqual(0, result.InvalidHeaders.Length);
            Assert.AreEqual(false, result.InvalidName);
            var map = sheet.Header.Select(c => c.Value.ToString()).ToArray();
            for (int i = 0; i < map.Length; i++)
                Assert.AreEqual(map[i], result.Map[i]);
        }

        [TestMethod]
        public async Task InvalidBasic()
        {
            var sheet = Utils.GetSheet(1, "sheet1",
                new object[] { "Id", "number", "user_number", "customer", "emails", "phones", "items" });

            var request = new SheetOnTypeParseRequest
            {

                MappingOptions = SheetMappingOptions.Default(sheet.Index),
                NamingStrategy = JsonNamingStrategy.SnakeCase,
                RootType = typeof(Order),
                Sheet = sheet
            };

            var svc = new ExcelSheetOnTypeValidator();

            var result = await svc.Validate(request);

            Assert.AreEqual(true, result.HasErrors);
            Assert.AreEqual(true, result.IgnoreFirstRow);
            Assert.AreEqual(1, result.InvalidHeaders.Length);
            Assert.AreEqual(false, result.InvalidName);
            
            var map = sheet.Header.Select(c => c.Value.ToString()).ToArray();
            Assert.AreEqual(map.Length -1, result.InvalidHeaders[0]);

            for (int i = 0; i < map.Length; i++)
                Assert.AreEqual(map[i], result.Map[i]);
            
        }
        [TestMethod]
        public async Task RelatedBasic()
        {
            var sheet = Utils.GetSheet(2, "items",
                new object[] { "number", "name", "country_code", "quantity" });

            var request = new SheetOnTypeParseRequest
            {

                MappingOptions = SheetMappingOptions.Default(sheet.Index),
                NamingStrategy = JsonNamingStrategy.SnakeCase,
                RootType = typeof(Order),
                Sheet = sheet
            };

            var svc = new ExcelSheetOnTypeValidator();

            var result = await svc.Validate(request);

            Assert.AreEqual(false, result.HasErrors);
            Assert.AreEqual(true, result.IgnoreFirstRow);
            Assert.AreEqual(0, result.InvalidHeaders.Length);
            Assert.AreEqual(false, result.InvalidName);
            var map = sheet.Header.Select(c => c.Value.ToString()).ToArray();
            for (int i = 0; i < map.Length; i++)
                Assert.AreEqual(map[i], result.Map[i]);
        }


    }
}