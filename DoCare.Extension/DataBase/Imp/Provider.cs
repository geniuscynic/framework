using System.Collections.Generic;
using System.Data;
using DoCare.Extension.DataBase.Utility;

namespace DoCare.Extension.DataBase.Imp
{
    public class Provider
    {
        protected readonly IDbConnection Connection;
        
        protected string DbPrefix;

        protected Dictionary<string, object> SqlParameter;

       

        public Aop Aop { get; set; }

        public Provider(IDbConnection connection):this(connection, new Dictionary<string, object>())
        {
            Connection = connection;

            DbPrefix = DatabaseFactory.GetStatementPrefix(connection);
        }


        public Provider(IDbConnection connection, Dictionary<string, object> SqlParameter)
        {
            this.SqlParameter = SqlParameter;

            Connection = connection;
            
            DbPrefix = DatabaseFactory.GetStatementPrefix(connection);
        }

       
    }
}
