﻿using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using DoCare.Extension.DataBase.Imp.Command.MsSql;
using DoCare.Extension.DataBase.Imp.Command.MySql;
using DoCare.Extension.DataBase.Imp.Command.Oracle;
using DoCare.Extension.DataBase.Imp.Operate;
using DoCare.Extension.DataBase.Imp.Operate.MySqlOperate;
using DoCare.Extension.DataBase.Imp.Operate.OracleOperate;
using DoCare.Extension.DataBase.Imp.Operate.SqlOperate;
using DoCare.Extension.DataBase.Interface.Command;
using DoCare.Extension.DataBase.Interface.Operate;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;

namespace DoCare.Extension.DataBase.Utility
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
            if (provider == DatabaseProvider.MySql.ToString())
            {
                return new MySqlConnection(connectionString);
            }
            if (provider == DatabaseProvider.Oracle.ToString())
            {
                return new OracleConnection(connectionString);
            }

            return null;
        }

        public static string GetStatementPrefix(IDbConnection dbConnection)
        {
            return dbConnection switch
            {
                SqlConnection _ => "@",
                _ => ":"
            };
        }


        public static IInsertable<T> CreateInsertable<T, TEntity>(IDbConnection connection, TEntity model, Aop aop)
        {
            return connection switch
            {
                SqlConnection _ => new SqlInsertable<T, TEntity>(connection, model) { Aop = aop},
                MySqlConnection _ => new MySqlInsertable<T, TEntity>(connection, model) { Aop = aop },
                OracleConnection _ => new OracleInsertable<T, TEntity>(connection, model) { Aop = aop },
                _ => new Insertable<T, TEntity>(connection, model) { Aop = aop }
            };
        }

        public static ISaveable<T> CreateSaveable<T, TEntity>(IDbConnection connection, TEntity model, Aop aop)
        {
            return connection switch
            {
                SqlConnection _ => new SqlSaveable<T, TEntity>(connection, model) { Aop = aop },
                MySqlConnection _ => new MySqlSaveable<T, TEntity>(connection, model) { Aop = aop },
                OracleConnection _ => new OracleSqlSaveable<T,TEntity>(connection, model) { Aop = aop },
                _ => new Saveable<T,TEntity>(connection, model) { Aop = aop }
            };
        }


        public static IUpdateable<T> CreateUpdateable<T>(IDbConnection connection, Aop aop)
        {
            return connection switch
            {
                SqlConnection _ => new SqlUpdateable<T>(connection) { Aop = aop },
                MySqlConnection _ => new MySqlUpdateable<T>(connection) { Aop = aop },
                OracleConnection _ => new OracleUpdateable<T>(connection) { Aop = aop },
                _ => new Updateable<T>(connection) { Aop = aop }
            };
        }

        public static IDoCareQueryable<T> CreateQueryable<T>(IDbConnection connection, Aop aop)
        {
            return connection switch
            {
                SqlConnection _ => new SqlQueryable<T>(connection) { Aop = aop },
                MySqlConnection _ => new MySqlQueryable<T>(connection) { Aop = aop },
                OracleConnection _ => new OracleQueryable<T>(connection) { Aop = aop },
                _ => new Queryable<T>(connection) { Aop = aop }
            };
        }

        public static IDeleteable<T> CreateDeleteable<T>(IDbConnection connection, Aop aop)
        {
            return connection switch
            {
                SqlConnection _ => new SqlDeleteable<T>(connection) { Aop = aop },
                MySqlConnection _ => new MySqlDeleteable<T>(connection) { Aop = aop },
                OracleConnection _ => new OracleDeleteable<T>(connection) { Aop = aop },
                _ => new Deleteable<T>(connection) { Aop = aop }
            };
        }

        public static IReaderableCommand<T> CreateReaderableCommand<T>(IDbConnection connection, StringBuilder sql, Dictionary<string, object> sqlParameter, Aop aop)
        {
            return connection switch
            {
                SqlConnection _ => new MsSqlReaderableCommand<T>(connection, sql, sqlParameter, aop),
                MySqlConnection _ => new MySqlReaderableCommand<T>(connection, sql, sqlParameter, aop),
                OracleConnection _ => new OracleReaderableCommand<T>(connection, sql, sqlParameter, aop),
                _ => new OracleReaderableCommand<T>(connection, sql, sqlParameter, aop) 
            };
        }

    }
}