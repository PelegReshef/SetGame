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
        public static void ArrangeCards(CardControl[,] board)
        {
            // find last cards on the board
            List<Card> toReplace = new List<Card>();
            for (int i = 4; i >= 0; i--)
            {
                for (int j = 0; j < 3; j++)
                {
                    toReplace.Add(board[j, i].currentCard);
                    if (toReplace.Count == 3)
                    {
                        goto checkNewPositions;

                    }
                }
            }

            checkNewPositions:
            
            // find first valid spots for new positions for the cards
            List<Card> potentialNewPositions = new List<Card>();
            List<Card> newPositions = new List<Card>();
            for (int i = 0; i < 5; i++)
            {
                for (int j =0; j < 3; j++)
                {
                    if (board[i, j].currentCard == null)
                    {
                        potentialNewPositions.Add(board[i, j].currentCard);
                    }
                    else
                    {
                        foreach (Card c in potentialNewPositions)
                        {
                            newPositions.Add(c);
                        }
                        potentialNewPositions.Clear();
                    }
                    
                    if (newPositions.Count >= 3)
                    {
                        goto replace;
                    }
                }
            }
            
            replace:
            // replace last cards with their new positions
            for (int i = 0; i < newPositions.Count; i++)
            {
                FindByCard(board, newPositions[i]).ChangeCard(toReplace[i]);
                FindByCard(board, toReplace[i]).DeleteCard();
            }
        }
        public static void AddNewCards(CardControl[,] board, Card[] cards)
        {
            Queue<Card> cardsQueue = new Queue<Card>(cards);
            for (int i = 0; i < 5; i++ )
            {
                for (int j = 0; j < 3; j++)
                {
                    if (board[j, i].GetCard() == null && cardsQueue.Count >= 1)
                    {
                        board[j, i].SetCard(cardsQueue.Dequeue());
                    }

                }
            }
        }
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
