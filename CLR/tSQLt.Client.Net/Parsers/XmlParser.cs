using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace tSQLt.Client.Net.Parsers
{
    public static class XmlParser
    {
        public static TestSuites Get(string xml)
        {
            try
            {
                var serializer = new XmlSerializer(typeof (TestSuites));
                return serializer.Deserialize(XmlReader.Create(new StringReader(xml))) as TestSuites;
            }
            catch (Exception ex)
            {
                return new TestSuites()
                {
                    Suites = new List<TestSuite>() { new TestSuite(ex.Message) }
                };
            }
        }
    }
}