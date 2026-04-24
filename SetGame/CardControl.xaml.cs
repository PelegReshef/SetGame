using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SetGame
{
    /// <summary>
    /// Interaction logic for CardControl.xaml
    /// </summary>
    public partial class CardControl : UserControl
    {
        public CardControl()
        {
            InitializeComponent();
        }
        Card currentCard = null;

        public Card GetCard() { return currentCard; }
        private void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }
        private void Show()
        {
            this.Visibility = Visibility.Visible;
        }
        private void ChangeCard(Card c)
        {
            cardStackPanel.Children.Clear();
            currentCard = c;
            if (c == null)
            {
                return;
            }
            for (int i = 0; i < (int)c.GetCount() + 1; i++)
            {
                Path p = new Path()
                {
                    Margin = new Thickness(2),
                    Stretch = Stretch.Uniform,
                    StrokeThickness = 1
                };

                p.Data = (Geometry)FindResource(c.GetShape().ToString());

                var color = Colors.White; // will get overriden

                switch (c.GetColor())
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
                switch (c.GetFill())
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



                cardStackPanel.Children.Add(p);
            }
        }
        public void Highlight()
        {
            this.cardBorder.Background = new SolidColorBrush(Colors.LightGoldenrodYellow);
        }
        public void Unhighlight()
        {
            this.cardBorder.Background = new SolidColorBrush(Colors.White);
        }

        public void SetCard(Card c)
        {
            ChangeCard(c);
            Show();
        }
        public void DeleteCard()
        {
            ChangeCard(null);
            Hide();

        }
        public static void ResetBoard(CardControl[,] board)
        {
            foreach (var cc in board)
            {
                cc.ChangeCard(null);
            }
        }
        public static void ShowBoard(CardControl[,] board)
        {
            foreach (var cc in board)
            {
                cc.Show();
            }
        }

        public static void HideBoard(CardControl[,] board)
        {
            foreach (var cc in board)
            {
                cc.Hide();
            }
        }

    }
}
