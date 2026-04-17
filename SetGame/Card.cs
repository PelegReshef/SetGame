using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SetGame
{
    public class Card
    {
        Shape shape;
        Color color;
        Fill fill;
        Count count;

        public Card(Shape shape, Color color, Fill fill, Count count)
        {
            this.shape = shape;
            this.color = color;
            this.fill = fill;
            this.count = count;
        }

        public Shape GetShape() { return shape; }
        public Color GetColor() { return color; }
        public Fill GetFill() { return fill; }
        public Count GetCount() { return count; }

        public Enum[] GetTraitArray()
        {
            return new Enum[]
            {
                shape,
                color, 
                fill, 
                count
            };
        }
    }
    public enum Shape
    {
        Squiggle,
        Diamond,
        Capsule
    }
    public enum Color
    {
        Green,
        Purple,
        Red
    }
    public enum Fill
    {
        Full,
        Hollow,
        Striped
    }
    public enum Count
    {
        One,
        Two, 
        Three
    }
}
