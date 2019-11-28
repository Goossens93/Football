using System;

namespace OSM
{
    class Project
    {
        static void Main(string[] args)
        {
            Soccerteam yourteam = new Soccerteam();
            yourteam.MakeTeam();
            yourteam.TransferPlayer();
            // yourteam.TransferPlayer();
        }
    }
}