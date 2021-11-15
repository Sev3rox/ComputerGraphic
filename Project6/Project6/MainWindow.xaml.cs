using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
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
        private int index=-1;
        List<System.Windows.Point> points=new List<System.Windows.Point>();
        List<System.Windows.Point> backpoints = new List<System.Windows.Point>();
        Bitmap image = new Bitmap(750,650);

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            canvas.Focusable = true;

            Loaded += (x, y) => Keyboard.Focus(canvas);
            Button_Tworzenie(null,null);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Type2 == 1)
            {

                if (Convert.ToInt32(sttb.Text) > 0)
                st = Convert.ToInt32(sttb.Text);

                var x = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                var y = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                points.Add(new System.Windows.Point(x, y));
                count++;

                if (count >= st + 1)
                {
                    canvas.Children.Clear();
                    drawCasteljau();
                    backpoints.Clear();
                    count = 0;
                    foreach (var it in points)
                        backpoints.Add(it);
                    points.Clear();

                    var str = "";
                    pointslbl.Content =str;
                }
            }
            else
            {
                index = -1;
                var x = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                var y = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                double min = 9999;

                int i = 0;

                foreach (var it in backpoints)
                {
                    var res=Math.Abs(it.X - x) + Math.Abs(it.Y - y);
                    if(res<min)
                    {
                        min = res;
                        index = i;
                    }
                    i++;
                }
            }
            
        }

        private void drawCasteljau()
        {
            System.Windows.Point tmp;
            for (double t = 0; t < 1; t += 0.005) {
                tmp = getCasteljauPoint(points.Count - 1, 0, t);
                var rec= new System.Windows.Shapes.Rectangle();
                rec.Width = 2;
                rec.Height = 2;
                rec.Fill = System.Windows.Media.Brushes.Black;
                canvas.Children.Add(rec);
                Canvas.SetTop(rec, tmp.Y);
                Canvas.SetLeft(rec, tmp.X);
            }
        }

        private System.Windows.Point getCasteljauPoint(int r, int i, double t)
        {
            if (r == 0) return points[i];

            System.Windows.Point p1 = getCasteljauPoint(r - 1, i, t);
            System.Windows.Point p2 = getCasteljauPoint(r - 1, i + 1, t);

            return new System.Windows.Point((int)((1 - t) * p1.X + t * p2.X), (int)((1 - t) * p1.Y + t * p2.Y));
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (backpoints.Count>=st+1 && Type2==2) {
                var x = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                var y = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                foreach (var it in backpoints)
                    points.Add(it);
                points[index] = new System.Windows.Point(x, y);

                canvas.Children.Clear();
                drawCasteljau();
                count = 0;
                backpoints.Clear();
                foreach (var it in points)
                    backpoints.Add(it);
                points.Clear();

                index = -1;

                var str = "";
                pointslbl.Content = str;
            }
        }
            private void Canvas_MouseMove(object sender, MouseEventArgs e)
            {
                if (backpoints.Count >= st + 1 && Type2 == 2 && index!=-1)
                {
                    var x = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                    var y = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                    foreach (var it in backpoints)
                        points.Add(it);
                    points[index] = new System.Windows.Point(x, y);

                    canvas.Children.Clear();
                    drawCasteljau();
                    backpoints.Clear();
                    foreach (var it in points)
                        backpoints.Add(it);
                    points.Clear();
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
            this.PrzeciaganieBtn.Background = System.Windows.Media.Brushes.LightGray;

        }

        private void Button_Tworzenie(object sender, RoutedEventArgs e)
        {
            resetColor2();
            this.Type2 = 1;
            this.TworzenieBtn.Background = System.Windows.Media.Brushes.BlueViolet;
        }

        private void Button_Przeciaganie(object sender, RoutedEventArgs e)
        {
            resetColor2();
            this.Type2 = 2;
            this.PrzeciaganieBtn.Background = System.Windows.Media.Brushes.BlueViolet;
        }

    }
}

