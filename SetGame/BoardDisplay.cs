using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace SetGame
{
    public class BoardDisplay
    {
        CardControl[,] board;
        public BoardDisplay(CardControl[,] board)
        {
            this.board = board;
        }
        public void SetCard(Card c, int x, int y)
        {
            board[x, y].ChangeCard(c);
            board[x, y].Show();
        }
        public void DeleteCard(int x, int y)
        {
            board[x, y].ChangeCard(null);
            board[x, y].Hide();

        }
        public void HighlightCard(int x, int y)
        {
            board[x, y].Highlight();
        }
        public void UnhighlightCard(int x, int y)
        {
            board[x, y].Unhighlight();
        }
        //public Viewbox RenderCard(Card card, int x, int y)
        //{
        //    Viewbox vb = new Viewbox()
        //    {
        //        Margin = new Thickness(2),
        //    };
        //    StackPanel sp = new StackPanel()
        //    {
        //        Orientation = Orientation.Horizontal,
        //        HorizontalAlignment = HorizontalAlignment.Center
        //    };
        //    for (int i = 0; i < (int)card.GetCount() + 1; i++)
        //    {
        //        Path p = new Path()
        //        {
        //            Margin = new Thickness(2),
        //            Stretch = Stretch.Uniform,
        //            StrokeThickness = 1
        //        };

        //        p.Data = (Geometry)mw.FindResource(card.GetShape().ToString());

        //        var color = Colors.White; // will get overriden

        //        switch (card.GetColor())
        //        {
        //            case Color.Red:
        //                color = Colors.Red;
        //                break;
        //            case Color.Green:
        //                color = Colors.Green;
        //                break;
        //            case Color.Purple:
        //                color = Colors.Purple;
        //                break;
        //            default:
        //                throw new Exception();
        //        }

        //        p.Stroke = new SolidColorBrush(color);
        //        int opacity = 0;
        //        switch (card.GetFill())
        //        {
        //            case Fill.Full:
        //                opacity = 255;
        //                break;
        //            case Fill.Striped:
        //                opacity = 75;
        //                break;
        //            case Fill.Hollow:
        //                opacity = 0;
        //                break;
        //            default:
        //                throw new Exception();
        //        }

        //        color.A = (byte)opacity;
        //        p.Fill = new SolidColorBrush(color);



        //        sp.Children.Add(p);
        //    }
        //    vb.Child = sp;
        //    return vb;
        //}

        public void ResetBoard()
        {
            foreach (var cc in board)
            {
                cc.ChangeCard(null);
            }
        }
        public void ShowBoard()
        {
            foreach(var cc in board)
            {
                cc.Show();
            }
        }

        public void HideBoard()
        {
            foreach(var cc in board)
            {
                cc.Hide();
            }
        }


    }
}
