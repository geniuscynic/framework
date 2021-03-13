using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using DoCare.Zkzx.Core.Database;

namespace CodeGenerator
{

    public class TableDefind
    {
        public string ColumnName { get; set; }
        public string DataTypeName { get; set; }

        public int ColumnOrder { get; set; }

        public int Length { get; set; }

        public int IsNullable { get; set; }

        public int IsIdentity { get; set; }
    }

    public class CodeGenerationTools
    {
        private Dbclient client;
        public CodeGenerationTools(string connectionString, string provider)
        {
            client = new Dbclient(connectionString, provider);
        }

        public string MsSql = @"select c.name as ColumnName,
c.colorder as ColumnOrder,
c.xtype as DataType,
typ.name as  DataTypeName,
c.Length, c.isnullable as IsNullable,
COLUMNPROPERTY (c.id, c.name,  'IsIdentity ') as IsIdentity
from dbo.syscolumns c 
inner join dbo.sysobjects t
on c.id = t.id
inner join dbo.systypes typ on typ.xtype = c.xtype
where OBJECTPROPERTY(t.id, N'IsUserTable') = 1
and t.name='{0}' order by c.colorder;";

    

        public async Task<IEnumerable<TableDefind>> GetTableDefine(string tableName)
        {
            return await client.Queryable<TableDefind>(string.Format(MsSql, tableName)).ExecuteQuery();
            //return  await client.SimpleQueryable<TableDefind>(MsSql).ExecuteQuery();
        }

       
    }
}
