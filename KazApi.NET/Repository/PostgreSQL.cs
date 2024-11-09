﻿using Dapper;
using Npgsql;
using System.Data;

namespace KazApi.Repository
{
    public class PostgreSQL : IDisposable, IDatabase
    {
        private static readonly string CONNECTION_STRING = "Server=localhost;Port=5432;User Id=postgres;Password=kaz_5050;Database=kaz_app";
        private IDbConnection Connection { get; set; }
        private IDbTransaction Transaction { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public PostgreSQL(IConfiguration configuration)
        {
            Connection = new NpgsqlConnection(configuration["ConnectionStrings:DefaultConnection"]);
        }
        public PostgreSQL()
        {
            Connection = new NpgsqlConnection(CONNECTION_STRING);
        }

        /// <summary>
        /// 接続状態
        /// </summary>
        public bool IsConnected()
        {
            return Connection.State == ConnectionState.Open;
        }

        /// <summary>
        /// 後処理
        /// </summary>
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// コネクションオープン
        /// </summary>
        public void ConnectionOpen()
        {
            if (!IsConnected())
            {
                Connection.Open();
            }
        }

        /// <summary>
        /// コネクションクローズ
        /// </summary>
        public void ConnectionClose()
        {
            if (IsConnected())
            {
                Connection.Close();
            }
        }

        /// <summary>
        /// トランザクション開始
        /// </summary>
        public void BeginTransaction()
        {
            Transaction = Connection.BeginTransaction(IsolationLevel.Serializable);
        }

        /// <summary>
        /// コミット
        /// </summary>
        public void CommitTransaction()
        {
            if (Transaction != null && Transaction.Connection != null)
            {
                Transaction.Commit();
            }
        }

        /// <summary>
        /// ロールバック
        /// </summary>
        public void RollBackTransaction()
        {
            if (Transaction != null && Transaction.Connection != null)
            {
                Transaction.Rollback();
            }
        }

        public IEnumerable<T> Select<T>(string query, object parameters)
        {
            return Connection.Query<T>(query, parameters).ToList();
        }

        public IEnumerable<T> Select<T>(string query)
        {
            return Connection.Query<T>(query).ToList();
        }

        public void Execute(string query)
        {
            Connection.Execute(query);
        }

        public void Execute(string query, object parameters)
        {
            Connection.Execute(query, parameters);
        }
    }
}
