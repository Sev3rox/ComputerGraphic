using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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

namespace Project4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage bitmap2;
        BitmapImage bithelp;
        int Type = 1;
        int pom = 0;
        public MainWindow()
        {
            InitializeComponent();
            this.RedBtn.Background = System.Windows.Media.Brushes.Blue;
        }

        private void chooseFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new();

            if (fileDialog.ShowDialog() == true)
            {
                string ext = System.IO.Path.GetExtension(fileDialog.FileName);
                if (ext == ".jpg" || ext == ".jpeg")
                {
                    ReadJpeg(fileDialog.FileName);
                }
                else
                {
                    MessageBox.Show("                           Not correct file format                     ");
                }
            }
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();
                bitmap2 = bitmapimage;
                this.img.Source = bitmapimage;
                this.img.Width = bitmap.Width;
                this.img.Height = bitmap.Height;
                return bitmapimage;
            }
        }

        public void ReadJpeg(string file)
        {
            bitmap2 = new BitmapImage();
            bitmap2.BeginInit();
            bitmap2.UriSource = new Uri(file);
            bitmap2.EndInit();
            bithelp = bitmap2;
            img.Source = bitmap2;
            this.img.Width = bitmap2.Width;
            this.img.Height = bitmap2.Height;
        }

        private Bitmap BitmapImage2Bitmap(BitmapImage bitmapImage)
        { 
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public void Operation(object sender, RoutedEventArgs e)
        {
            if (bitmap2 != null)
            {
                                this.Reset();

                var name = ((Button)sender).Name;
                var bitmap = new Bitmap((int)bitmap2.Width, (int)bitmap2.Height);
                var val = 0;
                if (value.Text != "")
                    val = Int32.Parse(value.Text);
                var helpbitmap = BitmapImage2Bitmap(bitmap2);

                if (val < 0)
                    val = 0;
                else if (val > 255)
                    val = 255;

                if (name == "AddBtn")
                {
                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);
                            if (Type == 1)
                            {
                                var r = PixelColor.R + val;
                                if (r > 255)
                                    r = 255;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r, PixelColor.G, PixelColor.B));
                            }
                            else if (Type == 2)
                            {
                                var g = PixelColor.G + val;
                                if (g > 255)
                                    g = 255;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(PixelColor.R, g, PixelColor.B));
                            }
                            else if (Type == 3)
                            {
                                var b = PixelColor.B + val;
                                if (b > 255)
                                    b = 255;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(PixelColor.R, PixelColor.G, b));
                            }
                        }
                    }
                }
                else if (name == "MinusBtn")
                {
                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);
                            if (Type == 1)
                            {
                                var r = PixelColor.R - val;
                                if (r < 0)
                                    r = 0;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r, PixelColor.G, PixelColor.B));
                            }
                            else if (Type == 2)
                            {
                                var g = PixelColor.G - val;
                                if (g < 0)
                                    g = 0;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(PixelColor.R, g, PixelColor.B));
                            }
                            else if (Type == 3)
                            {
                                var b = PixelColor.B - val;
                                if (b < 0)
                                    b = 0;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(PixelColor.R, PixelColor.G, b));
                            }
                        }
                    }
                }
                else if (name == "DivideBtn")
                {
                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);
                            if (Type == 1)
                            {
                                var r = PixelColor.R / val;
                                if (r < 0)
                                    r = 0;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r, PixelColor.G, PixelColor.B));
                            }
                            else if (Type == 2)
                            {
                                var g = PixelColor.G / val;
                                if (g < 0)
                                    g = 0;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(PixelColor.R, g, PixelColor.B));
                            }
                            else if (Type == 3)
                            {
                                var b = PixelColor.B / val;
                                if (b < 0)
                                    b = 0;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(PixelColor.R, PixelColor.G, b));
                            }
                        }
                    }
                }
                else if (name == "MultiplyBtn")
                {
                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);
                            if (Type == 1)
                            {
                                var r = PixelColor.R * val;
                                if (r > 255)
                                    r = 255;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r, PixelColor.G, PixelColor.B));
                            }
                            else if (Type == 2)
                            {
                                var g = PixelColor.G * val;
                                if (g > 255)
                                    g = 255;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(PixelColor.R, g, PixelColor.B));
                            }
                            else if (Type == 3)
                            {
                                var b = PixelColor.B * val;
                                if (b > 255)
                                    b = 255;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(PixelColor.R, PixelColor.G, b));
                            }
                        }
                    }
                }
                else if (name == "AddBrBtn")
                {
                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            var r = PixelColor.R + val;
                            if (r > 255)
                                r = 255;

                            var g = PixelColor.G + val;
                            if (g > 255)
                                g = 255;

                            var b = PixelColor.B + val;
                            if (b > 255)
                                b = 255;

                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r, g, b));

                        }
                    }
                }
                else if (name == "MinusBrBtn")
                {
                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            var r = PixelColor.R - val;
                            if (r < 0)
                                r = 0;

                            var g = PixelColor.G - val;
                            if (g < 0)
                                g = 0;

                            var b = PixelColor.B - val;
                            if (b < 0)
                                b = 0;

                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r, g, b));

                        }
                    }
                }
                else if (name == "Gray1Btn")
                {
                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            int avg = (PixelColor.R + PixelColor.G + PixelColor.B) / 3;

                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(avg, avg, avg));

                        }
                    }
                }
                else if (name == "Gray2Btn")
                {
                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            int gray = (int)((0.3 * PixelColor.R + 0.5 * PixelColor.G + 0.2 * PixelColor.B));

                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(gray, gray, gray));

                        }
                    }
                }
                this.BitmapToImageSource(bitmap);
            }
        }

        private void Filtering(object sender, RoutedEventArgs e)
        {
            if (bitmap2 != null)
            {
                this.Reset();
                var name = ((Button)sender).Name;
                var bitmap = new Bitmap((int)bitmap2.Width, (int)bitmap2.Height);
                var helpbitmap = BitmapImage2Bitmap(bitmap2);

                if (name == "Filtr1Btn")
                {
                    int[,] mask = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } }; //change mask here
                    Mask.Content = mask[0, 0] + " " + mask[0, 1] + " " + mask[0, 2] + " " + "\n"
                             + mask[1, 0] + " " + mask[1, 1] + " " + mask[1, 2] + " " + "\n"
                             + mask[2, 0] + " " + mask[2, 1] + " " + mask[2, 2] + " " + "\n";

                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {
                            var r = 0;
                            var g = 0;
                            var b = 0;

                            for (int i = -1; i < 2; i++)
                            {
                                for (int j = -1; j < 2; j++)
                                {
                                    System.Drawing.Color PixelColor = helpbitmap.GetPixel(x + i, y + j);
                                    r += PixelColor.R * mask[i+1, j + 1];
                                    g += PixelColor.G * mask[i + 1, j + 1];
                                    b += PixelColor.B * mask[i + 1, j + 1];
                                }
                            }

                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r / 9, g / 9, b / 9));

                        }
                    }
                }
                else if (name == "Filtr2Btn")
                {
                    int[,] mask = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };  //change mask here
                    Mask.Content = mask[0,0]+" "+ mask[0, 1] + " " + mask[0, 2] + " " +"\n"
                             + mask[1, 0] + " " + mask[1, 1] + " " + mask[1, 2] + " " +"\n"
                             + mask[2, 0] + " " + mask[2, 1] + " " + mask[2, 2] + " " + "\n";

                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {
                            var r = new List<int>();
                            var g = new List<int>();
                            var b = new List<int>();

                            for (int i = -1; i < 2; i++)
                            {
                                for (int j = -1; j < 2; j++)
                                {
                                    System.Drawing.Color PixelColor = helpbitmap.GetPixel(x + i, y + j);
                                    r.Add(PixelColor.R * mask[i + 1, j + 1]);
                                    g.Add(PixelColor.G * mask[i + 1, j + 1]);
                                    b.Add(PixelColor.B * mask[i + 1, j + 1]);
                                }
                            }

                            r.Sort();
                            g.Sort();
                            b.Sort();

                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(r[4], g[4], b[4]));
                        }
                    }
                }
                else if (name == "Filtr3Btn")
                {
                    int width = (int)bitmap2.Width;
                    int height = (int)bitmap2.Height;

                    int BitsPerPixel = System.Drawing.Image.GetPixelFormatSize(helpbitmap.PixelFormat);
                    int OneColorBits = BitsPerPixel / 8;

                    BitmapData bmpData = helpbitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, helpbitmap.PixelFormat);
                    int position;
                    int[,] maskx = new int[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } }; //change masks here
                    int[,] masky = new int[,] { { 1, 2, 1 }, { 0, 0, 0 }, { -1, -2, -1 } };

                    Mask.Content = maskx[0,0]+" "+ maskx[0, 1] + " " + maskx[0, 2] + "     " + masky[0, 0] + " " + masky[0, 1] + " " + masky[0, 2] + "\n"
                             + maskx[1, 0] + " " + maskx[1, 1] + " " + maskx[1, 2] + "     " + masky[1, 0] + " " + masky[1, 1] + " " + masky[1, 2] + "\n"
                             + maskx[2, 0] + " " + maskx[2, 1] + " " + maskx[2, 2] + "     " + masky[2, 0] + " " + masky[2, 1] + " " + masky[2, 2] + "\n";

                    byte Threshold = 128;

                    Bitmap dstBmp = new Bitmap(width, height, helpbitmap.PixelFormat);
                    BitmapData dstData = dstBmp.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, dstBmp.PixelFormat);

                    unsafe
                    {
                        byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                        byte* dst = (byte*)dstData.Scan0.ToPointer();

                        for (int i = 1; i < height - 1; i++)
                        {
                            for (int j = 1; j < width - 1; j++)
                            {
                                int NewX = 0, NewY = 0;

                                for (int ii = 0; ii < 3; ii++)
                                {
                                    for (int jj = 0; jj < 3; jj++)
                                    {
                                        int I = i + ii - 1;
                                        int J = j + jj - 1;
                                        byte Current = *(ptr + (I * width + J) * OneColorBits);
                                        NewX += maskx[ii, jj] * Current;
                                        NewY += masky[ii, jj] * Current;
                                    }
                                }
                                position = ((i * width + j) * OneColorBits);
                                if (NewX * NewX + NewY * NewY > Threshold * Threshold)
                                    dst[position] = dst[position + 1] = dst[position + 2] = 255;
                                else
                                    dst[position] = dst[position + 1] = dst[position + 2] = 0;
                            }
                        }
                    }
                    helpbitmap.UnlockBits(bmpData);
                    dstBmp.UnlockBits(dstData);

                    bitmap = dstBmp;
                }
                else if (name == "Filtr4Btn")
                {
                    int width = (int)bitmap2.Width;
                    int height = (int)bitmap2.Height;

                    int BitsPerPixel = System.Drawing.Image.GetPixelFormatSize(helpbitmap.PixelFormat);
                    int OneColorBits = BitsPerPixel / 8;

                    BitmapData bmpData = helpbitmap.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, helpbitmap.PixelFormat);
                    int position;
                    int[,] mask = new int[,] { { -1, -1, -1 }, { -1, 9, -1 }, { -1, -1, -1 } }; //change mask here
                    byte Threshold = 128;

                    Mask.Content = mask[0,0]+" "+ mask[0, 1] + " " + mask[0, 2] + " " +"\n"
                             + mask[1, 0] + " " + mask[1, 1] + " " + mask[1, 2] + " " +"\n"
                             + mask[2, 0] + " " + mask[2, 1] + " " + mask[2, 2] + " " + "\n";

                    Bitmap dstBmp = new Bitmap(width, height, helpbitmap.PixelFormat);
                    BitmapData dstData = dstBmp.LockBits(new System.Drawing.Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, dstBmp.PixelFormat);

                    unsafe
                    {
                        byte* ptr = (byte*)bmpData.Scan0.ToPointer();
                        byte* dst = (byte*)dstData.Scan0.ToPointer();

                        for (int i = 1; i < height - 1; i++)
                        {
                            for (int j = 1; j < width - 1; j++)
                            {
                                int NewX = 0;

                                for (int ii = 0; ii < 3; ii++)
                                {
                                    for (int jj = 0; jj < 3; jj++)
                                    {
                                        int I = i + ii - 1;
                                        int J = j + jj - 1;
                                        byte Current = *(ptr + (I * width + J) * OneColorBits);
                                        NewX += mask[ii, jj] * Current;
                                    }
                                }
                                position = ((i * width + j) * OneColorBits);
                                if (NewX * NewX> Threshold * Threshold)
                                    dst[position] = dst[position + 1] = dst[position + 2] = 255;
                                else
                                    dst[position] = dst[position + 1] = dst[position + 2] = 0;
                            }
                        }
                    }
                    helpbitmap.UnlockBits(bmpData);
                    dstBmp.UnlockBits(dstData);

                    bitmap = dstBmp;
                }
                else if (name == "Filtr5Btn")
                {
                    int[,] mask = new int[,] { { 1, 1, 1 }, { 1, 1, 1 }, { 1, 1, 1 } };  //change mask here
                    Mask.Content = mask[0, 0] + " " + mask[0, 1] + " " + mask[0, 2] + " " + "\n"
                             + mask[1, 0] + " " + mask[1, 1] + " " + mask[1, 2] + " " + "\n"
                             + mask[2, 0] + " " + mask[2, 1] + " " + mask[2, 2] + " " + "\n";

                    for (int x = 2; x < (int)bitmap2.Width - 2; x++)
                    {
                        for (int y = 2; y < (int)bitmap2.Height - 2; y++)
                        {

                            System.Drawing.Color prevx = helpbitmap.GetPixel(x - 2, y);
                            System.Drawing.Color nextx = helpbitmap.GetPixel(x + 2, y);
                            System.Drawing.Color prevy = helpbitmap.GetPixel(x, y-2);
                            System.Drawing.Color nexty = helpbitmap.GetPixel(x, y+2);

                            int avgR = (int)((prevx.R * mask[0, 1] + nextx.R * mask[2, 1] + prevy.R * mask[1, 0] + nexty.R * mask[1, 2]) / 4);
                            int avgG = (int)((prevx.G * mask[0, 1] + nextx.G * mask[2, 1] + prevy.G * mask[1, 0] + nexty.G * mask[1, 2]) / 4);
                            int avgB = (int)((prevx.B * mask[0, 1] + nextx.B * mask[2, 1] + prevy.B * mask[1, 0] + nexty.B * mask[1, 2]) / 4);


                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(avgR, avgG, avgB));
                        }
                    }
                }
                this.BitmapToImageSource(bitmap);
            }
        }  

        public void resetColor()
        {
            this.RedBtn.Background = System.Windows.Media.Brushes.LightGray;
            this.GreenBtn.Background = System.Windows.Media.Brushes.LightGray;
            this.BlueBtn.Background = System.Windows.Media.Brushes.LightGray;
        }

        public void Button_Red(object sender, RoutedEventArgs e)
        {
            resetColor();
            this.Type = 1;
            this.RedBtn.Background = System.Windows.Media.Brushes.Blue;
        }

        public void Button_Green(object sender, RoutedEventArgs e)
        {
            resetColor();
            this.Type = 2;
            this.GreenBtn.Background = System.Windows.Media.Brushes.Blue;
        }

        public void Button_Blue(object sender, RoutedEventArgs e)
        {
            resetColor();
            this.Type = 3;
            this.BlueBtn.Background = System.Windows.Media.Brushes.Blue;
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Reset()
        {
            Mask.Content = "";
            bitmap2 = bithelp;
            img.Source = bitmap2;
            this.img.Width = bitmap2.Width;
            this.img.Height = bitmap2.Height;
        }
    }
}
