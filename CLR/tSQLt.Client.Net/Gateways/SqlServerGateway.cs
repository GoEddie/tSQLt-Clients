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

                using (var cmd = con.CreateCommand())
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

                using (var cmd = con.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.CommandTimeout = _runTimeout;

                    var reader = cmd.ExecuteReader();




                    do
                    {
                        if (!reader.Read())
                        {
                            throw new InvalidOperationException(
                                string.Format("Expecting to get a data reader with the response to: \"{0}\" ", query));
                        }
                        var results = reader[0] as string;
                        if (results != null && results.Contains("testsuite"))
                        {
                            return results;
                        }

                    } while (reader.NextResult());

                    return null;


                }
            }
        }
    }
}