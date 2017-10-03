using System;
using System.Data.SqlClient;
using System.Text;

namespace tSQLt.Client.Net.Gateways
{
    public interface ISqlServerGateway
    {
        string RunWithXmlResult(string query);
        void RunWithNoResult(string query);
        DataReaderResult RunWithDataReader(string query);
    }

    public struct DataReaderResult
    {
        public SqlConnection Connection;
        public SqlCommand Command;
        public SqlDataReader Reader;
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
        public DataReaderResult RunWithDataReader(string query)
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = query;
            command.CommandType = System.Data.CommandType.Text;

            var result = new DataReaderResult();
            result.Connection = connection;
            result.Command = command;
            result.Reader = command.ExecuteReader();
            return result;
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

                    var builder = new StringBuilder();

                    do
                    {
                        while (reader.Read())
                        {
                            var part = reader[0] as string;
                            if (!String.IsNullOrEmpty(part))
                            {
                                builder.Append(part);
                            }
                        }
                    } while (reader.NextResult());

                    var results = builder.ToString();

                    if (results.Contains("testsuite"))
                    {
                        return results;
                    }

                    return null;


                }
            }
        }
    }
}