﻿using System;
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
    public class Queryable<T> : Provider, IDoCareQueryable<T>
    {
       

        private readonly IWhereCommand<T> whereCommand;

        private readonly StringBuilder _selectField = new StringBuilder();

        
        private string prefix = "";

        private readonly  StringBuilder _sortSql = new StringBuilder();
       

        public Queryable(IDbConnection connection)  : base(connection)
        {
            whereCommand = new WhereCommand<T>(SqlParameter);
        }

        public IDoCareQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            whereCommand.Where(predicate);

            return this;

        }

        public IDoCareQueryable<T> Where(string whereExpression)
        {
            whereCommand.Where(whereExpression);

            return this;
        }

        public IDoCareQueryable<T> Where<TEntity>(string whereExpression, Expression<Func<TEntity>> predicate)
        {
            whereCommand.Where(whereExpression, predicate);

            return this;
          
        }

        public IDoCareQueryable<T> OrderBy<TResult>(Expression<Func<T, TResult>> predicate)
        {
            var provider = new SelectProvider();
            provider.Visit(predicate);

            provider.SelectFields.ForEach(t =>
            {
                _sortSql.Append($"{t.ColumnName},");

                prefix = t.Prefix;
            });


            return this;
        }

        public IDoCareQueryable<T> OrderByDesc<TResult>(Expression<Func<T, TResult>> predicate)
        {
            var provider = new SelectProvider();
            provider.Visit(predicate);

            provider.SelectFields.ForEach(t =>
            {
                _sortSql.Append($"{t.ColumnName} desc,");

                prefix = t.Prefix;
            });

            return this;
        }

        public IReaderableCommand<TResult> Select<TResult>(Expression<Func<T,TResult>> predicate)
        {
            var provider = new SelectProvider();
            provider.Visit(predicate);

            provider.SelectFields.ForEach(t =>
            {
                _selectField.Append($"{t.Prefix}.{t.ColumnName} as {t.Parameter},");

                prefix = t.Prefix;
            });
            _selectField.Remove(_selectField.Length - 1, 1);
            return DatabaseFactory.CreateReaderableCommand<TResult>(Connection, Build(), SqlParameter, Aop);
        }


        private StringBuilder Build()
        {
            var sql = new StringBuilder();

            var type = typeof(T);
            var (tableName, properties) = ProviderHelper.GetMetas(type);

            var selectSql = new StringBuilder();
            if (_selectField.Length > 0)
            {
                selectSql.Append( _selectField);
            }
            else
            {
                foreach (var property in properties)
                {
                    selectSql.Append($"{property.ColumnName} as {property.Parameter},");
                }

                selectSql.Remove(selectSql.Length - 1, 1);
            }


            sql.Append($"select {selectSql} from {tableName} ");

            
            sql.Append(whereCommand.Build().Replace(DatabaseFactory.ParamterSplit, DbPrefix));

            if (_sortSql.Length > 0)
            {
                sql.Append(" order by ");
                sql.Append(_sortSql);
                sql.Remove(sql.Length - 1, 1);
            }

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
}