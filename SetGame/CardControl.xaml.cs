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
using System.Windows.Media.Animation;
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
            this.cardBorder.RenderTransform = new ScaleTransform(1, 1);
        }
        Card currentCard = null;
        bool IsDisabled = false;

        public Card GetCard() { return currentCard; }
        private void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }
        private void Show()
        {
            this.Visibility = Visibility.Visible;
        }
        public void Disable()
        {
            this.IsDisabled = true;
        }
        public void Enable()
        {
            this.IsDisabled = false;
        }
        private void ChangeCard(Card c)
        {
            Unhighlight();
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

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            // make cards bigger when you hover on them with the mouse
            if (IsDisabled) return;

            Cursor = Cursors.Hand;
            DoubleAnimation da = new DoubleAnimation();
            da.To = 1.06;
            da.Duration = TimeSpan.FromMilliseconds(450);
            da.EasingFunction = new BounceEase();
            cardBorder.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            cardBorder.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, da);
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            // make cards return to normal size when you stop hovering
            if (IsDisabled) return;

            Cursor = Cursors.Arrow;
            DoubleAnimation da = new DoubleAnimation();
            da.To = 1;
            da.Duration = TimeSpan.FromMilliseconds(350);
            da.EasingFunction = new QuadraticEase();
            cardBorder.RenderTransform.BeginAnimation(ScaleTransform.ScaleXProperty, da);
            cardBorder.RenderTransform.BeginAnimation(ScaleTransform.ScaleYProperty, da);

        }
        /// <summary>
        /// make all the cards on the board empty
        /// </summary>
        public static void ResetBoard(CardControl[,] board)
        {
            foreach (var cc in board)
            {
                cc.ChangeCard(null);
            }
        }
        /// <summary>
        /// make the cards on the board visible
        /// </summary>
        /// <param name="board"></param>
        public static void ShowBoard(CardControl[,] board)
        {
            foreach (var cc in board)
            {
                cc.Show();
            }
        }
        /// <summary>
        /// make the cards on the board invisble
        /// </summary>
        public static void HideBoard(CardControl[,] board)
        {
            foreach (var cc in board)
            {
                cc.Hide();
            }
        }
        /// <summary>
        /// make cards on the board unresponsive
        /// </summary>
        public static void DisableBoard(CardControl[,] board)
        {
            foreach (var cc in board)
            {
                cc.Disable();
            }
        }
        /// <summary>
        /// make cards on the board responsive
        /// </summary>
        public static void EnableBoard(CardControl[,] board)
        {
            foreach (var cc in board)
            {
                cc.Enable();
            }
        }

        /// <summary>
        /// find a CardControl on the board by the card it holds
        /// </summary>
        /// <returns>the CardControl if it was founds, else null</returns>
        public static CardControl FindByCard(CardControl[,] board, Card c)
        {
            foreach (var cc in board)
            {
                if (cc.currentCard?.Equals(c) ?? false)
                {
                    return cc;
                }
            }
            return null;
        }
        /// <summary>
        /// rearragne the cards on the board to fill empty spots.
        /// </summary>
        public static void ArrangeCards(CardControl[,] board)
        {
            // find last cards on the board
            var toReplace = GetCardsToReplace();

            // find first valid spots for new positions for the cards
            var newPositions = GetNewPositions();

            // replace last cards with their new positions
            for (int i = 0; i < newPositions.Count; i++)
            {
                int oldX = Grid.GetColumn(toReplace[i]); 
                int oldY = Grid.GetRow(toReplace[i]);

                int newX = Grid.GetColumn(newPositions[i]);
                int newY = Grid.GetRow(newPositions[i]);

                // keep replacing only if the new position is earlier 
                // on the board than the old position
                if ((oldY < newY) || (oldY == newY && oldX >= newX))
                {
                    return;
                }
                newPositions[i].SetCard(toReplace[i].currentCard);
                toReplace[i].DeleteCard();
            }




            List<CardControl> GetCardsToReplace()
            {
                List<CardControl> ret = new List<CardControl>();
                for (int y = 4; y >= 0; y--)
                {
                    // this loop goes from zero because I want the cards 
                    // to get arranged towards the left side of the board
                    for (int x = 0; x < 3; x++)
                    {
                        if (board[x, y].currentCard != null)
                        {
                            ret.Add(board[x, y]);
                        }
                        if (ret.Count >= 3)
                        {
                            return ret;
                        }
                    }
                }
                return ret;
            }

            List<CardControl> GetNewPositions()
            {
                List<CardControl> ret = new List<CardControl>();
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (board[x, y].currentCard == null)
                        {
                            ret.Add(board[x, y]);
                        }

                        if (ret.Count >= 3)
                        {
                            return ret;
                        }
                    }
                }
                return ret;
            }
        }
        /// <summary>
        /// add new cards to the board
        /// </summary>
        public static void AddNewCards(CardControl[,] board, Card[] cards)
        {
            Queue<Card> cardsQueue = new Queue<Card>(cards);
            for (int i = 0; i < 5; i++ )
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[j, i].currentCard == null && cardsQueue.Count >= 1)
                    {
                        board[j, i].SetCard(cardsQueue.Dequeue());
                    }

                }
            }
        }
        /// <summary>
        /// delete cards from the board
        /// </summary>
        public static void DeleteCards(CardControl[,] board, Card[] cards)
        {
            foreach (var card  in cards)
            {
                FindByCard(board, card)?.DeleteCard();
            }
        }


        /// <summary>
        /// replace an array of cards with another array on the board
        /// </summary>
        /// <returns>true if all cards were succefully replaced</returns>
        public static bool ReplaceCards(CardControl[,] board, Card[] oldCards, Card[] newCards)
        {
            bool success = true;
            for (int i = 0; i < oldCards.Length; i++)
            {
                CardControl cc = FindByCard(board, oldCards[i]);
                if (cc != null)
                {
                    cc.ChangeCard(newCards[i]);
                }
                else
                {
                    success = false;
                }
            }
            return success;
        }

    }
}
