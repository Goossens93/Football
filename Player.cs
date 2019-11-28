using System;

namespace OSM
{
    /// <summary>
    /// Creates a player for you soccerteam
    /// </summary>
    public class Player
    {
        public Player(string PName, int PSkill)
        {
            PlayerName = PName;
            PlayerSkill = PSkill;
        }
        public string PlayerName;
        public int PlayerSkill;
        public void TrainPlayer(int hours)
        {
            Random randgen = new Random();
            int rand = randgen.Next(100);
            if(rand < (5 * hours))
            {
                PlayerSkill ++;
            }
        }
    }
}