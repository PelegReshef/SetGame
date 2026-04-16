using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetGame
{
    public class Game
    {
        Card[,] board = new Card[3, 5];
        List<(int x, int y)> selectedCards = new List<(int x, int y)>();
        List<Player> players = new List<Player>();


    }
}