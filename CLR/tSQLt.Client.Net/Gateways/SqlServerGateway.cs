using System;
using System.Data.SqlClient;

namespace tSQLt.Client.Net.Gateways
{
    public interface ISqlServerGateway
    {
        string RunWithXmlResult(string query);
        void RunWithNoResult(string query);
    }

    public class SqlServerGateway : ISqlServerGateway
    {
        
        private readonly string _connectionString;
        private readonly int _runTimeout;

        public SqlServerGateway(string connectionString, int runTimeout)
        {
            _connectionString = connectionString;
            _runTimeout = runTimeout;
        }

        public void RunWithNoResult(string query)
        {

            using (var con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.CommandTimeout = _runTimeout;
                        cmd.ExecuteNonQuery();
                    }
                }
        }
        
        public string RunWithXmlResult(string query)
        {
            using (var con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;
                        cmd.CommandTimeout = _runTimeout;

                        SqlDataReader reader = cmd.ExecuteReader();
                        if (!reader.Read())
                        {
                            throw new InvalidOperationException(
                                string.Format("Expecting to get a data reader with the response to: \"{0}\" ", query));
                        }

                        var result =  reader[0] as string;
                        var testCount = 0;
                        if (Int32.TryParse(result, out testCount))
                        {
                            if (reader.NextResult())
                            {
                                if (reader.Read())
                                {
                                    return reader[0] as string;
                                }
                            }
                        }

                        return result;
                    }
                }            
        }

    }
}