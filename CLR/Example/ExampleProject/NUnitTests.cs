using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using tSQLt.Client.Net;

namespace ExampleProject
{
    [TestFixture]
    public class NUnitTests
    {
        /*Set the connection string to the database with your tests in*/
        private readonly tSQLtTestRunner _runner = new tSQLtTestRunner("server=.;integrated security=sspi;initial catalog=tSQLtTests;", 60*1000/*optional timeout, default is 2 minutes*/);

        /*These tests may fail, they are to demonstrate using the .net test client rather than actually passing!*/

        [Test]
        public void sqlcop_test_user_aliases()
        {
            var result = _runner.Run("SQLCop", "test User Aliases");
            Assert.IsTrue(result.Passed());
        }

        [Test]
        public void sqlcop_test_user_aliases_2()
        {
            var result = _runner.Run("SQLCop", "test User Aliases");
            Assert.IsTrue(result.Passed(), result.FailureMessages());
        }

        [Test]
        public void sql_cop_tests()
        {
            var result = _runner.RunClass("SQLCop");
            Assert.IsTrue(result.Passed(), result.FailureMessages());
        }

    }

}
