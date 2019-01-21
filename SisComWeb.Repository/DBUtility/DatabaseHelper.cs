using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisComWeb.Repository
{
    public class DatabaseHelper
    {
        public static string DbProvider()
        {
            return ConfigurationManager.ConnectionStrings["CnPasajes"].ProviderName;
        }

        public static string DbConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["CnPasajes"].ConnectionString;
        }

        public static IDatabase GetDatabase()
        {
            IDatabase db = new DatabaseSql();
            return db;
        }
    }
}
