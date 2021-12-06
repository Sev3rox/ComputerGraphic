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

                    bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(PixelColor.R, PixelColor.G, PixelColor.B));

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




        public void Bin(object sender, RoutedEventArgs e)
        {
            if (bitmap2 != null)
            {

                this.Reset();

                var name = ((Button)sender).Name;
                var bitmap = new Bitmap((int)bitmap2.Width, (int)bitmap2.Height);
                var helpbitmap = BitmapImage2Bitmap(bitmap2);
                int count = 0;

                for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                {
                    for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                    {
                        
                            bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, 255, 255));
                    }
                }

                if (name == "red")
                {

                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {

                            var pom = helpbitmap.GetPixel(x, y);
                            float hue = pom.GetHue();
                            float saturation = pom.GetSaturation();
                            float lightness = pom.GetBrightness();
                            if ((hue < 17||hue>340) && saturation > 0.10 && lightness > 0.10)
                            {
                                count++;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(pom.R, pom.G, pom.B));
                            }

                            var result = count / (bitmap2.Height * bitmap2.Width) * 100;

                            res.Content = "Result: " + String.Format("{0:0.0000}", result) + "%";
                        }
                    }

                    this.BitmapToImageSource(bitmap);
                }

                else if (name == "green")
                {

                    for (int x = 1; x < (int)bitmap2.Width-1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height-1; y++)
                        {

                            var pom = helpbitmap.GetPixel(x , y);
                            float hue = pom.GetHue();
                            float saturation = pom.GetSaturation();
                            float lightness = pom.GetBrightness();
                            if (hue >= 70 && hue < 165 &&saturation>0.10&&lightness>0.10)
                            {
                                count++;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(pom.R, pom.G, pom.B));
                            }

                            var result = count / (bitmap2.Height* bitmap2.Width)*100;

                            res.Content = "Result: "+String.Format("{0:0.0000}", result) + "%";
                        }
                    }

                    this.BitmapToImageSource(bitmap);
                }

                else if (name == "blue")
                {

                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {

                            var pom = helpbitmap.GetPixel(x, y);
                            float hue = pom.GetHue();
                            float saturation = pom.GetSaturation();
                            float lightness = pom.GetBrightness();
                            if (hue < 270 && hue >= 165 && saturation > 0.10 && lightness > 0.10)
                            {
                                count++;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(pom.R, pom.G, pom.B));
                            }

                            var result = count / (bitmap2.Height * bitmap2.Width) * 100;

                            res.Content = "Result: " + String.Format("{0:0.0000}", result) + "%";
                        }
                    }

                    this.BitmapToImageSource(bitmap);
                }

                else if (name == "yellow")
                {

                    for (int x = 1; x < (int)bitmap2.Width - 1; x++)
                    {
                        for (int y = 1; y < (int)bitmap2.Height - 1; y++)
                        {

                            var pom = helpbitmap.GetPixel(x, y);
                            float hue = pom.GetHue();
                            float saturation = pom.GetSaturation();
                            float lightness = pom.GetBrightness();
                            if (hue < 70 && hue >= 40 && saturation > 0.10 && lightness > 0.10)
                            {
                                count++;
                                bitmap.SetPixel(x, y, System.Drawing.Color.FromArgb(pom.R, pom.G, pom.B));
                            }

                            var result = count / (bitmap2.Height * bitmap2.Width) * 100;

                            res.Content = "Result: " + String.Format("{0:0.0000}", result) + "%";
                        }
                    }

                    this.BitmapToImageSource(bitmap);
                }
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

