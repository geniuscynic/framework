using System;
using System.Linq.Expressions;
using System.Text;
using DoCare.Extension.SqlHelper.Interface.Command;
using DoCare.Extension.SqlHelper.SqlProvider;

namespace DoCare.Extension.SqlHelper.Imp.Command
{
    internal class OrderByCommand<T> : IOrderByCommand<T>
    {
        private readonly StringBuilder _sortSql = new StringBuilder();
        private string prefix = "";
        public void OrderBy<TResult>(Expression<Func<T, TResult>> predicate)
        {
            var provider = new SelectProvider();
            provider.Visit(predicate);

            provider.SelectFields.ForEach(t =>
            {
                _sortSql.Append($"{t.Prefix}.{t.ColumnName},");

                prefix = t.Prefix;

            });
        }

        public void OrderByDesc<TResult>(Expression<Func<T, TResult>> predicate)
        {
            var provider = new SelectProvider();
            provider.Visit(predicate);

            provider.SelectFields.ForEach(t =>
            {
                _sortSql.Append($"{t.Prefix}.{t.ColumnName} desc,");

                prefix = t.Prefix;

            });
        }

        public StringBuilder Build(bool ignorePrefix = true)
        {
            if (_sortSql.Length <= 0) return _sortSql;


            var sql = new StringBuilder();
            sql.Append(" order by ");
            sql.Append(_sortSql);
            sql.Remove(sql.Length - 1, 1);

            if (ignorePrefix)
            {
                sql = sql.Replace($"{prefix}.", "");
            }

            return sql;

        }
    }
}
