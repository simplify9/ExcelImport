using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW.ExcelImport.Entity;
using SW.ExcelImport.Services;
using SW.ExcelImport.UnitTests.TypeBased;

namespace SW.ExcelImport.UnitTests.Integration
{
    [TestClass]
    public class ExcelValid
    {
        private static DbContext db;
        private static ExcelService svc;
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext tContext)
        {

            db = (ExcelDbContext)TestServerClientFactory.GetServiceProvider().GetService(typeof(DbContext));
            svc=(ExcelService)TestServerClientFactory.GetServiceProvider().GetService(typeof(ExcelService));

        }

        [TestMethod]
        public async Task LoadBasic()
        {
            var result = await svc.LoadExcelFileInfo("valid1" , new TypedParseToJsonOptions 
            {
                AssemblyQualifiedTypeName = typeof(Order).AssemblyQualifiedName,
                NamingStrategy = JsonNamingStrategy.SnakeCase
            });

            Assert.AreEqual(false, result.Sheets.Any(x => x.HasErrors()));
        }

        [TestMethod]
        public async Task ParseAndValidateBasic()
        {
            var options = new TypedParseToJsonOptions 
            {
                AssemblyQualifiedTypeName = typeof(Order).AssemblyQualifiedName,
                NamingStrategy = JsonNamingStrategy.SnakeCase
            };
            await svc.Import("valid1" , options);

            var recordCount =await  db.Set<RowRecord>().CountAsync();

            Assert.AreEqual(11, recordCount);
            recordCount = await db.Set<SheetRecord>().CountAsync();
            Assert.AreEqual(3, recordCount);

            recordCount =await  db.Set<RowRecord>().CountAsync(x => x.ParseOk ==true);
            Assert.AreEqual(11, recordCount);

            await svc.Process("valid1" , options);

            recordCount =await  db.Set<RowRecord>().CountAsync(x => x.IsValid !=null);
            Assert.AreEqual(11, recordCount);

        }
        
        // [TestMethod]
        // public async Task ParseAndValidateBig()
        // {
        //     var options = new TypedParseToJsonOptions 
        //     {
        //         AssemblyQualifiedTypeName = typeof(Order).AssemblyQualifiedName,
        //         NamingStrategy = JsonNamingStrategy.SnakeCase
        //     };
        //     await svc.Import("validbig" , options);

        //     var querySvc = new ExcelQueryable(db);
        //     var result = await querySvc.GetParsed(new ExcelQueryParsedOptions { Reference = "validbig" });
        //     Assert.AreEqual(47, result.TotalCount);

        //     await svc.Process("validbig" , options);

        //     var recordCount =await  db.Set<RowRecord>().CountAsync(x => x.IsValid ==null);

        //     Assert.AreEqual(1, recordCount);

        //     recordCount =await  db.Set<RowRecord>().CountAsync(x => x.Data !=null);
            
        //     Assert.AreEqual(26, recordCount);

        //     var validationResult = await querySvc.GetValidated(new ExcelQueryValidatedOptions { Reference = "validbig" });
        //     Assert.AreEqual(22, validationResult.TotalCount);
            

        // }
    }
}