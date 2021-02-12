using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bowling.Models;

namespace Bowling.ViewModels
{
    public class BowlingViewModel
    {
        public BowlingViewModel()
        {
            // Can easily add multiple games
            Games = new Game[1];
            Games[0] = new Game();
        }

        public Game[] Games { get; }

        public int TotalScore => Games.Sum(game => game.TotalScore);
    }
}