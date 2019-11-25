using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;
using TrackerLibrary.DataAccess.TextHelpers;
using System.Linq;

namespace TrackerLibrary.DataAccess
{
    public class TextConnector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";

        // TODO - Wire up the CreatePrize for text files
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // here we can use our extension methods from the TextConnectorHelpers namespace
            // Remember that "this" will be passed automatically as the string invoking the method

            // Load the text file
            // Convert the text to List<Prize Model>
           
            List<PrizeModel> prizes = PrizesFile.FullFilePath().LoadFile().ConvertToPrizeModels();

            // Find the max ID
            int currentId = 1;

            if (prizes.Count > 0 )
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            
            model.Id = currentId;

            // Add the new record with the new ID (max + 1)
            prizes.Add(model);

            // Convert the prizes to a list<string>
            // Save the list <string> to the text file
            prizes.SaveToPrizeFile(PrizesFile);

            return (model);
        }
    }
}
