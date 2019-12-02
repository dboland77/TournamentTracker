using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class MatchupEntryModel
    {
        // Adding three slashes creates an XML comment 
        // this will auto-format for you on a new line
        // It will also display the comment when someone is using this class
        /// <summary>
        /// Represents one team in the matchup
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Represents the score for this particular team
        /// </summary>
        
         // prop tab tab creates a property
        public double Score { get; set; }

        /// <summary>
        /// Represents the matchup that this team came
        /// from as the winner.
        /// </summary>
        public MatchupModel ParentMatchup { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="InitialScore"></param>
        /// //ctor tab tab creates a constructor
        public MatchupEntryModel()
        {
            // cw tab tab will print the line below
            Console.WriteLine();
        }
       

    }
}
