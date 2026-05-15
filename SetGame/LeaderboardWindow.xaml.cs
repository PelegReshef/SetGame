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
using System.Windows.Shapes;

namespace SetGame
{
    /// <summary>
    /// Interaction logic for LeaderboardWindow.xaml
    /// </summary>
    public partial class LeaderboardWindow : Window
    {
        public LeaderboardWindow()
        {
            InitializeComponent();
            var dv = DAL.GetDataView("SELECT * From PlayersTable ORDER BY PlayerScore DESC");
            this.leaderboard.ItemsSource = dv;
        }

        public void FilterBy(string column, string order)
        {
            var dv = DAL.GetDataView($"SELECT * FROM PlayersTable ORDER BY {column} {order}");
            if (this.leaderboard != null)
            {
                this.leaderboard.ItemsSource = dv;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var cb = (ComboBox)sender;

            switch (cb.SelectedIndex)
            {
                case 0:
                    FilterBy("PlayerName", "ASC");
                    break;
                case 1:
                    FilterBy("PlayerName", "DESC");
                    break;
                case 2:
                    FilterBy("PlayerScore", "DESC");
                    break;
                case 3:
                    FilterBy("PlayerScore", "ASC");
                    break;
                case 4:
                    FilterBy("Time", "DESC");
                    break;
                case 5:
                    FilterBy("Time", "ASC");
                    break;
            }
        }
    }
}
