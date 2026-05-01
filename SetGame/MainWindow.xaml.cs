using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitBoard();
            InitPlayers();
            CardControl.HideBoard(board);

        }
        Random rnd = new Random();

        // represents the game board
        CardControl[,] board = new CardControl[3, 5];

        // represents the players currently in game
        PlayerControl[,] playerBoard = new PlayerControl[2, 2];

        // how many cards are currently open
        int cardsCount = 0;

        // cards left to open in the game
        List<Card> cards = new List<Card>();

        // cards currently selected by user (up to 3)
        List<Card> selectedCards = new List<Card>();


        /// <summary>
        /// initialize the board variable with a new CardControl 2D array
        /// </summary>
        public void InitBoard()
        {
            for (int x = 0; x < 3; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    CardControl cc = new CardControl();
                    cc.MouseDown += CardControl_MouseDown;
                    Grid.SetColumn(cc, x);
                    Grid.SetRow(cc, y);
                    board[x, y] = cc;
                    cardsGrid.Children.Add(cc);
                }
            }
        }

        /// <summary>
        /// initialize the players grid with new PlayerControl array.
        /// meant to be called only once
        /// </summary>
        void InitPlayers()
        {
            for (int i = 0; i < 4; i++)
            {
                PlayerControl pc = new PlayerControl();
                Grid.SetRow(pc, i / 2);
                Grid.SetColumn(pc, i % 2);
                pc.MouseDown += PlayerControl_MouseButton;
                pc.Hide();
                playersGrid.Children.Add(pc);
                playerBoard[i % 2, i / 2] = pc;

                //if (string.IsNullOrWhiteSpace(players[i]))
                //{
                //    continue;
                //}
                //Border pc = new Border()
                //{
                //    BorderThickness = new Thickness(2),
                //    BorderBrush = new SolidColorBrush(Colors.Black),
                //    CornerRadius = new CornerRadius(10),
                //    Margin = new Thickness(10),
                    
                //};
                //Grid.SetRow(pc, i / 2);
                //Grid.SetColumn(pc, i % 2);
                //pc.MouseDown += PlayerBorder_MouseButton;

                //Grid g = new Grid();
                //g.RowDefinitions.Add(new RowDefinition());
                //g.RowDefinitions.Add(new RowDefinition());

                //TextBlock tb1 = new TextBlock()
                //{
                //    Text = $"Name: {players[i]}",
                //    FontSize = 15
                //};
                //TextBlock tb2 = new TextBlock()
                //{
                //    FontSize = 15
                //};
                //int points = 0;
                //tb2.Tag = points;
                //tb2.Text = $"Points: {points}";
                //Grid.SetRow(tb2, 1);
                //g.Children.Add(tb1);
                //g.Children.Add(tb2);
                //pc.Child = g;
                //pc.Tag = (tb2, players[i]);

            }
        }

        List<Player> CreatePlayersList()
        {
            List<Player> ret = new List<Player>();
            Window1 win = new Window1();
            //win.DialogResult = win.ShowDialog();
            if (win.ShowDialog().Value)
            {
                foreach (string name in win.names)
                {
                    Player p = new Player(name, 0);
                    ret.Add(p);
                }
            }
            return ret;
        }

        private void PlayerControl_MouseButton(object sender, MouseButtonEventArgs e)
        {
            if (!needToSelectPlayer)
            {
                return;
            }
            PlayerControl pc = (PlayerControl)sender;
            
            if (IsSet(selectedCards.ToArray()))
            {
                outputTBlock.Text = $"Well Done! Its a Set! \n {pc.GetPlayer().GetName()} gets a point!";

                pc.Increment();

                needToSelectPlayer = false;

                // only need to add cards when there are 12 cards
                // or less before finding the set
                if (cardsCount <= 12)
                {
                    // add new cards in the place of the current set
                    List<Card> newCards = DrawCards(3);
                    CardControl.ReplaceCards(board, selectedCards.ToArray(), newCards.ToArray());
                }
                else
                {
                    CardControl.ArrangeCards(board);
                    // pull the cards in the last row inside the first 4 rows
                    //List<Point> positionsInside = new List<Point>();
                    //List<Card> cardsOutside = new List<Card>();


                    //for (int x = 0; x < 3; x++)
                    //{
                    //    if (!selectedCards.Contains(boardDisplay[x, 4]))
                    //    {
                    //        Card c = boardCards[x, 4];
                    //        cardsOutside.Add(c);

                    //    }
                    //    DeleteCard(x, 4);
                    //}
                    //foreach(Border border in selectedCards)
                    //{
                    //    Point pos = (Point)border.Tag;
                    //    if (pos.Y < 4)
                    //    {
                    //        positionsInside.Add(pos);
                    //    }
                    //}
                    //for (int i = 0; i < cardsOutside.Count; i++)
                    //{
                    //    Point pos = positionsInside[i];
                    //    SetCard(cardsOutside[i], (int)pos.X, (int)pos.Y);
                    //}
                    //cardsCount -= 3;

                }
            }
            else
            {
                outputTBlock.Text = "Oops! Not a Set";
            }
            ResetCardSelction();

        }

        bool needToSelectPlayer = false;

        /// <summary>
        /// decides what happens when a card get slected
        /// </summary>
        public void CardControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CardControl cc = (CardControl)sender;

            // if the card was already selected,
            // deselect it and update list
            if (selectedCards.Contains(cc.GetCard()))
            {
                cc.Unhighlight();
                selectedCards.Remove(cc.GetCard());
                outputTBlock.Text = "Select three cards to create a Set";
                return;
            }

            // if this is the 4th card selected, 
            // deselct all cards selected before
            if (selectedCards.Count() >= 3)
            {
                ResetCardSelction();
            }

            cc.Highlight();
            selectedCards.Add(cc.GetCard());

            // if this is the 3rd card selected, 
            // let players check if all selected are a set
            if (selectedCards.Distinct().Count() == 3)
            {
                needToSelectPlayer= true;
                outputTBlock.Text = "Who found the Set? Click on them!";
            }
            else
            {
                outputTBlock.Text = "Select three cards to create a Set";
                needToSelectPlayer = false;
            }
        }
        void ResetCardSelction()
        {
            foreach (var card in selectedCards)
            {
                var cc = CardControl.FindByCard(this.board, card);
                cc?.Unhighlight();

            }
            selectedCards.Clear();
        }

        /// <summary>
        /// create a new list of all the cards in the game
        /// </summary>
        List<Card> CreateAllCardsList()
        {
            var cards = new List<Card>();

            foreach (Shape shape in Enum.GetValues(typeof(Shape)))
            {
                foreach(Color color in Enum.GetValues(typeof(Color)))
                {
                    foreach (Fill fill in Enum.GetValues(typeof(Fill)))
                    {
                        foreach(Count count in Enum.GetValues(typeof(Count)))
                        {
                             Card c = new Card(shape, color, fill, count);
                             cards.Add(c);
                        }
                    }
                }
            }
            return cards;

        }

        //void AddNewCards(List<Point> positions)
        //{
        //    foreach(Point p in positions)
        //    {
        //        int x = (int)p.X;
        //        int y = (int)p.Y;

        //        int index = rnd.Next(0, cards.Count);
        //        Card c = cards[index];

        //        SetCard(c, x, y);
        //        cards.Remove(c);
                
        //    }
        //}

        /// <summary>
        /// get a list of new cards that were
        /// drawn from the cards list by a specific amount
        /// </summary>
        List<Card> DrawCards(int amount)
        {
            List<Card> ret = new List<Card>();
            for (int i = 0; i < amount ; i++)
            {
                int index = rnd.Next(0, cards.Count);
                Card c = cards[index];
                ret.Add(c);

                cards.Remove(c);
            }
            return ret;
        }

        bool IsSet(Card[] cards)
        {
            Enum[][] traits =
            {
                cards[0].GetTraitArray(),
                cards[1].GetTraitArray(),
                cards[2].GetTraitArray(),
            };
            for(int i = 0; i < 4; i++)
            {
                // the traits of the cards have to be either all
                // different or all the same to be a set
                if(!((traits[0][i].Equals(traits[1][i]) &&
                    traits[1][i].Equals(traits[2][i]) ) ||

                    (!traits[0][i].Equals(traits[1][i]) &&
                    !traits[1][i].Equals(traits[2][i]) &&
                    !traits[2][i].Equals(traits[0][i]))))
                {
                    return false;
                }
            }
            return true;
        }

        void NewGame()
        {
            var players = CreatePlayersList();
            PlayerControl.CreateNewPlayers(playerBoard, players.ToArray());
            CardControl.ResetBoard(board);
            cardsCount = 12;

            cards = CreateAllCardsList();

            //start game with 12 cards
            List<Card> newCards = DrawCards(12);
            CardControl.AddNewCards(board, newCards.ToArray());

            //for (int x = 0; x < 3; x++)
            //{
            //    for (int y = 0; y < 4; y++) // game starts with 12 cards open
            //    {
            //        int index = rnd.Next(0, cards.Count);
            //        Card c = cards[index];
            //        boardCards[x, y] = c;
            //        boardDisplay[x, y].Child = RenderCard(c, x, y);
            //        boardDisplay[x, y].Visibility = Visibility.Visible;
            //        cards.Remove(c);

            //    }
            //}
        }

        private void newGameBtn_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void ThreeCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (cardsCount > 12)
            {
                return;
            }
            cardsCount += 3;
            CardControl.AddNewCards(board, DrawCards(3).ToArray());
            
        }

    }
}
