using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace tSQLt.Client.Net
{
    public static class StringExtensions
    {
        public static string UnQuote(this string src)
        {
            return src.Trim('[', ']');
        }
    }
}
