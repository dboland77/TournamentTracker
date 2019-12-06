using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using TrackerLibrary.DataAccess;

namespace TrackerLibrary
{
    public static class GlobalConfig
    { 
    public const string PrizesFile = "PrizeModels.csv";
    public const string PeopleFile = "PersonModels.csv";
    public const string TeamFile = "TeamModels.csv";
    public const string TournamentFile = "TournamentModels.csv";
    public const string MatchupFile = "MatchupModels.csv";
    public const string MatchupEntryFile = "MatchupEntryModels.csv";

        public static IDataConnection Connection { get; private set; } 

        public static void InitialiseConnections(DatabaseType db)
        {

            if (db == DatabaseType.Sql)
            {
                // Set up the SQL Connector properly
                SQLConnector sql = new SQLConnector();
                Connection = sql;
            }

            else if(db == DatabaseType.TextFile)
            {
               
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
