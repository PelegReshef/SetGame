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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Card card = new Card(Shape.Capsule, Color.Green, Fill.Striped, Count.Three);
            DisplayCard(card, 2, 1);
        }

        public void DisplayCard(Card card, int x, int y)
        {
            Border border = new Border()
            {
                BorderThickness = new Thickness(2),
                Margin = new Thickness(15),
                BorderBrush = new SolidColorBrush(Colors.Black),
            };
            Viewbox vb = new Viewbox()
            {
                Margin = new Thickness(2),
            };
            StackPanel sp = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            for (int i = 0; i < (int)card.getCount() + 1; i++)
            {
                Path p = new Path()
                {
                    Margin = new Thickness(2),
                    Stretch = Stretch.Uniform,
                    StrokeThickness = 1
                };
                
                p.Data = (Geometry)this.Resources[card.GetShape().ToString()];

               var color = Colors.White; // will get overriden

                switch (card.getColor())
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
                switch (card.getFill())
                {
                    case Fill.Full:
                        opacity = 255;
                        break;
                    case Fill.Striped:
                        opacity = 64;
                        break;
                    case Fill.Hollow:
                        opacity = 0;
                        break;
                    default:
                        throw new Exception();
                }

                color.A = (byte)opacity;
                p.Fill = new SolidColorBrush(color);

                

                sp.Children.Add(p);
            }
            vb.Child = sp;
            border.Child = vb;
            Grid.SetColumn(border, x);
            Grid.SetRow(border, y);
            this.cardsGrid.Children.Add(border);
            
        }
    }
}
