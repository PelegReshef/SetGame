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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        public List<string> names = new List<string>();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            names.Add(Player1Box.Text);
            names.Add(Player2Box.Text);
            names.Add(Player3Box.Text);
            names.Add(Player4Box.Text);

            DialogResult = true;
        }
    }
}
