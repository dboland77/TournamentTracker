using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialise the database connections
            // We need to have a reference to the tracker library in our UI to do the below. 
            TrackerLibrary.GlobalConfig.InitialiseConnections(TrackerLibrary.DatabaseType.TextFile);
            Application.Run(new CreateTournamentForm());
            //Application.Run(new TournamentDashboardForm());
        }
    }
}
