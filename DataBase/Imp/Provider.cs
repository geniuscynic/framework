using System.Collections.Generic;
using System.Data;
using DoCare.Extension.DataBase.Utility;

namespace DoCare.Extension.DataBase.Imp
{
    public class Provider
    {
        protected readonly IDbConnection Connection;
        
        protected string DbPrefix;

        protected Dictionary<string, object> SqlParameter = new Dictionary<string, object>();

       

        public Aop Aop { get; set; }


        public Provider(IDbConnection connection)
        {
            Connection = connection;
            
            DbPrefix = DatabaseFactory.GetStatementPrefix(connection);
        }

       
    }
}
