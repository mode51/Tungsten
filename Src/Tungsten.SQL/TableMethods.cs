//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Data.SqlClient;

//namespace lib.Data
//{
//    public partial class TableMethods
//    {
//        private static lib.Configuration.ConfigurationFile _configuration = null;
//        public static string GetConnectionString(string appSettingNamedConfiguration)
//        {
//            if (_configuration == null)
//                _configuration = lib.NamedConfiguration.LoadByAppSetting(appSettingNamedConfiguration);
//            return _configuration?["Data"].Values["ConnectionString"].AsString;
//        }
//        private static string GetConnectionString()
//        {
//            return lib.Configuration.Application.Values["ConnectionString"].AsString;
//        }
//        public static IDbConnection CreateConnection(string connectionString, bool open = false)
//        {
//            var connection = new SqlConnection(connectionString);
//            if (open)
//                connection.Open();
//            return connection;
//        }
//    }
//}
