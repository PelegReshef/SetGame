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
        Border[,] board = new Border[3, 5];
        MainWindow mw;
        public BoardDisplay(MainWindow mw)
        {
            this.mw = mw;
        }
        public void InitBoard()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    Border b = new Border()
                    {
                        BorderThickness = new Thickness(2),
                        Margin = new Thickness(15),
                        BorderBrush = new SolidColorBrush(Colors.Black),
                        CornerRadius = new CornerRadius(20),
                        Background = new SolidColorBrush(Colors.White),
                        Tag = new Point(x, y),
                    };
                    b.MouseDown += mw.Border_MouseDown;
                    Grid.SetColumn(b, x);
                    Grid.SetRow(b, y);
                    board[x, y] = b;
                    mw.cardsGrid.Children.Add(b);
                }
            }
        }
        public void SetCard(Card c, int x, int y)
        {
            board[x, y].Child = RenderCard(c, x, y);
            board[x, y].Visibility = Visibility.Visible;

        }
        public void DeleteCard(int x, int y)
        {
            board[x, y].Child = null;
            board[x, y].Visibility = Visibility.Collapsed;

        }
        public void HighlightCard(int x, int y)
        {
            board[x, y].Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
        }
        public void UnHighlightCard(int x, int y)
        {
            board[x, y].Background = new SolidColorBrush(Colors.White);
        }
        public Viewbox RenderCard(Card card, int x, int y)
        {
            Viewbox vb = new Viewbox()
            {
                Margin = new Thickness(2),
            };
            StackPanel sp = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            for (int i = 0; i < (int)card.GetCount() + 1; i++)
            {
                Path p = new Path()
                {
                    Margin = new Thickness(2),
                    Stretch = Stretch.Uniform,
                    StrokeThickness = 1
                };

                p.Data = (Geometry)mw.FindResource(card.GetShape().ToString());

                var color = Colors.White; // will get overriden

                switch (card.GetColor())
                {
                    case Color.Red:
                        color = Colors.Red;
                        break;
                    case Color.Green:
                        color = Colors.Green;
                        break;
                    case Color.Purple:
                        color = Colors.Purple;
                        break;
                    default:
                        throw new Exception();
                }

                p.Stroke = new SolidColorBrush(color);
                int opacity = 0;
                switch (card.GetFill())
                {
                    case Fill.Full:
                        opacity = 255;
                        break;
                    case Fill.Striped:
                        opacity = 75;
                        break;
                    case Fill.Hollow:
                        opacity = 0;
                        break;
                    default:
                        throw new Exception();
                }

                color.A = (byte)opacity;
                p.Fill = new SolidColorBrush(color);



                sp.Children.Add(p);
            }
            vb.Child = sp;
            return vb;
        }

        public void ResetBoard()
        {
            foreach (var b in board)
            {
                b.Child = null;
            }
        }
        public void ShowBoard()
        {
            foreach(var b in board)
            {
                b.Visibility = Visibility.Visible;
            }
        }

        public void HideBoard()
        {
            foreach(var b in board)
            {
                b.Visibility = Visibility.Collapsed;
            }
        }


    }
}
