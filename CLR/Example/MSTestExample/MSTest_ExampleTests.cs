using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using tSQLt.Client.Net;

namespace MSTestExample
{
    [TestClass]
    public class MSTest_ExampleTests
    {
        /*Set the connection string to the database with your tests in*/

        private readonly tSQLtTestRunner _runner =
            new tSQLtTestRunner("server=.;integrated security=sspi;initial catalog=tSQLtTests;");

        /*These tests may fail, they are to demonstrate using the .net test client rather than actually passing!*/

        [TestMethod]
        public void sqlcop_test_user_aliases()
        {
            var result = _runner.Run("SQLCop", "test User Aliases");
            Assert.IsTrue(result.Passed());


            
        }

        [TestMethod]
        public void sqlcop_test_user_aliases_2()
        {
            var result = _runner.Run("SQLCop", "test User Aliases");
            Assert.IsTrue(result.Passed(), result.FailureMessages());
        }

        [TestMethod]
        public void sql_cop_tests()
        {
            var result = _runner.RunClass("SQLCop");
            Assert.IsTrue(result.Passed(), result.FailureMessages());
        }
    }
}
