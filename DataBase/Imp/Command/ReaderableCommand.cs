using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using DoCare.Extension.Dao.Common;
using DoCare.Extension.DataBase.Interface.Command;
using Org.BouncyCastle.Crypto.Modes.Gcm;

namespace DoCare.Extension.DataBase.Imp.Command
{
    public class ReaderableCommand<T> : IReaderableCommand<T>
    {
        private readonly IDbConnection _connection;
        private readonly string _sql;
        private readonly Dictionary<string, object> _sqlParameter;
        private readonly Aop _aop;


        public ReaderableCommand(IDbConnection connection, string sql, Dictionary<string, object> sqlParameter, Aop aop)
        {
            _connection = connection;
            _sql = sql;
            _sqlParameter = sqlParameter;
            _aop = aop;
        }

        private async Task<IEnumerable<T>> EnumerableDelegate(Func<Task<IEnumerable<T>>> func)
        {
            try
            {
                _aop?.OnExecuting?.Invoke(_sql, _sqlParameter);

                var result = await func();

                _aop?.OnExecuted?.Invoke(_sql, _sqlParameter);

                return result;
            }
            catch
            {
                _aop?.OnError?.Invoke(_sql, _sqlParameter);
                throw;
            }
        }

        private async Task<T> SingleDelegate(Func<Task<T>> func)
        {
            try
            {
                _aop?.OnExecuting?.Invoke(_sql, _sqlParameter);

                var result = await func();

                _aop?.OnExecuted?.Invoke(_sql, _sqlParameter);

                return result;
            }
            catch
            {
                _aop?.OnError?.Invoke(_sql, _sqlParameter);
                throw;
            }
        }


        public async Task<IEnumerable<T>> ExecuteQuery()
        {
            
            return await EnumerableDelegate(async () => await _connection.QueryAsync<T>(_sql, _sqlParameter));
          
        }

        public async Task<T> ExecuteFirst()
        {
            return await SingleDelegate(async () => await _connection.QueryFirstAsync<T>(_sql, _sqlParameter));
        }

        public async Task<T> ExecuteFirstOrDefault()
        {
            return await SingleDelegate(async () => await _connection.QueryFirstOrDefaultAsync<T>(_sql, _sqlParameter));
        }

        public async Task<T> ExecuteSingle()
        {
            return await SingleDelegate(async () => await _connection.QuerySingleAsync<T>(_sql, _sqlParameter));
        }

        public async Task<T> ExecuteSingleOrDefault()
        {
            return await SingleDelegate(async () => await _connection.QuerySingleOrDefaultAsync<T>(_sql, _sqlParameter));
        }
    }
}
