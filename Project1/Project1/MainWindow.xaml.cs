using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
        private List<Line> linie = new();
        private List<Rectangle> prostokaty = new();
        private List<Ellipse> okregi = new();

        private int Type;   // 1 linia 2 prostokat 3 okrag 4 przeciagniecie 5 zmiana rozmiaru 6 zaznaczenie

        public int _x1 { get; set; }
        public int _y1 { get; set; }
        public int _x2 { get; set; }
        public int _y2 { get; set; }
        public Shape SelectedShape { get; set; }
        public int movex { get; set; }
        public int movey { get; set; }
        public int changex { get; set; }
        public int changey { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            this.Type = 1;
            this.LiniaBtn.Background = Brushes.Blue;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            Point pt = e.GetPosition(canvas);
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Type == 1 || Type == 2 || Type == 3)
            {
                if (_x1 == 0 && _y1 == 0)
                {
                    _x1 = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                    _y1 = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                    this.x1.Text = _x1.ToString();
                    this.y1.Text = _y1.ToString();
                }
                else
                {
                    _x2 = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                    _y2 = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                    this.x2.Text = _x2.ToString();
                    this.y2.Text = _y2.ToString();
                    this.Create(null, null);
                }
            }
            else if (Type == 4)
            {
                foreach (Shape s in canvas.Children)
                {
                    if (s.IsMouseDirectlyOver)
                    {
                        SelectedShape = s;
                        if (SelectedShape != null)
                        {
                            movex = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                            movey = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                        }
                        break;
                    }
                }
            }
            else if (Type == 5)
            {
                foreach (Shape s in canvas.Children)
                {
                    if (s.IsMouseDirectlyOver)
                    {
                        SelectedShape = s;
                        if (SelectedShape != null)
                        {
                            changex = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                            changey = Convert.ToInt32(Mouse.GetPosition(canvas).Y);
                        }
                        break;
                    }
                }
            }
            else if (Type == 6)
            {
                foreach (Shape s in canvas.Children)
                {
                    if (s.IsMouseDirectlyOver)
                    {
                        SelectedShape = s;
                        if (SelectedShape != null)
                        {
                            if (SelectedShape is Line)
                            {
                                _x1 = Convert.ToInt32(((Line)SelectedShape).X1);
                                _y1 = Convert.ToInt32(((Line)SelectedShape).Y1);
                                _x2 = Convert.ToInt32(((Line)SelectedShape).X2);
                                _y2 = Convert.ToInt32(((Line)SelectedShape).Y2);
                            }
                            if (SelectedShape is Rectangle)
                            {
                                _x1 = Convert.ToInt32(Canvas.GetLeft((Rectangle)SelectedShape));
                                _y1 = Convert.ToInt32(Canvas.GetTop((Rectangle)SelectedShape));
                                _x2 = _x1 + Convert.ToInt32(((Rectangle)SelectedShape).Width);
                                _y2 = _y1 + Convert.ToInt32(((Rectangle)SelectedShape).Height);
                            }
                            if (SelectedShape is Ellipse)
                            {
                                var xpom = Convert.ToInt32(Canvas.GetLeft((Ellipse)SelectedShape));
                                var ypom = Convert.ToInt32(Canvas.GetTop((Ellipse)SelectedShape));
                                var width = Convert.ToInt32(((Ellipse)SelectedShape).Width);

                                var midx = xpom + width / 2;
                                var midy = ypom + width / 2;

                                _x1 = midx;
                                _y1 = midy - width / 2;
                                _x2 = midx;
                                _y2 = midy + width / 2;
                            }
                        }
                        break;
                    }
                }

                this.x1.Text = _x1.ToString();
                this.y1.Text = _y1.ToString();
                this.x2.Text = _x2.ToString();
                this.y2.Text = _y2.ToString();
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Type == 4 || Type == 5)
            {
                if(Type == 4)
                {
                    if (SelectedShape is Line)
                    {
                        var xpom = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                        var ypom = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                        var xdif = xpom - movex;
                        var ydif = ypom - movey;

                        ((Line)SelectedShape).X1 = ((Line)SelectedShape).X1 + xdif;
                        ((Line)SelectedShape).X2 = ((Line)SelectedShape).X2 + xdif;
                        ((Line)SelectedShape).Y1 = ((Line)SelectedShape).Y1 + ydif;
                        ((Line)SelectedShape).Y2 = ((Line)SelectedShape).Y2 + ydif;

                        SelectedShape=null;
                    }
                    else if (SelectedShape is Rectangle)
                    {
                        var xpom = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                        var ypom = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                        var xdif = xpom - movex;
                        var ydif = ypom - movey;

                        Canvas.SetTop(SelectedShape, Convert.ToInt32(Canvas.GetTop((Rectangle)SelectedShape) + ydif));
                        Canvas.SetLeft(SelectedShape, Convert.ToInt32(Canvas.GetLeft((Rectangle)SelectedShape) + xdif));

                        SelectedShape = null;
                    }
                    else if (SelectedShape is Ellipse)
                    {
                        var xpom = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                        var ypom = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                        var xdif = xpom - movex;
                        var ydif = ypom - movey;

                        Canvas.SetTop(SelectedShape, Convert.ToInt32(Canvas.GetTop((Ellipse)SelectedShape) + ydif));
                        Canvas.SetLeft(SelectedShape, Convert.ToInt32(Canvas.GetLeft((Ellipse)SelectedShape) + xdif));

                        SelectedShape = null;
                    }
                }
                else if (Type==5)
                {
                    if (SelectedShape is Line)
                    {
                        var xpom = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                        var ypom = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                        var xdif = xpom - changex;
                        var ydif = ypom - changey;

                        ((Line)SelectedShape).X2 = ((Line)SelectedShape).X2 + xdif;
                        ((Line)SelectedShape).Y2 = ((Line)SelectedShape).Y2 + ydif;

                        SelectedShape = null;
                    }
                    else if (SelectedShape is Rectangle)
                    {
                        var xpom = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                        var ypom = Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                        var xdif = xpom - changex;
                        var ydif = ypom - changey;

                        if(SelectedShape.Width > -xdif)
                            SelectedShape.Width += xdif;
                        if (SelectedShape.Height > -ydif)
                            SelectedShape.Height += ydif;

                        SelectedShape = null;
                    }
                    else if (SelectedShape is Ellipse)
                    {
                        var pomx = Convert.ToInt32(Mouse.GetPosition(canvas).X);
                        var pomy= Convert.ToInt32(Mouse.GetPosition(canvas).Y);

                        var xpom = Convert.ToInt32(Canvas.GetLeft((Ellipse)SelectedShape));
                        var ypom = Convert.ToInt32(Canvas.GetTop((Ellipse)SelectedShape));
                        var width = Convert.ToInt32(((Ellipse)SelectedShape).Width);

                        var midx = xpom + width / 2;
                        var midy = ypom + width / 2;

                        var width2 = Math.Abs(Math.Sqrt((midx - pomx) * (midx - pomx) + (midy - pomy) * (midy - pomy)))*2;
                        ((Ellipse)SelectedShape).Width = width2;
                        ((Ellipse)SelectedShape).Height = width2;

                        Canvas.SetTop(SelectedShape, Convert.ToInt32(Canvas.GetTop((Ellipse)SelectedShape) - (width2 - width) / 2));
                        Canvas.SetLeft(SelectedShape, Convert.ToInt32(Canvas.GetLeft((Ellipse)SelectedShape) - (width2 - width) / 2));

                        SelectedShape = null;
                    }
                }
            }
        }

        public void resetColor()
        {
            this.LiniaBtn.Background = Brushes.LightGray;
            this.ProstokatBtn.Background = Brushes.LightGray;
            this.OkragBtn.Background = Brushes.LightGray;

            this.ZaznaczenieBtn.Background = Brushes.LightGray;
            this.PrzeciaganieBtn.Background = Brushes.LightGray;
            this.ZmianaRozmiaruBtn.Background = Brushes.LightGray;

            this.create.Content = "Utwórz";
            this.create.Width = 100;
        }

        private void Button_Linia(object sender, RoutedEventArgs e)
        {
            resetColor();
            this.Type = 1;
            this.LiniaBtn.Background = Brushes.Blue;
        }

        private void Button_Prostokat(object sender, RoutedEventArgs e)
        {
            resetColor();
            this.Type = 2;
            this.ProstokatBtn.Background = Brushes.Blue;
        }

        private void Button_Okrag(object sender, RoutedEventArgs e)
        {
            resetColor();
            this.Type = 3;
            this.OkragBtn.Background = Brushes.Blue;
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            if (Type == 1)
            {
                Line line = new();
                line.X1 = _x1;
                line.X2 = _x2;
                line.Y1 = _y1;
                line.Y2 = _y2;
                line.Stroke = Brushes.Blue;
                line.StrokeThickness = 5;
                canvas.Children.Add(line);
                linie.Add(line);
            }
            else if (Type == 2)
            {
                Rectangle rectangle = new();
                rectangle.Width = Math.Abs(_x1 - _x2);
                rectangle.Height = Math.Abs(_y1 - _y2);
                rectangle.StrokeThickness = 5;
                rectangle.Stroke = Brushes.Blue;
                canvas.Children.Add(rectangle);
                Canvas.SetTop(rectangle, Math.Min(_y1, _y2));
                Canvas.SetLeft(rectangle, Math.Min(_x1, _x2));
                prostokaty.Add(rectangle);
            }
            else if (Type == 3)
            {
                Ellipse ellipse = new();
                ellipse.Width = Math.Abs(Math.Sqrt((_x1 - _x2)*(_x1 - _x2) + (_y1 - _y2)*(_y1 - _y2)));
                ellipse.Height = ellipse.Width;
                ellipse.StrokeThickness = 5;
                ellipse.Stroke = Brushes.Blue;
                canvas.Children.Add(ellipse);

                Point mid = new Point((_x1 + _x2) / 2, (_y1 + _y2) / 2);


                Canvas.SetTop(ellipse, mid.Y - ellipse.Width / 2);
                Canvas.SetLeft(ellipse, mid.X - ellipse.Width / 2);
                okregi.Add(ellipse);
            }

            else if (Type == 6)
            {
                canvas.Children.Remove(SelectedShape);

                if (SelectedShape is Line){
                    Type = 1;
                    Create(null,null);
                    Type = 6;
                }
                else if (SelectedShape is Rectangle)
                {
                    Type = 2;
                    Create(null, null);
                    Type = 6;
                }
                else if (SelectedShape is Ellipse)
                {
                    Type = 3;
                    Create(null, null);
                    Type = 6;
                }
            }


            _x1 = 0;
            _y1 = 0;
            _x2 = 0;
            _y2 = 0;

            this.x1.Text = _x1.ToString();
            this.y1.Text = _y1.ToString();
            this.x2.Text = _x2.ToString();
            this.y2.Text = _y2.ToString();
        }
        private void Button_Przeciaganie(object sender, RoutedEventArgs e)
        {
            resetColor();
            this.Type = 4;
            this.PrzeciaganieBtn.Background = Brushes.BlueViolet;
        }
        private void Button_ZmianaRozmiaru(object sender, RoutedEventArgs e)
        {
            resetColor();
            this.create.Content = "Modyfikuj";
            this.create.Width = 160;
            this.Type = 5;
            this.ZmianaRozmiaruBtn.Background = Brushes.BlueViolet;
        }

        private void Button_Zaznaczenie(object sender, RoutedEventArgs e)
        {
            resetColor();
            this.Type = 6;
            this.ZaznaczenieBtn.Background = Brushes.BlueViolet;
        }

    }
}
