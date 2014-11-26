using System.Linq;
using NUnit.Framework;
using tSQLt.Client.Net.Parsers;

namespace tSQLt.Client.Net.Tests
{
    [TestFixture]
    public class XmlParsingTests
    {
        [Test]
        public void failure_count_is_correct_across_testsuites()
        {
            TestSuites suites =
                XmlParser.Get(@"<testsuites>
          <testsuite name=""uspBillOfMaterials"" tests=""1"" errors=""0"" failures=""1"">            
            <testcase classname=""uspBillOfMaterials"" name=""test does not return level 2 components of a level 0 assembly"">              
                <failure message=""Ouch"" />
            </testcase>
          </testsuite>
          <testsuite name=""cookingTests"" tests=""1"" errors=""0"" failures=""99"">            
            <testcase classname=""uspBillOfMaterials"" name=""test does not return level 2 components of a level 0 assembly"">              
                <failure message=""Ouch Again"" />            
            </testcase>
          </testsuite>
        </testsuites>");

            Assert.AreEqual(100, suites.FailureCount());
        }

        [Test]
        public void failure_is_reported_when_a_test_fails()
        {
            TestSuites suites =
                XmlParser.Get(@"<testsuites>
          <testsuite name=""uspBillOfMaterials"" tests=""1"" errors=""0"" failures=""1"">            
            <testcase classname=""uspBillOfMaterials"" name=""test does not return level 2 components of a level 0 assembly"">
              <failure message=""TODO:Implement this test."" />
            </testcase>
          </testsuite>
        </testsuites>");


            Assert.IsFalse(suites.Passed());
        }

        [Test]
        public void failure_is_reported_when_no_tests_run()
        {
            TestSuites suites =
                XmlParser.Get(@"<testsuites>
                                      <testsuite name=""uspBillOfMaterials"" tests=""0"" errors=""0"" failures=""0"">                        
                                      </testsuite>
                                    </testsuites>");

            Assert.IsFalse(suites.Passed());
        }

        [Test]
        public void failure_messages_are_deserialized()
        {
            TestSuites suites =
                XmlParser.Get(@"<testsuites>
          <testsuite name=""uspBillOfMaterials"" tests=""1"" errors=""0"" failures=""1"">            
            <testcase classname=""uspBillOfMaterials"" name=""test does not return level 2 components of a level 0 assembly"">
              <failure message=""TODO:Implement this test."" />
            </testcase>
          </testsuite>
        </testsuites>");

            TestSuite testSuite = suites.Suites.FirstOrDefault(p => p.Name == "uspBillOfMaterials");

            Assert.IsNotNull(testSuite, "Unable to deseialize test suite");

            Test testResult =
                testSuite
                    .Tests.FirstOrDefault(
                        p =>
                            p.ClassName == "uspBillOfMaterials" &&
                            p.Name == "test does not return level 2 components of a level 0 assembly");


            Assert.AreEqual("TODO:Implement this test.", testResult.Failure.Message);
        }


        [Test]
        public void success_is_reported_when_all_tests_pass()
        {
            TestSuites suites =
                XmlParser.Get(@"<testsuites>
          <testsuite name=""uspBillOfMaterials"" tests=""1"" errors=""0"" failures=""0"">            
            <testcase classname=""uspBillOfMaterials"" name=""test does not return level 2 components of a level 0 assembly"">              
            </testcase>
          </testsuite>
        </testsuites>");


            Assert.IsTrue(suites.Passed());
        }
    }
}