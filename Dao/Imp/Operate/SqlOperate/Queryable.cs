﻿using System.Data;

namespace DoCare.Extension.Dao.Imp.Operate.SqlOperate
{
    public class SqlQueryable<T> : Queryable<T>
    {
        public SqlQueryable(IDbConnection connection):base(connection)
        {
            

        }

      
    }
}
