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

        public SqlServerGateway(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void RunWithNoResult(string query)
        {

            using (var con = new SqlConnection(_connectionString))
                {
                    con.Open();

                    using (SqlCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = query;
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
                        SqlDataReader reader = cmd.ExecuteReader();
                        if (!reader.Read())
                        {
                            throw new InvalidOperationException(
                                string.Format("Expecting to get a data reader with the response to: \"{0}\" ", query));
                        }

                        return reader[0] as string;
                    }
                }
            
        }

    }
}