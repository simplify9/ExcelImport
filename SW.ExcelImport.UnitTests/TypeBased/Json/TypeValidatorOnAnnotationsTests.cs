using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SW.ExcelImport;
using SW.ExcelImport.Model;
using SW.ExcelImport.Services;
using Moq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;

namespace SW.ExcelImport.UnitTests.TypeBased.Json
{
    [TestClass]
    public class TypeValidatorOnAnnotationsTests
    {
        [TestMethod]
        public async Task Basic()
        {

            var settings = JsonUtil.GetSettings(JsonNamingStrategy.SnakeCase);


            var svc = new ExcelRowTypeValidatorOnAnnotations();

            var order = new Order { Number = "Number" };

            var item1 = new Item { Name = "first", Quantity = 4 };
            var item2 = new Item { Name = "second", Quantity = 10 };

            var Other1 = new Other { Prop1 = "property 1" };
            var Other2 = new Other { Prop1 = "property 2" };

            var orderSerialized = JsonConvert.SerializeObject(order, settings);
            var item1Serialized = JsonConvert.SerializeObject(item1, settings);
            var item2Serialized = JsonConvert.SerializeObject(item2, settings);
            var Other1Serialized = JsonConvert.SerializeObject(Other1, settings);
            var Other2Serialized = JsonConvert.SerializeObject(Other2, settings);


            var Item1Row = new Mock<IExcelRowParsed>();
            var Item2Row = new Mock<IExcelRowParsed>();

            var Other1Row = new Mock<IExcelRowParsed>();
            var Other2Row = new Mock<IExcelRowParsed>();

            var ItemSheet = new Mock<ISheet>();
            var OtherSheet = new Mock<ISheet>();

            ItemSheet.Setup(s => s.Name).Returns("items");
            OtherSheet.Setup(s => s.Name).Returns("other_data");

            Item1Row.Setup(r => r.RowAsData).Returns(item1Serialized);
            Item1Row.Setup(r => r.Sheet).Returns(ItemSheet.Object);
            Item2Row.Setup(r => r.RowAsData).Returns(item2Serialized);
            Item2Row.Setup(r => r.Sheet).Returns(ItemSheet.Object);


            Other1Row.Setup(r => r.RowAsData).Returns(Other1Serialized);
            Other1Row.Setup(r => r.Sheet).Returns(OtherSheet.Object);
            Other2Row.Setup(r => r.RowAsData).Returns(Other2Serialized);
            Other2Row.Setup(r => r.Sheet).Returns(OtherSheet.Object);

            var row = new Mock<IExcelRowParsed>();
            row.Setup(r => r.RowAsData).Returns(orderSerialized);
            var related = new IExcelRowParsed[] { Item1Row.Object, Item2Row.Object, Other1Row.Object, Other2Row.Object };


            var request = new Mock<ExcelRowValidateOnTypeRequest>();
            request.Setup(r => r.OnType).Returns(typeof(Order));
            request.Setup(r => r.Row).Returns(row.Object);
            request.Setup(r => r.RelatedRows).Returns(related);

            var result = await svc.Validate(request.Object);

            Assert.AreEqual(true, result.IsValid);
        }

        [TestMethod]
        public async Task Invalid()
        {
            var contractResolver = new DefaultContractResolver { NamingStrategy = new SnakeCaseNamingStrategy() };
            var settings = new JsonSerializerSettings { ContractResolver = contractResolver };


            var svc = new ExcelRowTypeValidatorOnAnnotations();

            var order = new Order { Number = "Number", Customer = "A very long customer name that will fail the validation" };
            //var order = new Order { Number = "Number", Customer = "customer" , Weight = new Weight()};

            var item1 = new Item { Name = "first item with a very long text", Quantity = 4 };
            var item2 = new Item { Name = "", Quantity = 10 };

            var Other1 = new Other { Prop1 = "property 1" };
            var Other2 = new Other { Prop1 = "property 2" };

            var orderSerialized = JsonConvert.SerializeObject(order, settings);
            var item1Serialized = JsonConvert.SerializeObject(item1, settings);
            var item2Serialized = JsonConvert.SerializeObject(item2, settings);
            var Other1Serialized = JsonConvert.SerializeObject(Other1, settings);
            var Other2Serialized = JsonConvert.SerializeObject(Other2, settings);


            var Item1Row = new Mock<IExcelRowParsed>();
            var Item2Row = new Mock<IExcelRowParsed>();

            var Other1Row = new Mock<IExcelRowParsed>();
            var Other2Row = new Mock<IExcelRowParsed>();

            var ItemSheet = new Mock<ISheet>();
            var OtherSheet = new Mock<ISheet>();

            ItemSheet.Setup(s => s.Name).Returns("items");
            OtherSheet.Setup(s => s.Name).Returns("other_data");

            Item1Row.Setup(r => r.RowAsData).Returns(item1Serialized);
            Item1Row.Setup(r => r.Sheet).Returns(ItemSheet.Object);
            Item2Row.Setup(r => r.RowAsData).Returns(item2Serialized);
            Item2Row.Setup(r => r.Sheet).Returns(ItemSheet.Object);


            Other1Row.Setup(r => r.RowAsData).Returns(Other1Serialized);
            Other1Row.Setup(r => r.Sheet).Returns(OtherSheet.Object);
            Other2Row.Setup(r => r.RowAsData).Returns(Other2Serialized);
            Other2Row.Setup(r => r.Sheet).Returns(OtherSheet.Object);

            var row = new Mock<IExcelRowParsed>();
            row.Setup(r => r.RowAsData).Returns(orderSerialized);
            var related = new IExcelRowParsed[] { Item1Row.Object, Item2Row.Object, Other1Row.Object, Other2Row.Object };


            var request = new Mock<ExcelRowValidateOnTypeRequest>();
            request.Setup(r => r.OnType).Returns(typeof(Order));
            request.Setup(r => r.Row).Returns(row.Object);
            request.Setup(r => r.RelatedRows).Returns(related);

            var result = await svc.Validate(request.Object);

            Assert.AreEqual(false, result.IsValid);
        }



    }
}