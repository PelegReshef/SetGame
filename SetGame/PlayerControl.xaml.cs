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
            UpdateUI();

        }

        public void Increment()
        {
            int pts = player.GetPoints();
            pts++;
            player.SetPoints(pts);

            UpdateUI();
        }
        private void UpdateUI()
        {
            nameTB.Text = player.GetName();
            pointsTB.Text = player.GetPoints().ToString();
        }
    }
}
