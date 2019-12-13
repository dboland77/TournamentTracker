using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        // Order our list randomly
        // check if it is big enough - if not add some byes (need 2^n entries)
        // Create our first round of matchups
        // Create every round after that - 8 matchups, 4 matchups, 2 matchups, 1 matchup

        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomisedTeams = RandomiseTeamOrder(model.EnteredTeams);
            int rounds = FindNumberofRounds(randomisedTeams.Count);
            int byes = NumberOfByes(rounds, randomisedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomisedTeams));
            CreateOtherRounds(model, rounds);  
        }

        public static void UpdateTournamentResults(TournamentModel model)
        {
            List<MatchupModel> toScore = new List<MatchupModel>();

            foreach (List<MatchupModel> round in model.Rounds)
            {
                foreach (MatchupModel rm in round)
                {
                    if (rm.Winner != null && (rm.Entries.Any(x => x.Score != 0) || rm.Entries.Count == 1))
                    {
                        toScore.Add(rm);
                    }
                }
            }

            MarkWinnersInMatchups(toScore);

            AdvanceWinners(toScore, model);

            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));
        }

        private static void AdvanceWinners(List<MatchupModel> models, TournamentModel tournament)
        {
            foreach (MatchupModel m in models)
            {
                foreach (List<MatchupModel> round in tournament.Rounds)
                {
                    foreach (MatchupModel rm in round)
                    {
                        foreach (MatchupEntryModel me in rm.Entries)
                        {
                            if (me.ParentMatchup != null)
                            {
                                if (me.ParentMatchup.Id == m.Id)
                                {
                                    me.TeamCompeting = m.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(rm);
                                }
                            }
                        }

                    }
                }
            }
        }

        private static void MarkWinnersInMatchups(List<MatchupModel> models)
        {
            //greater or lessser
            string greaterWins = ConfigurationManager.AppSettings["greaterWins"];

                foreach (MatchupModel m in models)
                {
                    // Checks for bye week entry
                    if (m.Entries.Count ==1)
                    {
                         m.Winner = m.Entries[0].TeamCompeting;
                         continue;
                    }
                //0 means false or low score wins
                if (greaterWins == "0")
                {
                    if (m.Entries[0].Score < m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[1].Score < m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We do not allow ties in this application.");
                    }
                }
                else
                {
                    // 1 means true or high score wins
                    if (m.Entries[0].Score > m.Entries[1].Score)
                    {
                        m.Winner = m.Entries[0].TeamCompeting;
                    }
                    else if (m.Entries[1].Score > m.Entries[0].Score)
                    {
                        m.Winner = m.Entries[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We do not allow ties in this application.");
                    }
                }

            }
            //if (teamOneScore > teamTwoScore)
            //{
            //    //Team one wins
            //    m.Winner = m.Entries[0].TeamCompeting;
            //}
            //else if (teamTwoScore > teamOneScore)
            //{
            //    m.Winner = m.Entries[1].TeamCompeting;
            //}
            //else
            //{
            //    MessageBox.Show("I do not handle tied games yet.");
            //}
        }
    
        private static void CreateOtherRounds(TournamentModel model, int rounds)
        {
            int round = 2;
            List<MatchupModel> previousRound = model.Rounds[0];
            List<MatchupModel> currRound = new List<MatchupModel>();
            MatchupModel currMatchup = new MatchupModel();
            while (round <= rounds)
            {
                foreach (MatchupModel matchup in previousRound)
                {
                    currMatchup.Entries.Add(new MatchupEntryModel { ParentMatchup = matchup });
                    if (currMatchup.Entries.Count >1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new MatchupModel();
                    }
                }

                model.Rounds.Add(currRound);
                previousRound = currRound;
                currRound = new List<MatchupModel>();
                round += 1;

            }
        }

        private static List<MatchupModel> CreateFirstRound(int byes, List<TeamModel> teams)
        {
            List<MatchupModel> output = new List<MatchupModel>();
            MatchupModel curr = new MatchupModel();


            foreach (TeamModel team in teams)
            {
                curr.Entries.Add(new MatchupEntryModel{ TeamCompeting = team });
                if (byes > 0 || curr.Entries.Count > 1)
                {
                    curr.MatchupRound = 1;
                    output.Add(curr);
                    curr = new MatchupModel();

                    if (byes > 0)
                    {
                        byes -= 1;
                    }
                }
            }

            return output;
        }

        private static int NumberOfByes(int rounds, int numberofTeams)
        {
            // Calculates 2^rounds not using Math.Pow because it uses doubles
            // Math.Pow(2, rounds);

            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberofTeams;
            return output;
        }

        private static int FindNumberofRounds(int teamCount)
        {
            int output = 1;
            int val = 2;

            while (val < teamCount)
            {
                output += 1;
                val *= 2;

            }

            return output;
        }

        private static List<TeamModel> RandomiseTeamOrder(List<TeamModel> teams)
        {
            // cards.OrderBy( a => Guid.NewGuid()).ToList(); (from stack overflow - the Guid should be unique
            return teams.OrderBy(x => Guid.NewGuid()).ToList();  
        }
    }
}
