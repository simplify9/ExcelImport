using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SW.ExcelImport.Services;
using SW.ExcelImport.UnitTests.TypeBased;

namespace SW.ExcelImport.UnitTests.Integration
{

    [TestClass]
    public class SheetValid
    {
        
        private static SheetRowReader<Order> svc;
        
        
        [ClassInitialize]
        public static void ClassInitialize(TestContext tContext)
        {
            svc=(SheetRowReader<Order>)TestServerClientFactory.GetServiceProvider().GetService(typeof(SheetRowReader<Order>));
        }
        
        [TestMethod]
        public async Task Basic()
        {
            var loadResult = await svc.Load("valid_single_sheet");
            Assert.AreEqual(false, loadResult.HasErrors);

            var result = await svc.ReadAll();
            
            Assert.IsTrue(result.Any());
            
        }

    }
}