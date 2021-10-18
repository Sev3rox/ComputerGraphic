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

namespace Project3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string type = "RGB";
        public MainWindow()
        {
            InitializeComponent();
            this.RGBBtn.Background = Brushes.Blue;
            this.RGB.Background = Brushes.Blue;
        }

        private void Button_RGB(object sender, RoutedEventArgs e)
        {
            type = "RGB";

            Label1.Content = "Red";
            Label2.Content = "Green";
            Label3.Content = "Blue";

            Slider4.Visibility = Visibility.Hidden;
            Stack.Visibility = Visibility.Hidden;

            Slider1.Maximum = 255;
            Slider1.Value = 0;
            Slider1.TickFrequency = 1;
            Slider2.Maximum = 255;
            Slider2.Value = 0;
            Slider2.TickFrequency = 1;
            Slider3.Maximum = 255;
            Slider3.Value = 0;
            Slider3.TickFrequency = 1;

            this.RGBBtn.Background = Brushes.Blue;
            this.RGB.Background = Brushes.Blue;
            this.CMYKBtn.Background = Brushes.LightGray;
            this.CMYK.Background = Brushes.LightGray;
        }

        private void Button_CMYK(object sender, RoutedEventArgs e)
        {
            type = "CMYK";

            Label1.Content = "Cyan";
            Label2.Content = "Magenta";
            Label3.Content = "Yellow";

            Slider4.Visibility = Visibility.Visible;
            Stack.Visibility = Visibility.Visible;

            Slider1.Maximum = 1;
            Slider1.Value = 0;
            Slider1.TickFrequency = 0.01;
            Slider2.Maximum = 1;
            Slider2.Value = 0;
            Slider2.TickFrequency = 0.01;
            Slider3.Maximum = 1;
            Slider3.Value = 0;
            Slider3.TickFrequency = 0.01;
            Slider4.Value = 0;

            this.CMYKBtn.Background = Brushes.Blue;
            this.CMYK.Background = Brushes.Blue;
            this.RGBBtn.Background = Brushes.LightGray;
            this.RGB.Background = Brushes.LightGray;
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            if (type == "RGB")
            {
                float cyan = 0;
                float magenta = 0;
                float yellow = 0;
                float black = 0;

                var red = Int32.Parse(Text1.Text);
                var green = Int32.Parse(Text2.Text);
                var blue = Int32.Parse(Text3.Text);

                black = float.Parse(((float)(255 - (Math.Max(red, Math.Max(green, blue)))) / 255).ToString("0.00"));
                cyan = float.Parse(((255 - red - black * 255) / (255 - black * 255)).ToString("0.00"));
                if (cyan <= 0)
                    cyan = 0;
                magenta = float.Parse(((255 - green - black * 255) / (255 - black * 255)).ToString("0.00"));
                if (magenta <= 0)
                    magenta = 0;
                yellow = float.Parse(((255 - blue - black * 255) / (255 - black * 255)).ToString("0.00"));
                if (yellow <= 0)
                    yellow = 0;

                RGBText.Content = "Red:" +red+"  Green:" + green + "  Blue" + blue;

                Color1.Background= new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));

                CMYKText.Content = "Cyan:" + cyan + "  Magenta:" + magenta +'\n'+"Yellow:" + yellow + "  Black:" + black;

                Color2.Background = new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
            }
            else
            {
                int red = 0;
                int green = 0;
                int blue = 0;

                var cyan = float.Parse(Text1.Text);
                var magenta = float.Parse(Text2.Text);
                var yellow = float.Parse(Text3.Text);
                var black = float.Parse(Text4.Text);

                red = (int)(255 * (1-cyan)*(1-black));
                green = (int)(255 * (1 - magenta) * (1 - black));
                blue = (int)(255 * (1 - yellow) * (1 - black));

                CMYKText.Content = "Cyan:" + cyan + "  Magenta:" + magenta + '\n' + "Yellow:" + yellow + "  Black:" + black;

                Color2.Background = new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));

                RGBText.Content = "Red:" + red + "  Green:" + green + "  Blue" + blue;

                Color1.Background = new SolidColorBrush(Color.FromRgb((byte)red, (byte)green, (byte)blue));
            }
        }
    }
}
