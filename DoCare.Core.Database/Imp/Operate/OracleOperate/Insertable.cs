﻿using System.Data;

namespace DoCare.Core.Database.Imp.Operate.OracleOperate
{
    public class OracleInsertable<T, TEntity>  : Insertable<T, TEntity>
    {
       
       
        public OracleInsertable(IDbConnection connection, TEntity model) : base(connection, model)
        {
            
        }

       
    }
}
