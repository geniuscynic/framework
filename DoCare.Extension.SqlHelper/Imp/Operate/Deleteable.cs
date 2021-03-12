using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DoCare.Extension.SqlHelper.Imp.Command;
using DoCare.Extension.SqlHelper.Interface.Command;
using DoCare.Extension.SqlHelper.Interface.Operate;
using DoCare.Extension.SqlHelper.Utility;

namespace DoCare.Extension.SqlHelper.Imp.Operate
{
    public class Deleteable<T> : Provider, IDeleteable<T>
    {
        private readonly  IWhereCommand<T> whereCommand;

        public Deleteable(IDbConnection connection) : base(connection)
        {
            whereCommand = new WhereCommand<T>(SqlParameter);
        }

        public IDeleteable<T> Where(Expression<Func<T, bool>> predicate)
        {
            whereCommand.Where(predicate);

            return this;
        }

        public IDeleteable<T> Where(string whereExpression)
        {
            whereCommand.Where(whereExpression);

            return this;
        }

        public IDeleteable<T> Where<TEntity>(string whereExpression, Expression<Func<TEntity>> predicate)
        {
            whereCommand.Where(whereExpression, predicate);

            return this;
          
        }

        public string Build()
        {
            var sql = new StringBuilder();

            var type = typeof(T);
            var (tableName, properties) = ProviderHelper.GetMetas(type);

            sql.Append($"delete from {tableName}  ");

            //sql.Append(" where ");
            var where = whereCommand.Build().Replace(DatabaseFactory.ParamterSplit, DbPrefix);

            if (where.Length > 0)
            {
                sql.Append(where);
            }
            else
            {
                sql.Append(" where ");

                foreach (var member in properties.Where(t => t.IsPrimaryKey))
                {
                    sql.Append($" {member.ColumnName} = {DbPrefix}{member.Parameter} and");
                }

                sql.Remove(sql.Length - 3, 3);
            }



            //sql.Remove(sql.Length - 3, 3);


            return sql.ToString();
        }

        public async Task<int> Execute()
        {
            var command = new WriteableCommand(Connection, Build().ToString(), SqlParameter, Aop);

            return await command.Execute();

        }


       
    }
}
