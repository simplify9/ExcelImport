using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

//[assembly: Parallelize(Workers = 1)]

namespace SW.Pmm.UnitTests
{
    [TestClass]
    public class Initialize
    {
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext context)
        {
        }

        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            //TestServerClientFactory.GetServer().Dispose();
        }
    }
}
