using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DoCare.Extension.SqlHelper.Imp.Command;
using DoCare.Extension.SqlHelper.Interface.Command;
using DoCare.Extension.SqlHelper.Interface.Operate;
using DoCare.Extension.SqlHelper.SqlProvider;
using DoCare.Extension.SqlHelper.Utility;

namespace DoCare.Extension.SqlHelper.Imp.Operate
{
    public class QueryableProvider :  Provider, IQueryableProvider
    {
        protected readonly string _alias;
        private readonly WhereCommand _whereCommand;
        private readonly IOrderByCommand _orderByCommand;

        private readonly StringBuilder _selectField = new StringBuilder();
        private StringBuilder _joinSql = new StringBuilder();

        public QueryableProvider(IDbConnection connection, string alias) : base(connection)
        {
            _alias = alias;
            _whereCommand = new WhereCommand(SqlParameter);

            _orderByCommand = new OrderByCommand();
        }

        public void Join<T1, T2>(string alias, Expression<Func<T1, T2, bool>> predicate)
        {
            var joinCommand = new JoinCommand(alias, SqlParameter);
            joinCommand.Join(predicate);

            _joinSql.Append(joinCommand.Build<T2>());
        }

        public void Join<T1, T2, T3>(string alias, Expression<Func<T1, T2, T3, bool>> predicate)
        {
            var joinCommand = new JoinCommand(alias, SqlParameter);
            joinCommand.Join(predicate);

            _joinSql.Append(joinCommand.Build<T3>());
        }

        public void Where<T>(Expression<Func<T, bool>> predicate)
        {
            _whereCommand.Where(predicate);
        }

        public void Where<T1, T2>(Expression<Func<T1, T2, bool>> predicate)
        {
            _whereCommand.Where(predicate);
        }

        public void Where<T1, T2, T3>(Expression<Func<T1, T2, T3, bool>> predicate)
        {
            _whereCommand.Where(predicate);
        }

        public void Where(string whereExpression)
        {
            _whereCommand.Where(whereExpression);
        }

        public void Where<TResult>(string whereExpression, Expression<Func<TResult>> predicate)
        {
            _whereCommand.Where(whereExpression, predicate);
        }

        public void OrderBy<T, TResult>(Expression<Func<T, TResult>> predicate)
        {
            _orderByCommand.OrderBy(predicate);
        }

        public void OrderBy<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> predicate)
        {
            _orderByCommand.OrderBy(predicate);
        }

        public void OrderBy<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> predicate)
        {
            _orderByCommand.OrderBy(predicate);
        }

        public void OrderByDesc<T, TResult>(Expression<Func<T, TResult>> predicate)
        {
            _orderByCommand.OrderByDesc(predicate);
        }

        public void OrderByDesc<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> predicate)
        {
            _orderByCommand.OrderByDesc(predicate);
        }

        public void OrderByDesc<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> predicate)
        {
            _orderByCommand.OrderByDesc(predicate);
        }

        private IReaderableCommand<TResult> VisitSelect<T, TResult>(Expression predicate)
        {
            var provider = new SelectProvider();
            provider.Visit(predicate);

            provider.SelectFields.ForEach(t =>
            {
                _selectField.Append($"{t.Prefix}.{t.ColumnName} as {t.Parameter},");

                //prefix = t.Prefix;
            });
            _selectField.Remove(_selectField.Length - 1, 1);
            return DatabaseFactory.CreateReaderableCommand<TResult>(Connection, Build<T>(), SqlParameter, Aop);
        }

        public IReaderableCommand<TResult> Select<T, TResult>(Expression<Func<T, TResult>> predicate)
        {
            return VisitSelect<T,TResult>(predicate);
        }


        public IReaderableCommand<TResult> Select<T1, T2, TResult>(Expression<Func<T1, T2, TResult>> predicate)
        {
            return VisitSelect<T1,TResult>(predicate);
        }

        public IReaderableCommand<TResult> Select<T1, T2, T3, TResult>(Expression<Func<T1, T2, T3, TResult>> predicate)
        {
            return VisitSelect<T1, TResult>(predicate);
        }


        private StringBuilder Build<T>()
        {
            //prefix = whereCommand.prefix;

            var sql = new StringBuilder();

            var type = typeof(T);
            var (tableName, properties) = ProviderHelper.GetMetas(type);

            var selectSql = new StringBuilder();
            if (_selectField.Length > 0)
            {
                selectSql.Append(_selectField);
            }
            else
            {
                foreach (var property in properties)
                {
                    selectSql.Append($"{_alias}.{property.ColumnName} as {property.Parameter},");
                }

                selectSql.Remove(selectSql.Length - 1, 1);
            }


            sql.Append($"select {selectSql} from {tableName} {_alias} {_joinSql}");


            sql.Append(_whereCommand.Build(false).Replace(DatabaseFactory.ParamterSplit, DbPrefix));

            sql.Append(_orderByCommand.Build(false));

            return sql;
        }


        public async Task<IEnumerable<T>> ExecuteQuery<T>()
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build<T>(), SqlParameter, Aop);

            return await command.ExecuteQuery();
        }

        public async Task<T> ExecuteFirst<T>()
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build<T>(), SqlParameter, Aop);

            return await command.ExecuteFirst();
        }

        public async Task<T> ExecuteFirstOrDefault<T>()
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build<T>(), SqlParameter, Aop);

            return await command.ExecuteFirstOrDefault();
        }

        public async Task<T> ExecuteSingle<T>()
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build<T>(), SqlParameter, Aop);

            return await command.ExecuteSingle();
        }

        public async Task<T> ExecuteSingleOrDefault<T>()
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build<T>(), SqlParameter, Aop);

            return await command.ExecuteSingleOrDefault();
        }

        public async Task<(IEnumerable<T> data, int total)> ToPageList<T>(int pageIndex, int pageSize)
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build<T>(), SqlParameter, Aop);

            return await command.ToPageList(pageIndex, pageSize);

        }

    }

}
