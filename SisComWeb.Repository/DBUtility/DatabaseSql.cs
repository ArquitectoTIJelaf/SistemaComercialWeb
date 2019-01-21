using SisComWeb.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Repository
{
    public class DatabaseSql : IDatabase
    {
        private DbConnection connection;
        private DbCommand command;
        private DbProviderFactory factory;
        private DbTransaction transaction;
        private int commandtimeout;
        public DatabaseSql()
        {
            factory = DbProviderFactories.GetFactory(DatabaseHelper.DbProvider());
        }

        public void Open()
        {
            connection = factory.CreateConnection();
            connection.ConnectionString = DatabaseHelper.DbConnectionString();
            try
            {
                if (connection != null && connection.State != ConnectionState.Open)
                    connection.Open();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(DatabaseSql)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                throw;
            }
        }

        public void OpenTransaction()
        {
            connection = factory.CreateConnection();
            connection.ConnectionString = DatabaseHelper.DbConnectionString();
            try
            {
                if (connection != null && connection.State != ConnectionState.Open)
                {
                    connection.Open();
                    if (transaction != null)
                        transaction = connection.BeginTransaction();
                }

            }
            catch (Exception ex)
            {
                Log.Instance(typeof(DatabaseSql)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                throw;
            }
        }

        public void Close()
        {
            try
            {
                if (connection != null && connection.State != ConnectionState.Closed)
                    connection.Close();
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(DatabaseSql)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                throw;
            }
        }

        public string CommandText
        {
            set
            {
                if (command == null)
                {
                    command = factory.CreateCommand();

                }
                if (connection == null)
                {
                    this.Open();
                }
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = value;
            }
        }

        public string ProcedureName
        {
            set
            {
                if (command == null)
                {
                    command = factory.CreateCommand();
                }
                if (connection == null)
                {
                    this.Open();
                }
                command.Connection = connection;
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = value;

                if (Int32.TryParse(ConfigurationManager.AppSettings["SetCommandTimeoutInSeconds"].ToString(), out commandtimeout))
                {
                    command.CommandTimeout = commandtimeout;
                }

            }
        }

        public object GetParameter(int index)
        {
            if (command != null)
            {
                try { return command.Parameters[index].Value; }
                catch { return null; }
            }
            return null;
        }

        public object GetParameter(string name)
        {
            if (command != null)
            {
                try { return command.Parameters[name].Value; }
                catch { return null; }
            }
            return null;
        }

        public void AddParameter(string parameterName, DbType parameterType, ParameterDirection parameterDirection, object parameterValue)
        {
            if (command != null)
            {
                DbParameter parameter = factory.CreateParameter();
                parameter.ParameterName = parameterName;
                parameter.DbType = parameterType;
                parameter.Direction = parameterDirection;
                parameter.Value = parameterValue ?? DBNull.Value;
                parameter.SourceColumnNullMapping = true;
                if (parameterType == DbType.String && (parameterDirection == ParameterDirection.Output || parameterDirection == ParameterDirection.InputOutput)) parameter.Size = -1;
                command.Parameters.Add(parameter);
            }
        }

        public void AddParameter(string parameterName, object parameterType, ParameterDirection parameterDirection, object parameterValue)
        {
            if (command != null)
            {
                DbParameter parameter = factory.CreateParameter();
                parameter.ParameterName = parameterName;
                parameter.DbType = (DbType)parameterType;
                parameter.Direction = parameterDirection;
                parameter.Value = parameterValue ?? DBNull.Value;
                parameter.SourceColumnNullMapping = true;
                command.Parameters.Add(parameter);
            }
        }

        public void AddTransaction()
        {
            try
            {
                if (transaction != null)
                {
                    if (command != null)
                    {
                        command.Transaction = transaction;
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Instance(typeof(DatabaseSql)).Error(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                throw;
            }
        }

        public void CommitTransaction()
        {
            this.transaction.Commit();
        }

        public void RollBackTransaction()
        {
            this.transaction.Rollback();
        }

        public IDataReader GetDataReader(bool close = true)
        {

            try
            {
                if (close)
                {
                    return command.ExecuteReader(CommandBehavior.SequentialAccess | CommandBehavior.CloseConnection);
                }
                else
                {
                    return command.ExecuteReader(CommandBehavior.SequentialAccess);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public int Execute()
        {
            return command.ExecuteNonQuery();
        }

        public object ExecuteScalar()
        {
            return command.ExecuteScalar();
        }

        public DataTable GetDataTable()
        {
            DataTable dt = new DataTable();

            using (DbDataAdapter da = factory.CreateDataAdapter())
            {
                da.SelectCommand = command;
                da.Fill(dt);
            }

            return dt;
        }

        public void ClearParamaters()
        {
            if (command != null)
            {
                command.Parameters.Clear();
            }
        }

        ~DatabaseSql()
        {
            Dispose(false);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
                if (connection != null)
                {
                    connection.Dispose();
                    connection = null;
                }
                if (command != null)
                {
                    command.Dispose();
                    command = null;
                }
                factory = null;
            }
        }

        #endregion
    }
}
