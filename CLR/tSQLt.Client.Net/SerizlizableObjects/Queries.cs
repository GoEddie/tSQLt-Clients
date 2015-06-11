using System;

namespace tSQLt.Client.Net
{
    static class Queries
    {
        public static string GetQueryForAll()
        {
            return "exec tSQLt.RunAll";
        }


        public static string GetQueryForJustResults()
        {
            return "exec tSQLt.XmlResultFormatter";
        }

        public static string GetQueryForSingleTest(string testClass, string name)
        {
            testClass = QuoteName(testClass);
            name = QuoteName(name);

            return String.Format("exec tSQLt.RunWithXmlResults '{0}.{1}'", testClass, name);
        }

        public static string GetQueryForClass(string testClass)
        {
            testClass = QuoteName(testClass);

            return String.Format("exec tSQLt.RunTestClass '{0}'", testClass);
        }

        private static string QuoteName(string name)
        {
            if (!name.StartsWith("["))
                name = '[' + name;

            if (!name.EndsWith("]"))
                name = name + ']';

            return name;
        }
    }
}