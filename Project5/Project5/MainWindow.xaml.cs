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

namespace Project5
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
            img.Source = bitmap2;
            this.img.Width = bitmap2.Width;
            this.img.Height = bitmap2.Height;

            var bitmap = new Bitmap((int)bitmap2.Width, (int)bitmap2.Height);
            var helpbitmap = BitmapImage2Bitmap(bitmap2);

            for (int x = 0; x < (int)bitmap2.Width; x++)
            {
                for (int y = 0; y < (int)bitmap2.Height; y++)
                {
                    System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                    int avg = (PixelColor.R + PixelColor.G + PixelColor.B) / 3;

                    bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(avg, avg, avg));

                }
            }

            this.BitmapToImageSource(bitmap);
            bithelp = bitmap2;
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

        public void Histogram(object sender, RoutedEventArgs e)
        {
            if (bitmap2 != null)
            {
                this.Reset();

                var name = ((Button)sender).Name;
                var bitmap = new Bitmap((int)bitmap2.Width, (int)bitmap2.Height);
                var helpbitmap = BitmapImage2Bitmap(bitmap2);

                int min = 255;
                int max = 0;
                if (name == "hist1")
                {
                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            if (PixelColor.G > max)
                                max = PixelColor.G;

                            if (PixelColor.G < min)
                                min=PixelColor.G;
                        }
                    }



                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            double val = (double)((double)((double)(PixelColor.G - min) / (double)(max - min)) * 255);

                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb((int)val, (int)val, (int)val));
                        }
                    }

                    txt.Content = "was: "+min+"-"+max+"\nis: 0-255";
                }
                else if (name == "hist2")
                {
                    var val =new int[256];

                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            val[PixelColor.G]++;
                        }
                    }

                    int[] LUT = calculateLUT(val, (int)(bitmap2.Width * bitmap2.Height));

                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(LUT[PixelColor.G], LUT[PixelColor.G], LUT[PixelColor.G]));
                            val[LUT[PixelColor.G]]++;
                        }
                    }

                }

                this.BitmapToImageSource(bitmap);
            }
        }


        private int[] calculateLUT(int[] values, int size)
        {
            double minValue = 0;
            for (int i = 0; i < 256; i++)
            {
                if (values[i] != 0)
                {
                    minValue = values[i];
                    break;
                }
            }

            int[] result = new int[256];
            double sum = 0;
            for (int i = 0; i < 256; i++)
            {
                sum += values[i];
                result[i] = (int)(((sum - minValue) / (size - minValue)) * 255.0);
            }

            return result;
        }


        private void Bin(object sender, RoutedEventArgs e)
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

                if (name == "bin1")
                {
                    if (val < 0)
                        val = 0;
                    else if (val > 255)
                        val = 255;

                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            if (PixelColor.G >= val)
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, 255, 255));
                            else
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                        }
                    }
                }
                else if (name == "bin2")
                {
                    if (val < 0)
                        val = 0;
                    else if (val > 100)
                        val = 100;

                    var vall = new int[256];

                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            vall[PixelColor.G]++;
                        }
                    }

                    var count=0;
                    var place = 0;
                    var prc = (double)((double)(bitmap2.Width * bitmap2.Height) * val / 100);

                    for (int i = 0; i < 255; i++)
                    {
                        count += vall[i];
                        if (count >= prc)
                        {
                            place = i;
                            break;
                        }
                    }


                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            if (PixelColor.G >= place)
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, 255, 255));
                            else
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                        }
                    }

                    txt.Content = "value= "+place;
                }
                else if (name == "bin3")
                {
                    var vall = new int[256];

                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            vall[PixelColor.G]++;
                        }
                    }

                    double valll = 0;
                    for (int i = 0; i < 255; i++)
                    {
                        if (vall[i] != 0)
                        {
                            double pomm = (double)((double)vall[i] / (double)(bitmap2.Width * bitmap2.Height));
                            var pom = Math.Log(pomm)* pomm;
                            valll -= pom;
                        }
                    }
                    ///////////////////////////////////////////////////////////////////
                    var vallp = new int[256];

                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            vallp[PixelColor.G]++;
                        }
                    }

                    var count = 0;
                    var place = 0;
                    var prc = (double)((double)(bitmap2.Width * bitmap2.Height) * 50 / 100);

                    for (int i = 0; i < 255; i++)
                    {
                        count += vallp[i];
                        if (count >= prc)
                        {
                            place = i;
                            break;
                        }
                    }

                    valll = place+valll;

                    for (int x = 0; x < (int)bitmap2.Width; x++)
                    {
                        for (int y = 0; y < (int)bitmap2.Height; y++)
                        {
                            System.Drawing.Color PixelColor = helpbitmap.GetPixel(x, y);

                            if (PixelColor.G >= valll)
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, 255, 255));
                            else
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(0, 0, 0));
                        }
                    }

                    txt.Content = "value= " + valll;
                }
                this.BitmapToImageSource(bitmap);
            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void Reset()
        {
            txt.Content = "";
            bitmap2 = bithelp;
            img.Source = bitmap2;
            this.img.Width = bitmap2.Width;
            this.img.Height = bitmap2.Height;
        }
    }
}

