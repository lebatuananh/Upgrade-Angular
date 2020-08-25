using System;
using System.Collections.Generic;
using Npgsql;
using System.Data;
using System.ComponentModel;
using System.Data.Common;
using DVG.WIS.Utilities.Databases.Base;
using System.Threading.Tasks;

namespace DVG.WIS.Utilities.Databases
{
    public class ConnectionEntity
    {
        public enum DBPosition
        {
            Manual = -1,

            [Description("ConnectionString")]
            Default = 0,

            [Description("MasterConnection")]
            Master = 1,

            [Description("MasterConnection")]
            Slave = 2
        }
    }

    public class PostgresSQL : IDisposable
    {

        /// <summary>
        /// Postgres is a class that will handle the postgres datbase. 
        /// </summary> 
        private string _connectionString;
        protected NpgsqlConnection _connection;
        protected DbTransaction _transaction;
        private static Dictionary<string, int> _dictConnection;
        protected ConnectionEntity.DBPosition _dbPosition = ConnectionEntity.DBPosition.Default;

        public PostgresSQL(bool isInit = true)
        {
            _connectionString = GetConnectionString(_dbPosition);

            if (null == _dictConnection)
            {
                _dictConnection = new Dictionary<string, int>();
            }

            if (isInit)
            {
                InitConnection();
            }
        }

        public PostgresSQL(string connectionString, bool isInit = true)
        {
            _connectionString = connectionString;

            if (isInit)
            {
                InitConnection();
            }
        }

        public PostgresSQL(int dbPosition, bool isInit = true)
        {
            _dbPosition = (ConnectionEntity.DBPosition)dbPosition;

            _connectionString = GetConnectionString(_dbPosition);

            if (isInit)
            {
                InitConnection();
            }
        }

        public PostgresSQL(ConnectionEntity.DBPosition dbPosition, bool isInit = true)
        {
            _dbPosition = dbPosition;

            _connectionString = GetConnectionString(_dbPosition);

            if (isInit)
            {
                InitConnection();
            }
        }

        protected void InitConnection()
        {
            _connection = CreateConnection();
            _connection.Open();
        }

        protected string GetConnectionString(ConnectionEntity.DBPosition dbPosition, string defaultValue = "ConnectionString")
        {
            string connectionName = string.Empty;
            if (dbPosition == ConnectionEntity.DBPosition.Manual)
            {
                if (!string.IsNullOrEmpty(defaultValue)) connectionName = defaultValue;
            }
            else
            {
                connectionName = Utils.GetEnumDescription(dbPosition);
            }

            if (string.IsNullOrEmpty(connectionName)) connectionName = "ConnectionString";

            return AppSettings.Instance.GetConnection(connectionName);
        }

        /// <summary>
        /// GetConnectionString - will get the connection string for use. 
        /// </summary>
        /// <returns>The connection string</returns>
        public string GetConnectionString()
        {
            if (string.IsNullOrEmpty(_connectionString))
                _connectionString = GetConnectionString(_dbPosition);
            return _connectionString;
        }

        public NpgsqlConnection CreateConnection()
        {
            var conn = new NpgsqlConnection(GetConnectionString());
            return conn;
        }
        /// <summary>
        /// Returns a SQL statement parameter name that is specific for the data provider.
        /// For example it returns ? for OleDb provider, or @paramName for MS SQL provider.
        /// </summary>
        /// <param name="paramName">The data provider neutral SQL parameter name.</param>
        /// <returns>The SQL statement parameter name.</returns>
        protected internal string CreateSqlParameterName(string paramName)
        {
            return "@" + paramName;
        }

        /// <summary>
        /// Creates a .Net data provider specific name that is used by the 
        /// <see cref="AddParameter"/> method.
        /// </summary>
        /// <param name="baseParamName">The base name of the parameter.</param>
        /// <returns>The full data provider specific parameter name.</returns>
        protected string CreateCollectionParameterName(string baseParamName)
        {
            return "@" + baseParamName;
        }

        /// <summary>
        /// Creates <see cref="System.Data.IDataReader"/> for the specified DB command.
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> object.</param>
        /// <returns>A reference to the <see cref="System.Data.IDataReader"/> object.</returns>
        public virtual IDataReader ExecuteReader(NpgsqlCommand command)
        {
            return command.ExecuteReader();
        }

        ///// <summary>
        ///// Creates <see cref="System.Data.IDataReader"/> for the specified DB command.
        ///// </summary>
        ///// <param name="commandType"></param>
        ///// <param name="commandText"></param>
        ///// <param name="parameters"></param>
        ///// <returns>A reference to the <see cref="System.Data.IDataReader"/> object.</returns>
        //protected internal virtual IDataReader ExecuteReader(CommandType commandType, string commandText, params SqlParameter[] parameters)
        //{
        //    var command = CreateCommand(commandType, commandText, parameters);
        //    return command.ExecuteReader();
        //}

        /// <summary>
        /// Creates <see cref="System.Data.IDataReader"/> for the specified DB command.
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> object.</param>
        /// <returns>A reference to the <see cref="System.Data.IDataReader"/> object.</returns>
        public virtual int ExecuteNonQuery(NpgsqlCommand command)
        {
            return command.ExecuteNonQuery();
        }

        ///// <summary>
        ///// Creates <see cref="System.Data.IDataReader"/> for the specified DB command.
        ///// </summary>
        ///// <param name="commandType"></param>
        ///// <param name="commandText"></param>
        ///// <param name="parameters"></param>
        ///// <returns>A reference to the <see cref="System.Data.IDataReader"/> object.</returns>
        //protected internal virtual int ExecuteNonQuery(CommandType commandType, string commandText, params SqlParameter[] parameters)
        //{
        //    var command = CreateCommand(commandType, commandText, parameters);
        //    return command.ExecuteNonQuery();
        //}

        /// <summary>
        /// Fill to DataTable
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public DataTable CreateDataTable(NpgsqlCommand command)
        {
            DataTable dataTable = new DataTable();
            new NpgsqlDataAdapter(command).Fill(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Map records from the DataReader
        /// </summary>
        /// <param name="reader">The <see cref="System.Data.IDataReader"/> object.</param>
        /// <returns>List entity of records.</returns>
        protected List<T> MapRecords<T>(IDataReader reader)
        {
            List<T> recordList = new List<T>();
            T entity = default(T);
            while (reader.Read())
            {
                recordList.Add(EntityBase.SetObjectValue(reader, entity));
            }
            return recordList;
        }

        /// <summary>
        /// Map records from the DataReader
        /// </summary>
        /// <param name="command">The <see cref="System.Data.IDbCommand"/> command.</param>
        /// <returns>List entity of records.</returns>
        public List<T> GetList<T>(NpgsqlCommand command)
        {
            List<T> returnValue;
            using (IDataReader reader = this.ExecuteReader(command))
            {
                returnValue = MapRecords<T>(reader);
            }
            return returnValue;
        }

        /// <summary>
        /// Map records from the DataReader
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<List<T>> GetListAsync<T>(NpgsqlCommand command)
        {
            var lstObject = new List<T>();
            T entity = default(T);
            using (var reader = await command.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    lstObject.Add(EntityBase.SetObjectValue(reader, entity));
                }
            }
            return lstObject;
        }

        public async Task<T> GetObjectAsync<T>(NpgsqlCommand command)
        {
            var returnValue = default(T);
            using (var reader = await command.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    returnValue = EntityBase.SetObjectValue(reader, returnValue);
                }
            }
            return returnValue;
        }

        public object GetFirtDataRecord(NpgsqlCommand command)
        {
            object returnValue = null;
            using (IDataReader reader = this.ExecuteReader(command))
            {
                if (reader.Read())
                {
                    returnValue = reader[0];
                }
            }
            return returnValue;
        }

        /// <summary>
        /// Adds a new parameter to the specified command. It is not recommended that 
        /// you use this method directly from your custom code. Instead use the 
        /// <c>AddParameter</c> method of the CmsMainDbBase classes.
        /// </summary>
        /// <param name="cmd">The <see cref="System.Data.IDbCommand"/> object to add the parameter to.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <returns>A reference to the added parameter.</returns>
        public IDbDataParameter AddParameter(NpgsqlCommand cmd, string paramName, object value)
        {
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = CreateCollectionParameterName(paramName);
            if (value is DateTime)
            {
                parameter.Value = (DateTime.MinValue == DateTime.Parse(value.ToString()) ? DBNull.Value : value);
            }
            else
            {
                parameter.Value = (value ?? DBNull.Value);
            }
            cmd.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Adds a new parameter to the specified command. It is not recommended that 
        /// you use this method directly from your custom code. Instead use the 
        /// <c>AddParameter</c> method of the CmsMainDbBase classes.
        /// </summary>
        /// <param name="cmd">The <see cref="System.Data.IDbCommand"/> object to add the parameter to.</param>
        /// <param name="paramName">The name of the parameter.</param>
        /// <param name="value">The value of the parameter.</param>
        /// <param name="paraDirection">The direction of the parameter.</param>
        /// <returns>A reference to the added parameter.</returns>
        public IDbDataParameter AddParameter(IDbCommand cmd, string paramName, object value, ParameterDirection paraDirection)
        {
            IDbDataParameter parameter = cmd.CreateParameter();
            parameter.ParameterName = CreateCollectionParameterName(paramName);
            if (value is DateTime)
            {
                parameter.Value = (DateTime.MinValue == DateTime.Parse(value.ToString()) ? DBNull.Value : value);
            }
            else
            {
                parameter.Value = (value ?? DBNull.Value);
            }
            parameter.Direction = paraDirection;
            cmd.Parameters.Add(parameter);
            return parameter;
        }

        /// <summary>
        /// Begins a new database transaction.
        /// </summary>
        /// <seealso cref="CommitTransaction"/>
        /// <seealso cref="RollbackTransaction"/>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction()
        {
            CheckTransactionState(false);
            _transaction = _connection.BeginTransaction();
            return _transaction;
        }

        /// <summary>
        /// Begins a new database transaction with the specified 
        /// transaction isolation level.
        /// <seealso cref="CommitTransaction"/>
        /// <seealso cref="RollbackTransaction"/>
        /// </summary>
        /// <param name="isolationLevel">The transaction isolation level.</param>
        /// <returns>An object representing the new transaction.</returns>
        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            CheckTransactionState(false);
            _transaction = _connection.BeginTransaction(isolationLevel);
            return _transaction;
        }

        /// <summary>
        /// Commits the current database transaction.
        /// <seealso cref="BeginTransaction"/>
        /// <seealso cref="RollbackTransaction"/>
        /// </summary>
        public void CommitTransaction()
        {
            CheckTransactionState(true);
            _transaction.Commit();
            _transaction = null;
        }

        /// <summary>
        /// Rolls back the current transaction from a pending state.
        /// <seealso cref="BeginTransaction"/>
        /// <seealso cref="CommitTransaction"/>
        /// </summary>
        public void RollbackTransaction()
        {
            CheckTransactionState(true);
            _transaction.Rollback();
            _transaction = null;
        }

        // Checks the state of the current transaction
        private void CheckTransactionState(bool mustBeOpen)
        {
            if (mustBeOpen)
            {
                if (null == _transaction)
                    throw new InvalidOperationException("Transaction is not open.");
            }
            else
            {
                if (null != _transaction)
                    throw new InvalidOperationException("Transaction is already open.");
            }
        }

        /// <summary>
        /// Creates and returns a new <see cref="System.Data.IDbCommand"/> object.
        /// </summary>
        /// <param name="sqlText">The text of the query.</param>
        /// <returns>An <see cref="System.Data.IDbCommand"/> object.</returns>
        public NpgsqlCommand CreateCommand(string sqlText)
        {
            return CreateCommand(sqlText, false);
        }

        /// <summary>
        /// Creates and returns a new <see cref="System.Data.IDbCommand"/> object.
        /// </summary>
        /// <param name="sqlText">The text of the query.</param>
        /// <param name="procedure">Specifies whether the sqlText parameter is 
        /// the name of a stored procedure.</param>
        /// <returns>An <see cref="System.Data.IDbCommand"/> object.</returns>
        public NpgsqlCommand CreateCommand(string sqlText, bool procedure)
        {
            NpgsqlCommand cmd = new NpgsqlCommand(sqlText, _connection);
            cmd.CommandText = sqlText;
            if (procedure)
                cmd.CommandType = CommandType.StoredProcedure;
            return cmd;
        }

        //public object GetParameterValueFromCommand(IDbCommand command, int paramterIndex)
        //{
        //    var parameter = command.Parameters[paramterIndex] as SqlParameter;
        //    return parameter != null ? parameter.Value : null;
        //}

        public virtual void Close()
        {
            if (null != _connection)
                _connection.Close();
        }

        public void Dispose()
        {
            Close();
            if (null != _connection)
                _connection.Dispose();
        }

        /// <summary>
        /// CheckDBConnectionInfo - will check the db connection information. 
        /// </summary>
        //public override void CheckDBConnectionInfo()
        //{
        //	// check the arguments 
        //	if (base.Server.Length == 0)
        //	{
        //		throw new Exception("Invalid Server Name");
        //	}
        //	// check the arguments 
        //	if (base.DBName.Length == 0)
        //	{
        //		throw new Exception("Invalid Database Name");
        //	}
        //	// check the arguments
        //	if (base.SAUserName.Length == 0)
        //	{
        //		throw new Exception("Invalid SA Name");
        //	}
        //	// check the arguments
        //	if (base.SAPassword.Length == 0)
        //	{
        //		throw new Exception("Invalid SA Password");
        //	}
        //}
        /// <summary>
        /// GetUserPassword - get the user password. 
        /// </summary>
        /// <param name="userName">the user to look up the paassword.</param>
        /// <returns>the password associated witht the user</returns>
        /// 
    }
}