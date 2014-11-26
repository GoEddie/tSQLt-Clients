using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using tSQLt.Client.Net.Gateways;

namespace tSQLt.Client.Net.Tests
{
    [TestFixture]
    public class RunTests
    {
        const string DefaultXml = @"<testsuites><testsuite name=""Test Failed"" tests=""1"" errors=""0"" failures=""0""/><testcase classname=""blah"" name=""bloo""/></testsuites>";

        [Test]
        public void single_test_runs_query_with_xml_results()
        {
            const string expected_format = "exec tSQLtTestRunner.RunWithXmlResults [{0}].[{1}]";
            const string className = "this is the class";
            const string testName = "testipoos blah blah";

            string expected = string.Format(expected_format, className, testName);

            var gateway = new Mock<ISqlServerGateway>();
            gateway.Setup(p => p.RunWithXmlResult(It.IsAny<string>())).Returns<string>((query) =>
            {
                Assert.AreEqual(expected, query);
                return DefaultXml;
            });


            var tester = new tSQLtTestRunner(gateway.Object);
            var result = tester.Run(className, testName);

            Assert.AreEqual(1, result.TestCount());
        }



        [Test]
        public void exceptions_in_the_gateway_result_in_errors_being_returned()
        {

            const string expected_format = "exec tSQLtTestRunner.RunWithXmlResults [{0}].[{1}]";
            const string className = "this is the class";
            const string testName = "testipoos blah blah";

            string expected = string.Format(expected_format, className, testName);

            var gateway = new Mock<ISqlServerGateway>();
            gateway.Setup(p => p.RunWithXmlResult(It.IsAny<string>())).Returns<string>((query) =>
            {
                throw new InvalidOperationException("blah blah blah");
            });
            
            var tester = new tSQLtTestRunner(gateway.Object);
            var result = tester.Run(className, testName);

            Assert.IsFalse(result.Passed());
            
        }
    }
}

