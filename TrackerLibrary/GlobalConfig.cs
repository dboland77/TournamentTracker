using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; } 

        public static void InitialiseConnections(DatabaseType db)
        {

            if (db == DatabaseType.Sql)
            {
                // TODO - Set up the SQL Connector properly
                SQLConnector sql = new SQLConnector();
                Connection = sql;
            }

            else if(db == DatabaseType.TextFile)
            {
                // TODO - Create the Text Connection
                TextConnector text = new TextConnector();
                Connection = text;
            }

        }
        public static string ConnectionString(string name)
        {
            // Get the connection string by looking up the name in App.config
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
