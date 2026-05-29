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

        int playersCount = 0;

        bool isGameActive = false;


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
            }
        }

        List<Player> CreatePlayersList()
        {
            List<Player> ret = new List<Player>();
            Window1 win = new Window1();
            
            if (win.ShowDialog() == true)
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
                if (cardsCount <= 12 && cards.Count >= 3)
                {
                    List<Card> newCards = DrawCards(3);
                    CardControl.ReplaceCards(board, selectedCards.ToArray(), newCards.ToArray());
                }
                else
                {
                    CardControl.DeleteCards(board, selectedCards.ToArray());
                    CardControl.ArrangeCards(board);
                    cardsCount -= 3;

                    if (cardsCount <= 0)
                    {
                        outputTBlock.Text = "Game is finished! \nPress on End game to see your scores on the leaderboard.";
                    }
                }
                
            }
            else
            {
                outputTBlock.Text = "Oops! Not a Set";
            }
            ResetCardSelction();
            cardsLeftTBlock.Text = "Cards Left: " + cards.Count.ToString();

        }

        bool needToSelectPlayer = false;

        /// <summary>
        /// decides what happens when a card get slected
        /// </summary>
        public void CardControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CardControl cc = (CardControl)sender;
            if (!isGameActive)
            {
                return;  
            }
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
        /// <summary>
        /// remove highlight from all cards
        /// </summary>
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


        /// <summary>
        /// draw new cards from the cards list
        /// </summary>
        List<Card> DrawCards(int amount)
        {
            if (cards.Count <= 0)
            {
                return new List<Card>();
            }
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

        /// <summary>
        /// check if the three cards given are a set
        /// </summary>
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

        /// <summary>
        /// initialize a new game
        /// </summary>
        void NewGame()
        {
            var players = CreatePlayersList();
            if (!players.Any())
            {
                return;
            }
            playersCount = players.Count;

            PlayerControl.CreateNewPlayers(playerBoard, players.ToArray());
            CardControl.HideBoard(board);
            CardControl.ResetBoard(board);

            cards = CreateAllCardsList();

            //start game with 12 cards
            cardsCount = 12;
            List<Card> newCards = DrawCards(12);
            CardControl.AddNewCards(board, newCards.ToArray());

            cardsLeftTBlock.Text = "Cards Left: " + cards.Count.ToString();
            CardControl.EnableBoard(board);
            isGameActive = true;
        }
        /// <summary>
        /// end current game and show leaderboard
        /// </summary>
        void EndGame()
        {
            if (!isGameActive)
            {
                return;
            }
            isGameActive = false;
            outputTBlock.Text = "Game ended. Press on new game to start playing.";
            try
            {
                foreach (PlayerControl pc in playersGrid.Children)
                {
                    if (pc.GetPlayer() is Player p)
                    {
                        string sqlStr = $"INSERT INTO PlayersTable (PlayerName, PlayerScore, PlayersAmount, [Time])" +
                            $" VALUES ('{p.GetName()}', {p.GetPoints()}, {playersCount}, '{DateTime.Now}')";
                        DAL.ExecuteNonQuery(sqlStr);
                    }
                }
            }
            catch
            {
                var result = MessageBox.Show("There is an error trying to update your score." +
                    "\n Would you like to see the leaderboard anyway?", "Error", MessageBoxButton.YesNo, MessageBoxImage.Error);
                if (result != MessageBoxResult.Yes)
                {
                    CardControl.DisableBoard(board);
                    return;
                }
            }
            CardControl.DisableBoard(board);
            LeaderboardWindow win = new LeaderboardWindow();
            win.ShowDialog();
        }

        private void newGameBtn_Click(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void ThreeCardsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (cardsCount > 12 || isGameActive == false)
            {
                return;
            }
            cardsCount += 3;
            CardControl.AddNewCards(board, DrawCards(3).ToArray());
            cardsLeftTBlock.Text = "Cards Left: " + cards.Count.ToString();

        }

        private void EndGameButton_Click(object sender, RoutedEventArgs e)
        {
            EndGame();
        }
    }
}
