using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

using DoCare.Extension.SqlHelper.Imp.Command.Oracle;
using DoCare.Extension.SqlHelper.Imp.Operate;

using DoCare.Extension.SqlHelper.Imp.Operate.OracleOperate;

using DoCare.Extension.SqlHelper.Interface.Command;
using DoCare.Extension.SqlHelper.Interface.Operate;
using Oracle.ManagedDataAccess.Client;

namespace DoCare.Extension.SqlHelper.Utility
{
    public class DatabaseFactory
    {
        public const string ParamterSplit = "@@@";

        public static IDbConnection CreateConnection(string connectionString, string provider)
        {
            if (provider == DatabaseProvider.MsSql.ToString())
            {
                return new SqlConnection(connectionString);
            }
            
            if (provider == DatabaseProvider.Oracle.ToString())
            {
                return new OracleConnection(connectionString);
            }

            return null;
        }

        public static string GetStatementPrefix(IDbConnection dbConnection)
        {
            return ":";
        }


        public static IInsertable<T> CreateInsertable<T, TEntity>(IDbConnection connection, TEntity model, Aop aop)
        {
            
            return new OracleInsertable<T, TEntity>(connection, model) { Aop = aop };
        }

        public static ISaveable<T> CreateSaveable<T, TEntity>(IDbConnection connection, TEntity model, Aop aop)
        {
            return new OracleSqlSaveable<T, TEntity>(connection, model) { Aop = aop };
        }


        public static IUpdateable<T> CreateUpdateable<T>(IDbConnection connection, Aop aop)
        {
            return new OracleUpdateable<T>(connection) { Aop = aop };
        }

        public static IDoCareQueryable<T> CreateQueryable<T>(IDbConnection connection, Aop aop)
        {
            return new OracleQueryable<T>(connection) { Aop = aop };
        }

        public static IDeleteable<T> CreateDeleteable<T>(IDbConnection connection, Aop aop)
        {
            return new OracleDeleteable<T>(connection) { Aop = aop };
        }

        public static IReaderableCommand<T> CreateReaderableCommand<T>(IDbConnection connection, StringBuilder sql, Dictionary<string, object> sqlParameter, Aop aop)
        {
            return new OracleReaderableCommand<T>(connection, sql, sqlParameter, aop);
        }

    }
}
