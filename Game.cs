using System;
using System.Threading;

namespace OSM
{
    public class Game
    {
        public Game(Soccerteam Team1, Soccerteam Team2)
        {
            T1 = Team1;
            T2 = Team2;
            Team1.Score = 0;
            Team2.Score = 0;
            System.Console.WriteLine("A match will start");
        }

        public void TheGame()
        {
            (T1CPM, T2CPM) = ChancesPerMinute();
            (T1CR, T2CR) = ConversionRate();
            System.Console.WriteLine($"The match is {T1.TeamName} against {T2.TeamName}");
            System.Console.WriteLine("Enter anything to start the match");
            Console.ReadLine();
            for (int i = 1; i < 91; i++)
            {
                System.Console.WriteLine($"{i}");
                WhichTeamHasAChance();
                Thread.Sleep(500);
                if (i == 45)
                {
                    System.Console.WriteLine("It is half-time");
                    ShowScore();
                    System.Console.WriteLine("Enter anything to continue");
                    Console.ReadLine();
                }
            }
            System.Console.WriteLine($"The game is over, the final score is {T1.Score} against {T2.Score}!");
        }
        public (double T1CPM, double T2CPM) ChancesPerMinute()
        {
            return (T1.TStats.Offense / T2.TStats.Defense * .1, T2.TStats.Offense / T1.TStats.Defense * .1);
        }
        public (double T1CR, double T2CR) ConversionRate()
        {
            // Change this so a single player getting the chance and getting his skill matched up against the goalkeeper's skill
            return (T1.TStats.Offense / T2.TStats.Keeper * .06, T2.TStats.Offense / T1.TStats.Keeper * .06);
        }
        public void WhichTeamHasAChance()
        {
            if (T1CPM * 100 > randgen.Next(100))
            {
                Chance(T1);
            }

            else if (T2CPM * (1 / (1 - T1CPM)) * 100 > randgen.Next(100))
            {
                Chance(T2);
            }
        }
        public void Chance(Soccerteam team)
        {
            System.Console.WriteLine($"{team.TeamName} goes towards the goal!");
            Thread.Sleep(1500);
            if (T1CR * 100 > randgen.Next(100))
            {
                Goal(team);
            }
            else
            {
                System.Console.WriteLine($"{team.TeamName} misses the opertunity");
            }
        }
        public void Goal(Soccerteam team)
        {
            System.Console.WriteLine($"{team.TeamName} scores!");
            Thread.Sleep(500);
            team.Score++;
            ShowScore();
            Thread.Sleep(1500);
        }
        public void ShowScore()
        {
            System.Console.WriteLine($"The score is {T1.Score} against {T2.Score}");
        }
        public double T1CPM;
        public double T2CPM;
        public double T1CR;
        public double T2CR;
        public Soccerteam T1;
        public Soccerteam T2;
        Random randgen = new Random();
    }
}

