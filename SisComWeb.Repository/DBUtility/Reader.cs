using System;
using System.Data;

namespace SisComWeb.Repository
{
    public class Reader
    {
        public static object GetObjectValue(IDataReader dr, string column)
        {
            try
            {
                var obj = dr[column];
                return obj == DBNull.Value ? null : obj;
            }
            catch { return null; }
        }

        public static string GetStringValue(IDataReader dr, string column)
        {
            try
            {
                var obj = GetObjectValue(dr, column);
                return obj == null ? "" : obj.ToString().Trim();
            }
            catch { return ""; }
        }
        public static float GetRealValue(IDataReader dr, string column)
        {
            try
            {
                var obj = GetObjectValue(dr, column);
                if (obj == null) return 0;
                return Convert.ToSingle(obj);
            }
            catch { return 0; }
        }
        public static double GetFloatValue(IDataReader dr, string column)
        {
            try
            {
                var obj = GetObjectValue(dr, column);
                if (obj == null) return 0;
                return Convert.ToDouble(obj);
            }
            catch { return 0; }
        }

        public static decimal GetDecimalValue(IDataReader dr, string column)
        {
            try
            {
                var obj = GetObjectValue(dr, column);
                if (obj == null) return 0;
                return Convert.ToDecimal(obj);
            }
            catch { return 0; }
        }


        public static Int64 GetBigIntValue(IDataReader dr, string column)
        {
            try
            {
                var obj = GetObjectValue(dr, column);
                if (obj == null) return 0;
                return Convert.ToInt64(obj);
            }
            catch { return 0; }
        }

        public static Int32 GetIntValue(IDataReader dr, string column)
        {
            try
            {
                var obj = GetObjectValue(dr, column);
                if (obj == null) return 0;
                return Convert.ToInt32(obj);
            }
            catch { return 0; }
        }


        public static Int16 GetSmallIntValue(IDataReader dr, string column)
        {
            try
            {
                var obj = GetObjectValue(dr, column);
                if (obj == null) return 0;
                return Convert.ToInt16(obj);
            }
            catch { return 0; }
        }

        public static byte GetTinyIntValue(IDataReader dr, string column)
        {
            try
            {
                var obj = GetObjectValue(dr, column);
                if (obj == null) return 0;
                return Convert.ToByte(obj);
            }
            catch { return 0; }
        }

        public static DateTime GetDateTimeValue(IDataReader dr, string column)
        {
            try
            {
                var obj = dr[column];
                return obj == DBNull.Value ? DateTime.Now : (DateTime)obj;
            }
            catch { return new DateTime(); }
        }

        public static bool GetBooleanValue(IDataReader dr, string column)
        {
            try
            {
                var obj = GetObjectValue(dr, column);
                if (obj == null) return false;
                return Convert.ToBoolean(obj);
            }
            catch { throw; }
        }
    }
}
