using System;
using System.Net;
using System.Web;
using tSQLt.Client.Net.Gateways;
using tSQLt.Client.Net.Parsers;

namespace tSQLt.Client.Net
{
    public class tSQLtTestRunner
    {
        private readonly ISqlServerGateway _gateway;

        /// <summary>
        /// Creates a tSQLtTestRunner which is used to run tests against a Sql Server database which contains the tests to run
        /// </summary>
        /// <param name="connectionString">The connection string including the initial catalog of the database to connect to</param>
        public tSQLtTestRunner(string connectionString)
        {
            _gateway = new SqlServerGateway(connectionString);
        }

        /// <summary>
        /// This is typically only used to test the API. Use the "tSQLtTestRunner(string connectionString)" constructor instead
        /// </summary>
        /// <param name="gateway"></param>
        public tSQLtTestRunner(ISqlServerGateway gateway /*Typically only used for testing the API*/)
        {
            _gateway = gateway;
        }

        /// <summary>
        /// Execute the tSQLt test that is in the schema "testClass" with the name "name"
        /// 
        /// For Example to run the SQLCop test "test User Aliases" you would pass in:
        /// 
        ///     testClass = "SQLCop"
        ///     name      = "test User Aliases"
        ///
        /// </summary>
        /// <param name="testClass">The name of the schema the test lives in</param>
        /// <param name="name">The name of the test to run</param>
        /// <returns>TestSuites - The Results of the test</returns>
        public TestSuites Run(string testClass, string name)
        {
            return GetResults(Queries.GetQueryForSingleTest(testClass, name));
        }

        /// <summary>
        /// Executes all of the tSQLt tests that are within the schema "testClass"
        /// 
        /// For Example to run all of the SQLCop tests you would pass in:
        /// 
        ///     testClass = "SQLCop"
        /// 
        /// </summary>
        /// <param name="testClass">The name of the schema holding all the tests you would like to run</param>
        /// <returns></returns>
        public TestSuites RunClass(string testClass)
        {
            return GetResults(Queries.GetQueryForClass(testClass), Queries.GetQueryForJustResults());
        }

        /// <summary>
        /// Executes all of the tSQLt tests that are within the database the tSQLtTestRunner is connected to
        /// </summary>
        /// <returns></returns>
        public TestSuites RunAll()
        {
            return GetResults(Queries.GetQueryForAll(), Queries.GetQueryForJustResults());
        }

        private TestSuites GetResults(string queryRun, string queryResults)
        {
            string xml = "";

            try
            {
                _gateway.RunWithNoResult(queryRun);
                xml = _gateway.RunWithXmlResult(queryResults);
            }
            catch (Exception ex)
            {
                xml = FailureMessageXml(ex.Message);
            }

            return XmlParser.Get(xml);
        }

        private TestSuites GetResults(string query)
        {
            string xml = "";

            try
            {
                xml = _gateway.RunWithXmlResult(query);
            }
            catch (Exception ex)
            {
                xml = FailureMessageXml(ex.Message);
            }

            return XmlParser.Get(xml);
        }

        private string FailureMessageXml(string message)
        {
            return string.Format(@"<testsuites><testsuite name=""failure"" tests=""1"" errors=""1"" failures=""1""> 
                <testcase classname=""failure"" name=""message"">              
                <failure message=""{0}"" />
            </testcase></testsuite></testsuites> ", WebUtility.HtmlEncode(message.Replace("\r", "").Replace("\n", "")));

        }

    }
}