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
    /// Interaction logic for PlayerControl.xaml
    /// </summary>
    public partial class PlayerControl : UserControl
    {
        public PlayerControl()
        {
            InitializeComponent();
        }

        Player player = null;

        public Player GetPlayer() { return player; }
        public void SetPlayer(Player p)
        {
            player = p;
            UpdateUi();
        }
        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }
        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Increment()
        {
            int pts = player.GetPoints();
            pts++;
            player.SetPoints(pts);

            UpdateUi();
        }
        private void UpdateUi()
        {
            nameTB.Text = player != null ? player.GetName() : "";
            pointsTB.Text = player != null ? $"Points: {player.GetPoints().ToString()}" : "";
        }

        /// <summary>
        /// remove the players from the players board controls
        /// </summary>
        /// <param name="playerBoard"></param>
        public static void ResetAllPlayers(PlayerControl[,] playerBoard)
        {
            foreach (PlayerControl pc in playerBoard)
            {
                pc.SetPlayer(null);
            }
        }


        /// <summary>
        /// add new players to the board
        /// </summary>
        public static void CreateNewPlayers(PlayerControl[,] playerBoard, Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                playerBoard[i % 2, i / 2].SetPlayer(players[i]);
                playerBoard[i % 2, i / 2].Show();
            }
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
    }
}
