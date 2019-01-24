using System;
using System.Data;

namespace SisComWeb.Repository
{
    public interface IDatabase : IDisposable
    {
        void Open();

        void Close();

        string CommandText { set; }

        string ProcedureName { set; }

        object GetParameter(int index);

        object GetParameter(string name);

        void AddParameter(string parameterName, DbType parameterType, ParameterDirection parameterDirection, Object parameterValue);

        void AddParameter(string parameterName, Object parameterType, ParameterDirection parameterDirection, Object parameterValue);

        IDataReader GetDataReader(bool close = true);

        int Execute();

        object ExecuteScalar();

        DataTable GetDataTable();

        void ClearParamaters();
    }
}
