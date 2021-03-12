using System.Collections.Generic;
using System.Data;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DoCare.Core.Database.Imp.Command;
using DoCare.Core.Database.Interface;
using DoCare.Core.Database.Interface.Command;
using DoCare.Core.Database.Interface.Operate;
using DoCare.Core.Database.Utility;


namespace DoCare.Core.Database.Imp.Operate
{
    public class Insertable<T, TEntity> : Provider, IInsertable<T>
    {

        protected readonly TEntity _model;

        public Insertable(IDbConnection connection, TEntity model) : base(connection)
        {
            _model = model;
        }

        private StringBuilder Build()
        {
            var columnList = new List<string>();
            var parameterList = new List<string>();

            var type = typeof(T);
           
            var (tableName, properties) = Utility.ProviderHelper.GetMetas(type);

            foreach (var p in properties)
            {
                if (p.IsIdentity)
                {
                    continue;
                }

                columnList.Add(p.ColumnName);
                parameterList.Add($"{DbPrefix}{p.Parameter}");
                SqlParameter.Add(p.Parameter, p.PropertyInfo.GetValue(_model));
                //Console.WriteLine("Name:{0} Value:{1}", p.Name, p.GetValue(_model));
            }

            StringBuilder sql = new StringBuilder();

            sql.Append($"insert into {tableName} ({string.Join(",", columnList)}) values ({string.Join(",", parameterList)})");

            return sql;
        }


        public async Task<int> Execute()
        {
            var command = new WriteableCommand(Connection, Build().ToString(), SqlParameter, Aop);
          
            return await command.Execute();
        }

      
    }
}
