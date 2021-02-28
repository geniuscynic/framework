using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using DoCare.Extension.DataBase.Interface.Command;
using DoCare.Extension.DataBase.SqlProvider;


namespace DoCare.Extension.DataBase.Imp.Command
{
    internal class WhereCommand<T> : IWhereCommand<T>
    {
        private readonly Dictionary<string, object> _sqlPamater;
        private readonly WhereProvider _whereProvider = new WhereProvider();

        private readonly StringBuilder _whereCause = new StringBuilder();

        private string prefix = "";

        public WhereCommand(Dictionary<string, object> sqlPamater)
        {
            _sqlPamater = sqlPamater;
        }

        public void Where(Expression<Func<T, bool>> predicate)
        {
            _whereProvider.Visit(predicate);

            _whereProvider.whereModel.Sql.Append(" and");

            prefix = _whereProvider.whereModel.Prefix;
        }

        public void Where(string whereExpression)
        {
            _whereCause.Append($" ({whereExpression}) and");
        }

        public void Where<TResult>(string whereExpression, Expression<Func<TResult>> predicate)
        {
            _whereCause.Append($" ({whereExpression}) and");

            var provider = new SelectProvider();
            provider.Visit(predicate);

            //var dic = (IDictionary<string, object>)_dynamicModel;

            var model = predicate.Compile().Invoke();
            var types = model.GetType();


            provider.SelectFields.ForEach(t =>
            {
                var values = types.GetProperty(t.Parameter)?.GetValue(model);

                _sqlPamater[t.Parameter] = values;
            });
        }

        public StringBuilder Build(bool ignorePrefix = true)
        {
            var sql = new StringBuilder();
            sql.Append(" where ");

            sql.Append(_whereCause);

            sql.Append(_whereProvider.whereModel.Sql);
            sql.Remove(sql.Length - 3, 3);

            foreach (var keyValuePair in _whereProvider.whereModel.Parameter)
            {
                _sqlPamater[keyValuePair.Key] = keyValuePair.Value;
            }

            return ignorePrefix ? sql.Replace($"{prefix}.", "") : sql;
        }
    }
}
