﻿using System.Data;

namespace DoCare.Extension.Dao.Imp.Operate.MySqlOperate
{
    public class MySqlInsertable<T, TEntity>  : Insertable<T, TEntity>
    {
       
        //private string sql = "insert into {0}  values ({1});";
        public MySqlInsertable(IDbConnection connection, TEntity model):base(connection,model)
        {
            
        }

    }
}
