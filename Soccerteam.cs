using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace OSM
{
    /// <summary>
    /// Creates a soccerteam with eleven players
    /// </summary>
    public class Soccerteam
    {
        public Soccerteam()
        {
            players = new List<Player>();
        }
        public void MakeTeam()
        {
            bool error;
            System.Console.WriteLine("Hello, do you want to create a [n]ew team or use an [e]xisting one?");
            string choice = Console.ReadLine();
            do
            {
                error = false;
                if (choice == "n")
                {
                    do
                    {
                        error = false;
                        System.Console.WriteLine("Do you want to create a [d]efault team or do you want to [c]reate all your players");
                        string choice2 = Console.ReadLine();
                        if (choice2 == "d")
                        {
                            CreateDefaultTeam();
                        }
                        else if (choice2 == "c")
                        {
                            InitTeam();
                        }
                        else
                        {
                            error = true;
                        }
                    } while (error);
                }
                else if (choice == "e")
                {
                    LoadTeam();
                }
                else
                {
                    error = true;
                }
            } while (error);
        }
        private void CreateDefaultTeam()
        {
            for (int i = 1; i < 12; i++)
            {
                AddDefaultPlayer(i);
            }
        }
        private void AddDefaultPlayer(int i)
        {
            AddPlayer($"Player {i}", 10);
        }

        private void InitTeam()
        {
            CompTeam();
            NameTeam();
            for (int i = 1; i < 12; i++)
            {
                AddPlayer();
            }
        }

        public void AddPlayer()
        {
            bool error = false;
            string PName = "";
            int PSkill = 0;
            do
            {
                error = false;
                System.Console.WriteLine("Please enter the player's name");
                PName = Console.ReadLine();
                if (!PName.All(Char.IsLetter))
                {
                    error = true;
                    System.Console.WriteLine("Invalid input");
                }
            } while (error);
            do
            {
                error = false;
                System.Console.WriteLine("Please enter the player's skill [1-100]");
                string PSkillString = Console.ReadLine();
                if (!PSkillString.All(Char.IsDigit))
                {
                    System.Console.WriteLine("Please only enter digits");
                    error = true;
                }
                else
                {
                    PSkill = int.Parse(PSkillString);
                    if (PSkill > 100 || PSkill < 1)
                    {
                        error = true;
                        System.Console.WriteLine("Input out of bounds");
                    }
                }
            } while (error);
            AddPlayer(PName, PSkill);
        }
        private void AddPlayer(string PName, int PSkill)
        // This has to be totaly different, pick player you want to replace or add
        {
            Player NewPlayer = new Player(PName, PSkill);
            players.Add(NewPlayer);
            CalcTeamStats();
        }

        private void DelPlayer(int delPlayer)
        {
            if(delPlayer < 11)
            {
                ChangePlayer(11, delPlayer);
                players.Remove(players[11]);
            }
            else
            {
                players.Remove(players[delPlayer - 1]);
            }
        }
        public void ChangePlayer()
        {
            ShowTeam();
            System.Console.WriteLine("Please enter the number of the player you would like to change");
            int playerIn = int.Parse(Console.ReadLine());
            System.Console.WriteLine("New enter the number of the player you want to change him with");
            int playerOut = int.Parse(Console.ReadLine());
            ChangePlayer(playerIn, playerOut);
            CalcTeamStats();
        }

        private void ChangePlayer(int playerIn, int playerOut)
        {
            Player PIn = players[playerIn - 1];
            Player POut = players[playerOut - 1];
            players[playerIn - 1] = POut;
            players[playerOut - 1] = PIn;
        }

        private void BuyPlayer(string oppTeam, string compName)
        {
            error = false;
            Soccerteam OppTeam = new Soccerteam();
            OppTeam.LoadTeam(compName, oppTeam);
            if(OppTeam.players.Count < 12)
            {
                error = true;
                System.Console.WriteLine($"{OppTeam.TeamName} can't sell a player, the team has only 11 players left.");
            }
            else
            {
                OppTeam.ShowTeam();
                System.Console.WriteLine("Please enter the number of the player you want to buy");
                int boughtPlayer = int.Parse(Console.ReadLine());
                AddPlayer(OppTeam.players[boughtPlayer - 1].PlayerName, OppTeam.players[boughtPlayer - 1].PlayerSkill);
                OppTeam.DelPlayer(boughtPlayer);
                OppTeam.SaveTeam(compName, oppTeam);
            }
        }

        private void SellPlayer(string oppTeam, string compName)
        {
            error = false;
            if (players.Count < 12)
            {
                error = true;
                System.Console.WriteLine($"{TeamName} can't sell a player, the team has only 11 players left.");
            }
            else
            {
                ShowTeam();
                System.Console.WriteLine("Please enter the number of the player you want to sell");
                int soldPlayer = int.Parse(Console.ReadLine()); // Make this with some more limitations
                Soccerteam OppTeam = new Soccerteam();
                OppTeam.LoadTeam(compName, oppTeam);
                OppTeam.AddPlayer(players[soldPlayer - 1].PlayerName, players[soldPlayer - 1].PlayerSkill);
                DelPlayer(soldPlayer);
                OppTeam.SaveTeam(compName, oppTeam);
            }
        }

        public void TransferPlayer()
        {
            System.Console.WriteLine("With which team do you want to trade?");
            System.Console.WriteLine("First enter the competition of the team");
            string compName = GetCompName();
            System.Console.WriteLine("Now enter the team's name from the list"); // Make this with more limitations, has to be on the list, but can't be same team
            ShowTeamsInCompetition(compName);
            string oppTeam = Console.ReadLine();
            System.Console.WriteLine("Do you want to [b]uy or [s]ell a player?");
            do
            {
                error = false;
                string transfer = Console.ReadLine();
                switch (transfer)
                {
                    case "b":
                        {
                            BuyPlayer(oppTeam, compName);
                            break;
                        }
                    case "s":
                        {
                            SellPlayer(oppTeam, compName);
                            break;
                        }
                    default:
                        {
                            System.Console.WriteLine("Invalid input");
                            error = true;
                            break;
                        }
                }
            } while (error);
            SaveTeam(competitionName, TeamName);
            ShowTeam();
        }

        ///<summary>
        ///Shows you your full team with their skills
        ///</summary>
        public void ShowTeam()
        {
            int i = 0;
            foreach (Player player in players)
            {
                i++;
                System.Console.WriteLine($"{i} Name:{player.PlayerName} Skill:{player.PlayerSkill}");
            }
        }
        private void CompTeam()
        {
            System.Console.WriteLine("Please enter the name of the competition you want to play in");
            competitionName = GetCompName();
        }

        private string GetCompName()
        {
            System.Console.WriteLine("You can choose [E]redivisie, [Prim]era Division, [Prem]ier League or [B]undes Liga");
            bool error;
            string cName = new string("");
            string compName = new string("");
            do
            {
                cName = Console.ReadLine();
                error = false;
                switch (cName)
                {
                    case "E":
                        compName = "Eredivisie";
                        break;
                    case "Prim":
                        compName = "Primera Division";
                        break;
                    case "Prem":
                        compName = "Premier League";
                        break;
                    case "B":
                        compName = "Bundes Liga";
                        break;
                    default:
                        System.Console.WriteLine("Invalid Input");
                        error = true;
                        break;
                }
            } while (error);
            return compName;
        }
        private void NameTeam()
        {
            do
            {
                error = false;
                string TName = new string("");
                System.Console.WriteLine("Please enter your teamname");
                TName = Console.ReadLine();
                if (!TName.All(char.IsLetterOrDigit) || TName.All(char.IsDigit))
                {
                    System.Console.WriteLine("Invalid input");
                    error = true;
                }
                else
                {
                    teamName = TName;
                }
            } while (error);
        }
        private TeamStats CalcTeamStats()
        {
            tStats = new TeamStats();
            if (Players.Count > 10)
            {
                tStats.Offense = players.GetRange(8, 3).Select(i => i.PlayerSkill).Sum() + 0.55 * Players.GetRange(5, 3).Select(i => i.PlayerSkill).Sum() + 0.1 * Players.GetRange(1, 4).Select(i => i.PlayerSkill).Sum();
                tStats.Defense = players.GetRange(1, 4).Select(i => i.PlayerSkill).Sum() + 0.55 * Players.GetRange(5, 3).Select(i => i.PlayerSkill).Sum() + 0.1 * Players.GetRange(8, 3).Select(i => i.PlayerSkill).Sum();
                tStats.Keeper = players[0].PlayerSkill;
                stats = true;
            }
            return TStats;
        }

        public void SaveTeam(string cName, string tName)
        {
            File.Delete($"{cName}/{tName}.txt");
            using (StreamWriter writer = File.AppendText($"{cName}/{tName}.txt"))
            {
                foreach (Player player in players)
                {
                    writer.WriteLine(player.PlayerName);
                    writer.WriteLine(player.PlayerSkill);
                }
            }
        }

        private void ShowTeamsInCompetition(string compName)
        {
            string[] Teams = { "" };
            Teams = Directory.GetFiles(compName);
            foreach (string teampath in Teams)
            {
                string team = teampath.Substring(compName.Length + 1, teampath.Length - 5 - compName.Length);
                System.Console.WriteLine($"{team}");
            }
        }
        private void LoadTeam()
        {
            CompTeam();
            ShowTeamsInCompetition(competitionName);
            System.Console.WriteLine("Please enter the name of your team from the available teams");
            do
            {
                error = false;
                teamName = Console.ReadLine();
                LoadTeam(competitionName, teamName);
            } while (error);
        }
        private void LoadTeam(string cName, string tName)
        {
            try
            {
                using (var reader = File.OpenText($"C:/Users/vgoossen/Soccerteam/src/OSM/{cName}/{tName}.txt"))
                {
                    string line;
                    string playerName;
                    int playerSkill;
                    line = reader.ReadLine();
                    do
                    {
                        playerName = line;
                        line = reader.ReadLine();
                        playerSkill = int.Parse(line);
                        AddPlayer(playerName, playerSkill);
                        line = reader.ReadLine();
                    } while (line != null);
                }
            }
            catch (FileNotFoundException)
            {
                System.Console.WriteLine("This team is not available");
                error = true;
            }
        }

        private bool stats = false;
        private TeamStats tStats;
        public TeamStats TStats
        {
            get { return tStats; }
        }
        private List<Player> players;
        public List<Player> Players
        {
            get { return players; }
        }
        public int Score;
        // Make this less accessible
        private string teamName;
        public string TeamName
        {
            set { teamName = TeamName; }
            get { return teamName; }
        }

        private string competitionName;
        public string CompetitionName
        {
            set { competitionName = CompetitionName; }
            get { return competitionName; }
        }
        private bool error;
    }
}