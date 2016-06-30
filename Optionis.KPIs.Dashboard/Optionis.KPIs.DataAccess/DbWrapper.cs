using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;

namespace Optionis.KPIs.DataAccess
{
    class DbWrapper
    {
        static readonly string _connectionString = ConfigurationManager.ConnectionStrings["DeploymentsDb"].ConnectionString;

        public int ExecuteScalar(string sql, object sqlParams = null)
        {
            int result;
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                result = sqlConnection.ExecuteScalar<int>(sql, sqlParams);
                sqlConnection.Close();
            }
            return result;
        }

        public T GetSingle<T>(string sql, object sqlParams = null) where T : class
        {
            return Query(sqlConnection => sqlConnection.Query<T>(sql, sqlParams).FirstOrDefault());
        }
    
        public IEnumerable<T> Get<T>(string sql, object sqlParams = null) where T : class
        {
            return Query(sqlConnection => sqlConnection.Query<T>(sql, sqlParams));
        }

        static T Query<T>(Func<IDbConnection, T> func) where T : class
        {
            T result = null;
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                result = func(sqlConnection);
                sqlConnection.Close();
            }
            return result;
        }
    }
}