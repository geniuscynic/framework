using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DoCare.Extension.SqlHelper.Utility;
using DoCare.ToolKit;
using Newtonsoft.Json;


namespace DoCare.Extension.SqlHelper
{
    public class BaseSqlHelperDA<T>
    {
        private static readonly Logger logger = Logger.GetLogger(typeof(BaseSqlHelperDA<T>));
        
        public const string OracleClientProvider = "system.data.oracleclient";
        public const string SqlClientProvider = "system.data.sqlclient";
        public const string OleDbProvider = "system.data.oledb";
        public const string DevartOracleClientProvider = "devart.data.oracle";

        protected Dbclient dbClient;
        public BaseSqlHelperDA() : this("docare")
        {

        }

        public BaseSqlHelperDA(string id)
        {
            ConnectionStringSettings connectionString = ConfigurationManager.ConnectionStrings[id];
            if (connectionString == null)
                throw new ConfigurationErrorsException(string.Format("配制文件中找不到名为{0}的连接字符串", id));

            var provider = "";
            switch (connectionString.ProviderName.ToLower())
            {
                case SqlClientProvider:
                    provider = DatabaseProvider.MsSql.ToString();
                    break;

                default:
                    provider = DatabaseProvider.Oracle.ToString();
                    break;
            }

            dbClient = new Dbclient(connectionString.ConnectionString, provider);

            dbClient.Aop = new Aop()
            {
                OnError = (sql, paramter) =>
                {

                    logger?.Error($"Sql:  {sql}, \r\n paramter: {JsonConvert.SerializeObject(paramter)}");
                    //Console.WriteLine(sql);
                },
                OnExecuting = (sql, paramter) =>
                {
                    //Console.WriteLine(sql);
                    logger?.InfoFormat($"Sql:  {sql}, \r\n paramter: {JsonConvert.SerializeObject(paramter)}");
                },

            };
        }


        public virtual int Add(T entity)
        {
            return dbClient.Insertable(entity).Execute().Result;
        }


        public virtual int Add(List<T> entitys)
        {
            return dbClient.Insertable(entitys).Execute().Result;
        }

        public int Delete()
        {
            return dbClient.Deleteable<T>().Execute().Result;
        }

        public int Delete(Expression<Func<T, bool>> predicate)
        {
            return dbClient.Deleteable<T>().Where(predicate).Execute().Result;
        }

        public int Delete(string where)
        {
            return dbClient.Deleteable<T>().Where(where).Execute().Result;
        }

        public int Delete<TEntity>(string whereExpression, Expression<Func<TEntity>> predicate)
        {
            return dbClient.Deleteable<T>().Where(whereExpression, predicate).Execute().Result;
        }


        public int Update<TEntity>(Expression<Func<TEntity>> predicate, Expression<Func<T, bool>> where)
        {
            return dbClient.Updateable<T>().SetColumns(predicate).Where(where).Execute().Result;
        }


        public int Update<TEntity>(Expression<Func<TEntity>> predicate, string where)
        {
            return dbClient.Updateable<T>().SetColumns(predicate).Where(where).Execute().Result;
        }

        
        public int Save(T entity)
        {
            return dbClient.Saveable<T>(entity).Execute().Result;
        }

       


    }
}
