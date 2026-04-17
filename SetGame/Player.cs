using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetGame
{
    public class Player
    {
        public string name;
        public int points;

        public string GetName() { return name; }
        public int GetPoints() { return points; }

        public void SetPoints(int points) { this.points = points; }

        public Player(string name, int points)
        {
            this.name = name;
            this.points = points;
        }
    }
}
