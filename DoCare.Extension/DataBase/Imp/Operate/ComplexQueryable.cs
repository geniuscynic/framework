using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using DoCare.Extension.DataBase.Imp.Command;
using DoCare.Extension.DataBase.Interface.Command;
using DoCare.Extension.DataBase.Interface.Operate;
using DoCare.Extension.DataBase.SqlProvider;
using DoCare.Extension.DataBase.Utility;

namespace DoCare.Extension.DataBase.Imp.Operate
{
     

    public class ComplexQueryable<T> :  Provider, IComplexQueryable<T>
    {
        protected readonly string _alias;
        protected readonly WhereCommand whereCommand;
        private readonly IOrderByCommand<T> orderByCommand;

        private readonly StringBuilder _selectField = new StringBuilder();
        protected StringBuilder _joinSql = new StringBuilder();
        //private readonly StringBuilder _selectField = new StringBuilder();


      

        //private readonly  StringBuilder _sortSql = new StringBuilder();

        public ComplexQueryable(IDbConnection connection, string alias) : base(connection)
        {
            _alias = alias;
            whereCommand = new WhereCommand(SqlParameter);

            orderByCommand = new OrderByCommand<T>();
        }

       
        public IComplexQueryable<T, T2> Join<T2>(string alias, Expression<Func<T, T2, bool>> predicate)
        {
            var joinCommand = new JoinCommand(alias, SqlParameter);
            joinCommand.Join(predicate);

            _joinSql.Append(joinCommand.Build<T2>());
            
            return new ComplexQueryable<T, T2>(Connection, _alias, _joinSql, SqlParameter);
        }

        public IComplexQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            whereCommand.Where(predicate);

            return this;

        }

        public IComplexQueryable<T> Where(string whereExpression)
        {
            whereCommand.Where(whereExpression);

            return this;
        }

        public IComplexQueryable<T> Where<TEntity>(string whereExpression, Expression<Func<TEntity>> predicate)
        {
            whereCommand.Where(whereExpression, predicate);

            return this;

        }

        public IComplexQueryable<T> OrderBy<TResult>(Expression<Func<T, TResult>> predicate)
        {
            orderByCommand.OrderBy(predicate);


            return this;
        }

        public IComplexQueryable<T> OrderByDesc<TResult>(Expression<Func<T, TResult>> predicate)
        {
            orderByCommand.OrderByDesc(predicate);

            return this;
        }

        public IReaderableCommand<TResult> Select<TResult>(Expression<Func<T, TResult>> predicate)
        {
            var provider = new SelectProvider();
            provider.Visit(predicate);

            provider.SelectFields.ForEach(t =>
            {
                _selectField.Append($"{t.Prefix}.{t.ColumnName} as {t.Parameter},");

                //prefix = t.Prefix;
            });
            _selectField.Remove(_selectField.Length - 1, 1);
            return DatabaseFactory.CreateReaderableCommand<TResult>(Connection, Build(), SqlParameter, Aop);
        }


        private StringBuilder Build()
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


            sql.Append(whereCommand.Build(false).Replace(DatabaseFactory.ParamterSplit, DbPrefix));

            sql.Append(orderByCommand.Build(false));

            return sql;
        }

      


        public async Task<IEnumerable<T>> ExecuteQuery()
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build(), SqlParameter, Aop);

            return await command.ExecuteQuery();
        }

        public async Task<T> ExecuteFirst()
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build(), SqlParameter, Aop);

            return await command.ExecuteFirst();
        }

        public async Task<T> ExecuteFirstOrDefault()
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build(), SqlParameter, Aop);

            return await command.ExecuteFirstOrDefault();
        }

        public async Task<T> ExecuteSingle()
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build(), SqlParameter, Aop);

            return await command.ExecuteSingle();
        }

        public async Task<T> ExecuteSingleOrDefault()
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build(), SqlParameter, Aop);

            return await command.ExecuteSingleOrDefault();
        }

        public async Task<(IEnumerable<T> data, int total)> ToPageList(int pageIndex, int pageSize)
        {
            var command = DatabaseFactory.CreateReaderableCommand<T>(Connection, Build(), SqlParameter, Aop);

            return await command.ToPageList(pageIndex, pageSize);

        }

    }

    public class ComplexQueryable<T1, T2> : ComplexQueryable<T1>, IComplexQueryable<T1, T2>
    {
        //private readonly StringBuilder _join;

        //private readonly Dictionary<string, object> _sqlParameter;

       // private IWhereCommand<T1, T2> _whereCommand;
        //public ComplexQueryable(IDbConnection connection) : base(connection)
        //{
        //}

        public ComplexQueryable(IDbConnection connection, string _alias, StringBuilder join, Dictionary<string, object> sqlParameter) : base(connection, _alias)
        {
            _joinSql = join;
            SqlParameter = sqlParameter;

            //_whereCommand = new WhereCommand<T1, T2>(SqlParameter);
        }

        public IComplexQueryable<T1, T2, T3> Join<T3>(string alias, Expression<Func<T1, T2, T3, bool>> predicate)
        {

            var joinCommand = new JoinCommand(alias, SqlParameter);
            joinCommand.Join(predicate);

            _joinSql.Append(joinCommand.Build<T3>());
            
            return new ComplexQueryable<T1, T2, T3>(Connection, _alias, _joinSql, SqlParameter);
        }

        public IComplexQueryable<T1, T2> Where(Expression<Func<T1, T2, bool>> predicate)
        {
            whereCommand.Where(predicate);

            return this;
        }


        

        public IComplexQueryable<T1, T2> OrderBy<TResult>(Expression<Func<T1, T2, TResult>> predicate)
        {
            throw new NotImplementedException();
        }

        public IComplexQueryable<T1, T2> OrderByDesc<TResult>(Expression<Func<T1, T2, TResult>> predicate)
        {
            throw new NotImplementedException();
        }

       
    }


    public class ComplexQueryable<T1, T2, T3> : ComplexQueryable<T1, T2>, IComplexQueryable<T1, T2, T3>
    {
        //private readonly StringBuilder _join;

        private readonly Dictionary<string, object> _sqlParameter;

      

        public ComplexQueryable(IDbConnection connection, string _alias, StringBuilder join, Dictionary<string, object> sqlParameter) : base(connection, _alias, join, sqlParameter)
        {
           // _joinSql = join;
           // _sqlParameter = sqlParameter;
        }

        public IComplexQueryable<T1, T2, T3> Where(Expression<Func<T1, T2, T3, bool>> predicate)
        {
            whereCommand.Where(predicate);

            return this;
        }

        public IComplexQueryable<T1, T2, T3> OrderBy<TResult>(Expression<Func<T1, T2, T3, TResult>> predicate)
        {
            throw new NotImplementedException();
        }

        public IComplexQueryable<T1, T2, T3> OrderByDesc<TResult>(Expression<Func<T1, T2, T3, TResult>> predicate)
        {
            throw new NotImplementedException();
        }

       
    }
}
