using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SW.ExcelImport.UnitTests.TypeBased
{
    [TestClass]
    public class CreateObjectFromDictionaryTests
    {
        public class SomeParent
        {
            public SomeChild1 Child1 { get; set; }
            public SomeChild2 Child2 { get; set; }
            public string Id { get; set; }
        }

        public class SomeChild1
        {
            public string Prop1 { get; set; }
            public int? Prop2 { get; set; }
        }
        public class SomeChild2
        {
            public string Prop1 { get; set; }
            public string Prop2 { get; set; }
        }
        
        [TestMethod]
        public void AllNullsIgnoredTest()
        {
            var values = new Dictionary<string, object>
            {
                {"Child1.Prop1", null},
                {"Child1.Prop2", null},
                {"Child2.Prop1", null},
                {"Child2.Prop2", "some data"}
            };

            var parseResult = (SomeParent)typeof(SomeParent).CreateFromDictionary(values);
            
            Assert.IsNull(parseResult.Child1);
            Assert.IsNotNull(parseResult.Child2);
            Assert.AreEqual("some data", parseResult.Child2.Prop2);
        }
    }
}