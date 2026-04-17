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

        public void ChangeCard(Card c)
        {
            CardStackPanel.Children.Clear();
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



                CardStackPanel.Children.Add(p);
            }
        }
    }
}
