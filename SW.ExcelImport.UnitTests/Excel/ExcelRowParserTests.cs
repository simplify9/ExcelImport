using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using SW.ExcelImport;
using SW.ExcelImport.Model;
using SW.ExcelImport.Services;
using Moq;



namespace SW.ExcelImport.UnitTests.Excel
{
    
    [TestClass]
    public class ExcelRowParserTests
    {
        public class Dto
        {
            public int MyProperty { get; set; }
        }
        [TestMethod]
        public void ParseIdTes()
        {
            var repo = new Mock<IExcelRepo>().Object;
            var row = new Mock<IExcelRow>();
            row.Setup(x=> x.Cells).Returns(new ICell[] {});
            //var parser = new ExcelRowParser(repo);
            //var request = new Mock<IExcelRowParseRequest>().Setup(x=> x.)
            //parser.ParseId()
            

        }
    }
}