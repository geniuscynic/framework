using System;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DoCare.Extension.DataBase.Imp.Command;
using DoCare.Extension.DataBase.Interface.Command;
using DoCare.Extension.DataBase.Interface.Operate;
using DoCare.Extension.DataBase.Utility;

namespace DoCare.Extension.DataBase.Imp.Operate
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
            var (tableName, _) = ProviderHelper.GetMetas(type);

            sql.Append($"delete from {tableName}  ");

            //sql.Append(" where ");

            sql.Append(whereCommand.Build().Replace(DatabaseFactory.ParamterSplit, DbPrefix));

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
