using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Project1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int Type2;
        private int st = 3;
        private int count = 0;
        private int xoldold = -1;
        private int yoldold = -1;
        private int xold = -1;
        private int yold = -1;
        private int xvec = -1;
        private int yvec = -1;
        private int xpoint = -1;
        private int ypoint = -1;
        private int isover = 0;
        private int issecond = 0;
        private List<Line> linie = new();
        //private List<Line> backlinie = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            canvas.Focusable = true;

            Loaded += (x, y) => Keyboard.Focus(canvas);
            Button_Tworzenie(null,null);
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            txt.Content = "x: "+(e.GetPosition(canvas).X.ToString())+"   y: "+(e.GetPosition(canvas).Y.ToString());
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Type2 == 1)
            {

                if (Convert.ToInt32(sttb.Text) > 0)
                    st = Convert.ToInt32(sttb.Text);

                if (count == 0)
                {
                    xold = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                    yold = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                    xoldold = xold;
                    yoldold = yold;
                    count++;
                    linie.Clear();
                    canvas.Children.Clear();
                }
                else
                {
                    var x = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                    var y = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                    var lin = new Line();
                    lin.X1 = xold;
                    lin.Y1 = yold;
                    lin.X2 = x;
                    lin.Y2 = y;
                    xold = x;
                    yold = y;
                    lin.Stroke = System.Windows.Media.Brushes.Blue;
                    lin.StrokeThickness = 3;
                    canvas.Children.Add(lin);
                    linie.Add(lin);
                    count++;

                    if (count == st)
                    {

                        var lin1 = new Line();
                        lin1.X1 = xold;
                        lin1.Y1 = yold;
                        lin1.X2 = xoldold;
                        lin1.Y2 = yoldold;
                        lin1.Stroke = System.Windows.Media.Brushes.Blue;
                        lin1.StrokeThickness = 3;
                        canvas.Children.Add(lin1);
                        linie.Add(lin1);
                        count = 0;
                    }
                }
            }
            else if (Type2 == 2)
            {
                foreach (Shape s in canvas.Children)
                {
                    if (s.IsMouseDirectlyOver)
                    {
                        isover = 1;
                        xvec = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                        yvec = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                    }
                }
            }
            else if (Type2 == 3&&issecond!=1)
            { 
                xpoint = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                ypoint = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                issecond = 1;
            }
            else if (Type2 == 3 && issecond == 1)
            {
                foreach (Shape s in canvas.Children)
                {
                    if (s.IsMouseDirectlyOver)
                    {
                        isover = 1;
                        xvec = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                        yvec = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                        issecond = 0;
                    }
                }
            }

            else if (Type2 == 4 && issecond != 1)
            {
                xpoint = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                ypoint = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                issecond = 1;
            }
            else if (Type2 == 4 && issecond == 1)
            {
                foreach (Shape s in canvas.Children)
                {
                    if (s.IsMouseDirectlyOver)
                    {
                        isover = 1;
                        xvec = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                        yvec = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                        issecond = 0;
                    }
                }
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Type2==2 && isover==1) {
                var x = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                var y = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                var xval = x-xvec;
                var yval = y - yvec;

                foreach (Shape s in canvas.Children)
                {

                        ((Line)s).X1 = ((Line)s).X1 + xval;
                        ((Line)s).X2 = ((Line)s).X2 + xval;
                        ((Line)s).Y1 = ((Line)s).Y1 + yval;
                        ((Line)s).Y2 = ((Line)s).Y2 + yval;
                    
                }
                isover = 0;
            }
            else if (Type2 == 3 && isover == 1)
            {
                var x = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                var y = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                var a = Math.Sqrt(Math.Abs(xpoint - xvec) * Math.Abs(xpoint - xvec) + Math.Abs(ypoint - yvec) * Math.Abs(ypoint - yvec));
                var b = Math.Sqrt(Math.Abs(x-xvec)* Math.Abs(x - xvec)+ Math.Abs(y - yvec)* Math.Abs(y - yvec));
                var c = Math.Sqrt(Math.Abs(xpoint - x) * Math.Abs(xpoint - x) + Math.Abs(ypoint - y) * Math.Abs(ypoint - y));

                var angle = Math.Acos(((a*a+c*c-b*b)/(2*a*c)));
                angle = angle / 3.14 * 180;

                foreach (Shape s in canvas.Children)
                {
                    var x1pom = ((Line)s).X1;
                    var x2pom = ((Line)s).X2;
                    var y1pom = ((Line)s).Y1;
                    var y2pom = ((Line)s).Y2;

                    ((Line)s).X1 = xpoint+(x1pom - xpoint)*Math.Cos(angle) -(y1pom - ypoint) * Math.Sin(angle);
                    ((Line)s).X2 = xpoint + (x2pom - xpoint) * Math.Cos(angle) - (y2pom - ypoint) * Math.Sin(angle);
                    ((Line)s).Y1 = ypoint + (x1pom - xpoint) * Math.Sin(angle) + (y1pom - ypoint) * Math.Cos(angle);
                    ((Line)s).Y2 = ypoint + (x2pom - xpoint) * Math.Sin(angle) + (y2pom - ypoint) * Math.Cos(angle);

                }
                isover = 0;
            }
            else if (Type2 == 4 && isover == 1)
            {

                var x = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                var y = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                var len1= Math.Sqrt(Math.Abs(xpoint - xvec) * Math.Abs(xpoint - xvec) + Math.Abs(ypoint - yvec) * Math.Abs(ypoint - yvec));
                var len2 = Math.Sqrt(Math.Abs(xpoint - x) * Math.Abs(xpoint - x) + Math.Abs(ypoint - y) * Math.Abs(ypoint - y));

                var scal = len2/len1;

                foreach (Shape s in canvas.Children)
                {

                    var x1pom = ((Line)s).X1;
                    var x2pom = ((Line)s).X2;
                    var y1pom = ((Line)s).Y1;
                    var y2pom = ((Line)s).Y2;

                    ((Line)s).X1 = xpoint + (x1pom - xpoint) * scal;
                    ((Line)s).X2 = xpoint + (x2pom - xpoint) * scal;
                    ((Line)s).Y1 = ypoint + (y1pom - ypoint) * scal;
                    ((Line)s).Y2 = ypoint + (y2pom - ypoint) * scal;
                }
                isover = 0;
            }
        }

        private void Create(object sender, RoutedEventArgs e)
        {

            System.Windows.Shapes.Rectangle rectangle = new();
                rectangle.Width = Math.Abs(1 - 1);
                rectangle.Height = Math.Abs(1 - 1);
                rectangle.StrokeThickness = 5;
                rectangle.Stroke = System.Windows.Media.Brushes.Blue;
                canvas.Children.Add(rectangle);
                Canvas.SetTop(rectangle, Math.Min(1, 1));
                Canvas.SetLeft(rectangle, Math.Min(1, 1));
                
        }

        public void resetColor2()
        {
            this.TworzenieBtn.Background = System.Windows.Media.Brushes.LightGray;
            this.PrzesuniecieBtn.Background = System.Windows.Media.Brushes.LightGray;
            this.ObrutBtn.Background = System.Windows.Media.Brushes.LightGray;
            this.SkalowanieBtn.Background = System.Windows.Media.Brushes.LightGray;

        }

        private void Button_Tworzenie(object sender, RoutedEventArgs e)
        {
            resetColor2();
            this.Type2 = 1;
            this.TworzenieBtn.Background = System.Windows.Media.Brushes.BlueViolet;
        }

        private void Button_Przesuniecie(object sender, RoutedEventArgs e)
        {
            resetColor2();
            this.Type2 = 2;
            this.PrzesuniecieBtn.Background = System.Windows.Media.Brushes.BlueViolet;
        }

        private void Button_Obrut(object sender, RoutedEventArgs e)
        {
            resetColor2();
            this.Type2 = 3;
            this.ObrutBtn.Background = System.Windows.Media.Brushes.BlueViolet;
        }

        private void Button_Skalowanie(object sender, RoutedEventArgs e)
        {
            resetColor2();
            this.Type2 = 4;
            this.SkalowanieBtn.Background = System.Windows.Media.Brushes.BlueViolet;
        }

        private void Button_Vector(object sender, RoutedEventArgs e)
        {
            if (Type2 == 2)
            {
                var xval = Int32.Parse(x.Text);
                var yval = Int32.Parse(y.Text);

                foreach (Shape s in canvas.Children)
                {

                    ((Line)s).X1 = ((Line)s).X1 + xval;
                    ((Line)s).X2 = ((Line)s).X2 + xval;
                    ((Line)s).Y1 = ((Line)s).Y1 + yval;
                    ((Line)s).Y2 = ((Line)s).Y2 + yval;

                }
            }
        }

        private void Button_Obrut2(object sender, RoutedEventArgs e)
        {
            xpoint = Convert.ToInt32(x.Text);
            ypoint = Convert.ToInt32(y.Text);

            var angle = Int32.Parse(sttb2.Text);
            foreach (Shape s in canvas.Children)
            {
                var x1pom = ((Line)s).X1;
                var x2pom = ((Line)s).X2;
                var y1pom = ((Line)s).Y1;
                var y2pom = ((Line)s).Y2;

                ((Line)s).X1 = xpoint + (x1pom - xpoint) * Math.Cos(angle) - (y1pom - ypoint) * Math.Sin(angle);
                ((Line)s).X2 = xpoint + (x2pom - xpoint) * Math.Cos(angle) - (y2pom - ypoint) * Math.Sin(angle);
                ((Line)s).Y1 = ypoint + (x1pom - xpoint) * Math.Sin(angle) + (y1pom - ypoint) * Math.Cos(angle);
                ((Line)s).Y2 = ypoint + (x2pom - xpoint) * Math.Sin(angle) + (y2pom - ypoint) * Math.Cos(angle);

            }
        }


        private void Button_Wsp(object sender, RoutedEventArgs e)
        {
            xpoint = Convert.ToInt32(x.Text);
            ypoint = Convert.ToInt32(y.Text);

            var scal = Int32.Parse(sttb3.Text);

            foreach (Shape s in canvas.Children)
            {

                var x1pom = ((Line)s).X1;
                var x2pom = ((Line)s).X2;
                var y1pom = ((Line)s).Y1;
                var y2pom = ((Line)s).Y2;

                ((Line)s).X1 = xpoint + (x1pom - xpoint) * scal;
                ((Line)s).X2 = xpoint + (x2pom - xpoint) * scal;
                ((Line)s).Y1 = ypoint + (y1pom - ypoint) * scal;
                ((Line)s).Y2 = ypoint + (y2pom - ypoint) * scal;
            }
        }

        private void Button_Save(object sender, RoutedEventArgs e)
        {
            var str = "";
            foreach (Shape s in canvas.Children)
            {
                str += ((Line)s).X1 + " "+ ((Line)s).X2 + " "+ ((Line)s).Y1 + " "+ ((Line)s).Y2 + "\n";
            }

            File.WriteAllText("WriteText.txt", str);
        }

        private void Button_Load(object sender, RoutedEventArgs e)
        {
            canvas.Children.Clear();

            string[] text=File.ReadAllLines("WriteText.txt");
            foreach (string line in text)
            {
                string[] nums= Regex.Split(line, @"\D+");
                var linee = new Line();
                linee.X1 = Int32.Parse(nums[0]);
                linee.X2 = Int32.Parse(nums[1]);
                linee.Y1 = Int32.Parse(nums[2]);
                linee.Y2 = Int32.Parse(nums[3]);

                linee.Stroke = System.Windows.Media.Brushes.Blue;
                linee.StrokeThickness = 3;
                canvas.Children.Add(linee);
                linie.Add(linee);
            }
        }
    }
}

